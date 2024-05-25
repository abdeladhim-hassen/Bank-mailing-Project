import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfigurationEventRoutingModule } from './configuration-event-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ListEventsComponent } from './list-events/list-events.component';
import { EditEventsComponent } from './edit-events/edit-events.component';
import { TemplateManagementComponent } from './template-management/template-management.component';
import { EditTemplateComponent } from './edit-template/edit-template.component';



@NgModule({
  declarations: [
    ListEventsComponent,
    EditEventsComponent,
    TemplateManagementComponent,
    EditTemplateComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ConfigurationEventRoutingModule,
  ]
})
export class ConfigurationEventModule { }
