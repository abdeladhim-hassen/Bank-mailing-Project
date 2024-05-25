import { Component, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { ModalService } from '../../../Services/modal.service';
import { Subscription } from 'rxjs';
import { UserUpdate } from '../../../DTOS/UserUpdate';
import { ImageConvertService } from '../../../Services/image-convert.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserDetails } from '../../../DTOS/UserDetails';
import { UserService } from '../../../Services/user.service';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';
import { ErrorToastComponent } from '../../../toast/error-toast/error-toast.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit, OnDestroy {
  @Output() submit = new EventEmitter<ServiceResponse<UserDetails>>();
  model: UserDetails | null = null;
  visible = false;
  isDragOver = false
  private modalStateSubscription: Subscription | undefined;

  file: File | null = null;
  selectedFileName: string | undefined;

  firstName = new FormControl('', [Validators.required, Validators.minLength(4)]);
  lastName = new FormControl('', [Validators.required, Validators.minLength(4)]);
  telephone = new FormControl('', [Validators.required, Validators.pattern(/^\d{8}$/)]);
  role = new FormControl('User', [Validators.required, Validators.pattern(/^(Admin|User)$/)]);
  etat = new FormControl<boolean | string>(true, [Validators.required]);

  registerForm = new FormGroup({
    firstName: this.firstName,
    lastName: this.lastName,
    telephone: this.telephone,
    role: this.role,
    etat: this.etat,
  });
  submissionInProgress = false;

  constructor(private modalService: ModalService,
              private imageConvertService: ImageConvertService,
              private userService: UserService,
              private toastr: ToastrService) {}

  ngOnInit(): void {
    this.reset();
  }

  ngOnDestroy(): void {
    if (this.modalStateSubscription) {
      this.modalStateSubscription.unsubscribe();
    }
  }

  onSubmit() {
    const formData = this.registerForm.value as UserUpdate;
    this.submissionInProgress = true;

    if (this.file) {
      this.imageConvertService.getBase64(this.file)
        .then(base64 => {
          formData.avatarUrl = base64 as string;
          this.emitSubmit(formData);
          this.submissionInProgress = false;
        })
        .catch(error => {
          this.submissionInProgress = false;
          this.toastr.error(error.message, 'Error', {
            toastComponent: ErrorToastComponent
          });
        });
    } else {
      this.emitSubmit(formData);
      this.submissionInProgress = false;
    }
  }

  closeModal() {
    this.modalService.closeModal();
    this.visible = false;
  }

  private emitSubmit(formData: UserUpdate) {
    if (this.model) {
      formData.id = this.model.id;

      // Convert etat to boolean if it's a string
      if (typeof formData.etat === 'string') {
        formData.etat = formData.etat === 'true';
      }

      this.EditUser(formData);
      this.modalService.closeModal();
      this.visible = false;
      this.model = null;
      this.file = null;
      this.selectedFileName = undefined;
    }
  }

  reset() {
    this.modalStateSubscription = this.modalService.getModalState().subscribe((modalData: any) => {
      if (modalData && modalData.data?.model) {
        this.model = modalData.data.model;
        this.visible = true;

        // Patch form values with model data
        this.firstName.setValue(this.model?.firstName || '');
        this.lastName.setValue(this.model?.lastName || '');
        this.telephone.setValue(this.model?.telephone || '');
        this.role.setValue(this.model?.role || 'User');
        this.etat.setValue(this.model?.etat !== undefined ? this.model.etat : true);
      }
    });
  }


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

  private EditUser(user: UserUpdate){
    console.log(user.etat);
    console.log(typeof(user.etat));
    return this.userService.editUser(user).subscribe({
      next: (response: ServiceResponse<UserDetails>) => {
        this.submit.emit(response);
      },
      error: (error) => {
        this.submit.emit(error);
      }
    });
  }
}
