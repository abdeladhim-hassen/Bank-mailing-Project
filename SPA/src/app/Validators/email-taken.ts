import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService } from '../Services/user.service';

@Injectable({
  providedIn: 'root'
})
export class EmailTaken implements AsyncValidator {
  constructor(private userService: UserService) {}

  validate = (control: AbstractControl): Observable<ValidationErrors | null> => {
    const email = control.value;

    return this.userService.checkEmailExists(email).pipe(
      map(exists => (exists ? { EmailTaken: true } : null)),
    );
  }
}
