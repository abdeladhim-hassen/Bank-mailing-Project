import { ModalService } from './../../../Services/modal.service';
import { Component, EventEmitter, Output } from '@angular/core';
import { NotificationDto } from '../../../DTOS/NotificationDto';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';
import { Subscription } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '../../../Services/notification.service';
import { UpdateNotificationDto } from '../../../DTOS/UpdateNotificationDto';

@Component({
  selector: 'app-edit-notification',
  templateUrl: './edit-notification.component.html',
  styleUrl: './edit-notification.component.css'
})
export class EditNotificationComponent {
  @Output() submit = new EventEmitter<ServiceResponse<NotificationDto>>();
  model: NotificationDto | null = null;
  visible = false;
  private modalStateSubscription: Subscription | undefined;
  hashtags: string[] = ['[Nom Client]', '[Prenom Client]'];
  isDragging: boolean = false;
  name = new FormControl('', [Validators.required, Validators.minLength(8)]);
  sendDate = new FormControl(new Date(), [Validators.required]);
  emailBody = new FormControl('', [Validators.required]);
  smsMessage = new FormControl('', [Validators.required]);
  whatsMessage = new FormControl('', [Validators.required]);

  NotificationForm = new FormGroup({
    name: this.name,
    sendDate: this.sendDate,
    emailBody: this.emailBody,
    smsMessage: this.smsMessage,
    whatsMessage: this.whatsMessage,
  });

  constructor(private notificationService: NotificationService,
    private modalService: ModalService) {}
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
   const formData = this.NotificationForm.value as UpdateNotificationDto
    if (this.model) {
      formData.id = this.model.id
      this.EditNotification(formData)
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
          this.sendDate.setValue(this.model?.sendDate ||  new Date());
          this.name.setValue(this.model?.name || '');
          this.emailBody.setValue(this.model?.emailBody ||  '');
          this.smsMessage.setValue(this.model?.smsMessage ||  '');
          this.whatsMessage.setValue(this.model?.whatsMessage ||  '');
      }
    });
  }

  private EditNotification(Event: UpdateNotificationDto){
    return this.notificationService.updateNotification(Event).subscribe({
      next: (response: ServiceResponse<NotificationDto>) => {
        this.submit.emit(response);
      },
      error: (error) => {
        this.submit.emit(error);
      }
    });
  }

  insertHashtag(hashtag: string, control: FormControl, textareaId: string) {
    if (!this.isDragging && control.value) {
      const textarea = document.getElementById(textareaId) as HTMLTextAreaElement;
      const startPos = textarea.selectionStart;
      const endPos = textarea.selectionEnd;
      const textBeforeCursor = control.value.substring(0, startPos);
      const textAfterCursor = control.value.substring(endPos);
      control.setValue(textBeforeCursor + `#${hashtag} ` + textAfterCursor);
      textarea.focus();
      textarea.setSelectionRange(startPos + hashtag.length + 2, startPos + hashtag.length + 2);
    }
  }

  onDragStart(event: DragEvent, hashtag: string) {
    event.dataTransfer!.setData('text/plain', `#${hashtag} `);
    this.isDragging = true;
  }

  onDragEnd() {
    this.isDragging = false;
  }

}
