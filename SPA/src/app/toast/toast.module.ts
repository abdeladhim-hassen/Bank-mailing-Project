import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorToastComponent } from './error-toast/error-toast.component';
import { SuccessToastComponent } from './success-toast/success-toast.component';



@NgModule({
  declarations: [
    ErrorToastComponent,
    SuccessToastComponent
  ],
  imports: [
    CommonModule
  ],
  exports:[
    ErrorToastComponent,
    SuccessToastComponent
  ]
})
export class ToastModule { }
