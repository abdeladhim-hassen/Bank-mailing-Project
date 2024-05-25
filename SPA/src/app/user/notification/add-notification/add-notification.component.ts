import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '../../../Services/notification.service';
import { AddNotification } from '../../../DTOS/AddNotification';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';
import { ErrorToastComponent } from '../../../toast/error-toast/error-toast.component';
import { SuccessToastComponent } from '../../../toast/success-toast/success-toast.component';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-notification',
  templateUrl: './add-notification.component.html',
  styleUrl: './add-notification.component.css'
})
export class AddNotificationComponent {
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


  submissionInProgress = false;

  constructor(private notificationService: NotificationService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  register() {
    this.submissionInProgress = true;

    const formData = this.NotificationForm.value as AddNotification;
    this.notificationService.createNotification(formData).subscribe({
      next: (response: ServiceResponse<number>) => {
        this.toastr.success(response.message, 'Success', {
          toastComponent: SuccessToastComponent
        })
        this.router.navigate(['notification/notification-management']);
      },
      error: (error: ServiceResponse<number>) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      },
      complete: () => {
        this.submissionInProgress = false;
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
