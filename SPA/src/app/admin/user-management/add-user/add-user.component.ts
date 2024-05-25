import { AgenceService } from './../../../Services/agence.service';
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ServiceResponse } from "../../../DTOS/ServiceResponse";
import { UserRegister } from "../../../DTOS/UserRegister";
import { EmailTaken } from "../../../Validators/email-taken";
import { LoginTaken } from "../../../Validators/login-taken";
import { UserService } from "../../../Services/user.service";
import { ImageConvertService } from "../../../Services/image-convert.service";
import { ToastrService } from "ngx-toastr";
import { AgenceDetails } from '../../../DTOS/AgenceDetails';
import { SuccessToastComponent } from '../../../toast/success-toast/success-toast.component';
import { ErrorToastComponent } from '../../../toast/error-toast/error-toast.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit{
  isDragOver = false
  Agences: AgenceDetails[] = []
  file: File | null = null
  selectedFileName: string | undefined
  onFileSelected($event: Event) {
    const fileInput = ($event as DragEvent).dataTransfer ?
      ($event as DragEvent).dataTransfer :
      ($event.target as HTMLInputElement);
    if (fileInput?.files && fileInput.files.length > 0) {
      this.file = fileInput.files[0];
      this.selectedFileName = this.file.name;
    }
    this.isDragOver = false;

  }

  firstName = new FormControl('', [Validators.required, Validators.minLength(4)]);
  lastName = new FormControl('', [Validators.required, Validators.minLength(4)]);
  email = new FormControl('', [Validators.required, Validators.email], [this.emailTaken.validate]);
  login = new FormControl('', [Validators.required, Validators.minLength(4)], [this.loginTaken.validate]);
  telephone = new FormControl('', [Validators.required, Validators.pattern(/^\d{8}$/)]);
  role = new FormControl('User', [Validators.required]);
  etat = new FormControl(true, [Validators.required]);
  agenceId = new FormControl(1, [Validators.required]);
  registerForm = new FormGroup({
    firstName: this.firstName,
    lastName: this.lastName,
    login: this.login,
    email: this.email,
    telephone: this.telephone,
    role: this.role,
    etat: this.etat,
    agenceId: this.agenceId
  });

  submissionInProgress = false;

  constructor(
    private userService: UserService,
    private emailTaken: EmailTaken,
    private loginTaken: LoginTaken,
    private imageConvertService: ImageConvertService,
    private toastr: ToastrService,
    private agenceService: AgenceService,
    private router: Router,
  ) {}
  ngOnInit(): void {
    this.agenceService.getAllAgences().subscribe({
      next: (response: ServiceResponse<AgenceDetails[]>) => {
        this.Agences = response.data || [];
      },
      error : (error: ServiceResponse<AgenceDetails[]>) => {
        this.toastr.error(error.message, 'Error');
      }
    })
  }
  register() {;
    this.submissionInProgress = true;

    const formData = this.registerForm.value as UserRegister;

    if (this.file) {
      this.imageConvertService.getBase64(this.file)
        .then(base64 => {
          formData.avatarUrl = base64 as string;
          this.AddUser(formData);
        })
        .catch(error => {
          this.toastr.error(error, 'Error');
          this.submissionInProgress = false;
        });
    } else {
      this.AddUser(formData);
    }
  }

  private AddUser(formData: UserRegister): void {
    this.submissionInProgress = true;

    this.userService.addUser(formData).subscribe({
      next: (response: ServiceResponse<number>) => {
        this.toastr.success(response.message, 'Success', {
          toastComponent: SuccessToastComponent
        });
        this.router.navigate(['/admin/user-management']);
      },
      error: (error: ServiceResponse<number>) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      }
    });

    this.submissionInProgress = false;
  }

}
