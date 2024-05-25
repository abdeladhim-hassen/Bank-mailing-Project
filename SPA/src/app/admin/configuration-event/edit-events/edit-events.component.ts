import { Component, EventEmitter, Output } from '@angular/core';
import { ModalService } from '../../../Services/modal.service';
import { EvenementDto } from '../../../DTOS/EvenementDto ';
import { Subscription } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EventConfigurationService } from '../../../Services/event-configuration.service';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';


@Component({
  selector: 'app-edit-events',
  templateUrl: './edit-events.component.html',
  styleUrl: './edit-events.component.css'
})
export class EditEventsComponent {
  @Output() submit = new EventEmitter<ServiceResponse<EvenementDto>>();
  model: EvenementDto | null = null;
  visible = false;
  private modalStateSubscription: Subscription | undefined;

  heureEnvoi = new FormControl<Date>( new Date(),[Validators.required]);

  canal = new FormControl('', [Validators.required, Validators.pattern(/^(SMS|WHATSAPP|EMAIL)$/)]);

  EventForm = new FormGroup({
    heureEnvoi: this.heureEnvoi,
    canal: this.canal,
  });

  constructor(private modalService: ModalService, private  eventConfigurationService: EventConfigurationService,) {}

  ngOnInit(): void {
    this.reset();
  }

  ngOnDestroy(): void {
    if (this.modalStateSubscription) {
      this.modalStateSubscription.unsubscribe();
    }
  }


  closeModal() {
    this.modalService.closeModal();
    this.visible = false;
  }

  public onSubmit() {
   const formData = this.EventForm.value as EvenementDto
    if (this.model) {
      this.model.canal = formData.canal
      this.model.heureEnvoi = formData.heureEnvoi
      this.EditEvent(this.model)
      this.modalService.closeModal();
      this.visible = false;
      this.model = null
    }
  }


  reset() {
    this.modalStateSubscription = this.modalService.getModalState().subscribe((modalData: any) => {
      if (modalData && modalData.data?.model) {
        this.model = modalData.data.model;
        this.visible = true;
          this.heureEnvoi.setValue(this.model?.heureEnvoi ||  new Date());
          this.canal.setValue(this.model?.canal || 'true');
      }
    });
  }

  private EditEvent(Event: EvenementDto){
    return this.eventConfigurationService.updateEvent(Event).subscribe({
      next: (response: ServiceResponse<EvenementDto>) => {
        this.submit.emit(response);
      },
      error: (error) => {
        this.submit.emit(error);
      }
    });
  }

}
