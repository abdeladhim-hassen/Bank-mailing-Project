import { Component } from '@angular/core';
import { ServiceResponse } from '../../DTOS/ServiceResponse';
import { AuthService } from '../../Services/auth.service';
import { UserLogin } from '../../DTOS/UserLogin';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorToastComponent } from '../../toast/error-toast/error-toast.component';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  user: UserLogin  = {
    login : '',
    password: ''
  };


  inSubmission = false

  constructor(private auth: AuthService, private toastr: ToastrService) { }

  login() {
    this.inSubmission = true;

    this.auth.login(this.user).subscribe({
      next: (response: ServiceResponse<string>) => {
        this.inSubmission = false;
        setTimeout(() => {
          location.reload();
        }, 1);
      },
      error: (error) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      },
      complete: () => {
        // Facultatif : Gérez la logique de complétion si nécessaire
      }
    });
  }

}
