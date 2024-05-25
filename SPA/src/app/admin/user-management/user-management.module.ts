import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserManagementRoutingModule } from './user-management-routing.module';
import { AddUserComponent } from './add-user/add-user.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserManagementComponent } from './user-management/user-management.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { EventBlockerDirective } from './directives/event-blocker.directive';
import { NgxMaskDirective, provideEnvironmentNgxMask } from 'ngx-mask';


@NgModule({
  declarations: [
    EventBlockerDirective,
    AddUserComponent,
    UserManagementComponent,
    EditUserComponent
  ],
  imports: [
    NgxMaskDirective,
    CommonModule,
    ReactiveFormsModule,
    UserManagementRoutingModule
  ],
  providers: [
    provideEnvironmentNgxMask(),
  ]
})
export class UserManagementModule { }
