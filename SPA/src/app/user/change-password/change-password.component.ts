import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmPasswordValidators } from '../../Validators/confirm-password';
import { UserService } from '../../Services/user.service';
import { UserChangePassword } from '../../DTOS/UserChangePassword';
import { ServiceResponse } from '../../DTOS/ServiceResponse';
import { ToastrService } from 'ngx-toastr';
import { ErrorToastComponent } from '../../toast/error-toast/error-toast.component';
import { SuccessToastComponent } from '../../toast/success-toast/success-toast.component';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  submissionInProgress = false;
  password = new FormControl('', [Validators.required, Validators.minLength(6)]);
  confirmPassword = new FormControl('', [Validators.required]);

  ChangePasswordForm = new FormGroup({
    password: this.password,
    confirmPassword: this.confirmPassword,

  },[ConfirmPasswordValidators.match('password','confirmPassword')]);
  constructor(private userService: UserService,
    private toastr: ToastrService
  ) {}

  Change()
  {
    this.submissionInProgress = true;
    const formdata = this.ChangePasswordForm.value as UserChangePassword
    this.userService.changePassword(formdata).subscribe({
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
