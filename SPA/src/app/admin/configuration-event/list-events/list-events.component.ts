import { Component } from "@angular/core";
import { EvenementDto } from "../../../DTOS/EvenementDto ";
import { ServiceResponse } from "../../../DTOS/ServiceResponse";
import { EventConfigurationService } from "../../../Services/event-configuration.service";
import { ModalService } from "../../../Services/modal.service";
import { EditEventsComponent } from "../edit-events/edit-events.component";
import { ActivatedRoute, Params } from "@angular/router";
import { SuccessToastComponent } from "../../../toast/success-toast/success-toast.component";
import { ToastrService } from "ngx-toastr";
import { ErrorToastComponent } from "../../../toast/error-toast/error-toast.component";


@Component({
  selector: 'app-list-events',
  templateUrl: './list-events.component.html',
  styleUrl: './list-events.component.css'
})
export class ListEventsComponent {
  Evenements: EvenementDto[] = [];
  constructor(private  eventConfigurationService: EventConfigurationService,
    private modalService: ModalService,
    private route: ActivatedRoute,
    private toastr: ToastrService){}
  ngOnInit() {
    this.route.queryParams.subscribe((params: Params) => {
      const category: number = params['category'];
      if (category) {
        this.eventConfigurationService.getAllEvent(category).subscribe({
          next: (response: ServiceResponse<EvenementDto[]>) => {
            this.Evenements = response.data || [];
          },
          error: (error) => {
            console.error(error.message);
          }
        });
      }
      else {
        this.Evenements = [];
      }

    })

  }

  onModalSubmit(response: ServiceResponse<EvenementDto>) {
    if (response.data) {
      this.Evenements = this.Evenements.map(event => {
          if (event.id === response.data!.id) {
              return response.data!;
          } else {
              return event;
          }
      }).filter(event => event !== undefined) as EvenementDto[];
      this.toastr.success(response.message, 'Success', {
        toastComponent: SuccessToastComponent
      });
  }
  };

  openEditModal(model: EvenementDto) {
    this.modalService.openModal(EditEventsComponent, { model: model });
  }
}
