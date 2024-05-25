import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChangePasswordComponent } from './change-password/change-password.component';

const routes: Routes = [
  {
    path: 'change-password',
    component: ChangePasswordComponent,

  },
  {
    path:"notification",
    loadChildren: async () => (await (import('./notification/notification.module'))).NotificationModule,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
