import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService } from '../Services/user.service';


@Injectable({
  providedIn: 'root'
})
export class LoginTaken implements AsyncValidator {
  constructor(private userService: UserService) {}

  validate = (control: AbstractControl): Observable<ValidationErrors | null> => {
    const login = control.value;

    return this.userService.checkLoginExists(login).pipe(
      map(exists => (exists ? { LoginTaken: true } : null)),
    );
  }
}
