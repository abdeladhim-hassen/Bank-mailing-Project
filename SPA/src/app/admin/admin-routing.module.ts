import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path:"configuration-event",
    loadChildren: async () => (await (import('./configuration-event/configuration-event.module'))).ConfigurationEventModule,
  },
  {
    path:"user-management",
    loadChildren: async () => (await (import('./user-management/user-management.module'))).UserManagementModule,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
