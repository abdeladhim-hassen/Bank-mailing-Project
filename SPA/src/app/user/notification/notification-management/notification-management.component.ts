import { NotificationService } from './../../../Services/notification.service';
import { Component } from '@angular/core';
import { NotificationDto } from '../../../DTOS/NotificationDto';
import { ModalService } from '../../../Services/modal.service';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';
import { EditNotificationComponent } from '../edit-notification/edit-notification.component';
import { ErrorToastComponent } from '../../../toast/error-toast/error-toast.component';
import { ToastrService } from 'ngx-toastr';
import { SuccessToastComponent } from '../../../toast/success-toast/success-toast.component';

@Component({
  selector: 'app-notification-management',
  templateUrl: './notification-management.component.html',
  styleUrl: './notification-management.component.css'
})
export class NotificationManagementComponent {
  notifications: NotificationDto[] = [];
  constructor(private  notificationService: NotificationService,
    private toastr: ToastrService,
    private modalService: ModalService){}
  ngOnInit() {
    this.notificationService.getAllNotifications().subscribe({
      next: (response: ServiceResponse<NotificationDto[]>) => {
        this.notifications = response.data || [];
      },
      error: (error) => {
        console.error(error.message);
      }
    });
  }
  onModalSubmit(response: ServiceResponse<NotificationDto>) {
    if (response.data) {
      this.notifications = this.notifications.map(notification => {
          if (notification.id === response.data!.id) {
              return response.data!;
          } else {
              return notification;
          }
      }).filter(notification => notification !== undefined) as NotificationDto[];
      this.toastr.success(response.message, 'Success', {
        toastComponent: SuccessToastComponent
      });
  }
  };


  openEditModal(model: NotificationDto) {
    this.modalService.openModal(EditNotificationComponent, { model: model });
  }
}
