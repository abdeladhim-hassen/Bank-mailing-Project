
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListEventsComponent } from './list-events/list-events.component';
import { TemplateManagementComponent } from './template-management/template-management.component';
const routes: Routes = [
  {
    path: 'notifications-update',
    component: TemplateManagementComponent,
  },
  {
    path: 'events-update',
    component: ListEventsComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigurationEventRoutingModule { }
