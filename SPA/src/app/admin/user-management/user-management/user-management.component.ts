import { Component } from '@angular/core';
import { UserDetails } from '../../../DTOS/UserDetails';
import { UserService } from '../../../Services/user.service';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';
import { UserUpdate } from '../../../DTOS/UserUpdate';
import { EditUserComponent } from '../edit-user/edit-user.component';
import { ModalService } from '../../../Services/modal.service';
import { Observable } from 'rxjs';
import { ErrorToastComponent } from '../../../toast/error-toast/error-toast.component';
import { ToastrService } from 'ngx-toastr';
import { SuccessToastComponent } from '../../../toast/success-toast/success-toast.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent {
  UserDetails: UserDetails[] = [];
  constructor(private  userService: UserService,
    private modalService: ModalService,
    private toastr: ToastrService){}
  ngOnInit() {
    this.userService.getAllUsers().subscribe({
      next: (response: ServiceResponse<UserDetails[]>) => {
        this.UserDetails = response.data || [];
      },
      error: (error) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      }
    });
  }
  onModalSubmit(response: ServiceResponse<UserDetails>) {
    if (response.data) {
      this.UserDetails = this.UserDetails.map(user => {
          if (user.id === response.data!.id) {
              return response.data!;
          } else {
              return user;
          }
      }).filter(user => user !== undefined) as UserDetails[];
      this.toastr.success(response.message, 'Success', {
        toastComponent: SuccessToastComponent
      });
  }
  };


  openEditModal(model: UserDetails) {
    this.modalService.openModal(EditUserComponent, { model: model });
  }
}
