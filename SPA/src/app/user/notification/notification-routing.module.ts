import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotificationManagementComponent } from './notification-management/notification-management.component';
import { AddNotificationComponent } from './add-notification/add-notification.component';

const routes: Routes = [
  { path: '', redirectTo: 'notification-management', pathMatch: 'full' },
  {
    path: 'notification-management',
    component: NotificationManagementComponent,
  },
  {
    path: 'add-notification',
    component: AddNotificationComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationRoutingModule { }
