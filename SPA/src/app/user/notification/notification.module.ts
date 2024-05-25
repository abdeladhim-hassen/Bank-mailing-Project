import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NotificationRoutingModule } from './notification-routing.module';
import { NotificationManagementComponent } from './notification-management/notification-management.component';
import { AddNotificationComponent } from './add-notification/add-notification.component';
import { EditNotificationComponent } from './edit-notification/edit-notification.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    NotificationManagementComponent,
    AddNotificationComponent,
    EditNotificationComponent
  ],
  imports: [
    CommonModule,
    NotificationRoutingModule,
    ReactiveFormsModule
  ]
})
export class NotificationModule { }
