import { Component } from '@angular/core';
import { ServiceResponse } from '../../DTOS/ServiceResponse';
import { UserService } from '../../Services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmPasswordValidators } from '../../Validators/confirm-password';
import { ResetPassword } from '../../DTOS/ResetPassword ';
import { ToastrService } from 'ngx-toastr';
import { SuccessToastComponent } from '../../toast/success-toast/success-toast.component';
import { ErrorToastComponent } from '../../toast/error-toast/error-toast.component';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  submissionInProgress = false;
  email = '';
  show = false
  verificationCode = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required, Validators.minLength(6)]);
  confirmPassword = new FormControl('', [Validators.required]);
  ResetForm = new FormGroup({
    verificationCode: this.verificationCode,
    password: this.password,
    confirmPassword: this.confirmPassword,

  },[ConfirmPasswordValidators.match('password','confirmPassword')]);
  constructor(private userService: UserService,
    private toastr: ToastrService
  ) {}




  send(): void {

    this.submissionInProgress = true;

    this.userService.sendVerificationCode(this.email).subscribe({
      next: (response: ServiceResponse<boolean>) => {
        this.submissionInProgress = false;
        this.show = response.data || false
      },
      error: (error) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
        this.submissionInProgress = false;
      },
      complete: () => {
      }
    });
  }


  Change()
  {
    const formdata = this.ResetForm.value as ResetPassword
    formdata.email = this.email
    this.userService.resetPasswordWithVerificationCode(formdata).subscribe({
      next: (response: ServiceResponse<boolean>) => {
        this.submissionInProgress = false;
        this.toastr.success(response.message, 'Success', {
          toastComponent: SuccessToastComponent
        })
      },
      error: (error: ServiceResponse<boolean>) => {
        this.submissionInProgress = false;
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      },
      complete: () => {
        this.submissionInProgress = false;
      }
    });
  }

}
