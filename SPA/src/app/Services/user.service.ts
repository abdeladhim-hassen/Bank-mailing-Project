import { CurrentUser } from './../DTOS/CurrentUser';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { UserRegister } from '../DTOS/UserRegister';
import { ServiceResponse } from '../DTOS/ServiceResponse';
import { Observable, of } from 'rxjs';
import { ResetPassword } from '../DTOS/ResetPassword ';
import { UserChangePassword } from '../DTOS/UserChangePassword';
import { UserDetails } from '../DTOS/UserDetails';
import { UserUpdate } from '../DTOS/UserUpdate';
import { jwtDecode } from 'jwt-decode';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly BaseUrl = environment.apiUrl + '/user';

  constructor(private http: HttpClient) { }

  addUser(request: UserRegister): Observable<ServiceResponse<number>> {
    const url = `${this.BaseUrl}/add`;
    return this.http.post<ServiceResponse<number>>(url,request);
  }

  getCurrentUser(): Observable<CurrentUser> {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('Token not found');
    }
    const decoded: any = jwtDecode(token);
    const currentUser: CurrentUser = {
      avatarUrl: decoded['AvatarUrl'],
      firstName: decoded['FirstName'],
      lastName: decoded['LastName'],
      email: decoded['Email'],
      userName: decoded['UserName'],
    };
    return of(currentUser);
  }

  editUser(request: UserUpdate): Observable<ServiceResponse<UserDetails>> {
    const url = `${this.BaseUrl}`;
    return this.http.put<ServiceResponse<UserDetails>>(url, request);
  }


  getAllUsers(): Observable<ServiceResponse<UserDetails[]>> {
    return this.http.get<ServiceResponse<UserDetails[]>>(`${this.BaseUrl}`)
  }
  changePassword(userChangePassword: UserChangePassword): Observable<ServiceResponse<boolean>> {
    return this.http.post<ServiceResponse<boolean>>(`${this.BaseUrl}/change-password`, userChangePassword);
  }

  resetPasswordWithVerificationCode(resetPassword: ResetPassword): Observable<ServiceResponse<boolean>> {
    return this.http.put<ServiceResponse<boolean>>(`${this.BaseUrl}/reset-password`, resetPassword);
  }

  sendVerificationCode(email: string): Observable<ServiceResponse<boolean>> {
    const url = `${this.BaseUrl}/send-verification-code?email=${email}`;
    return this.http.post<ServiceResponse<boolean>>(url, null);
  }

  checkEmailExists(email: string): Observable<boolean> {
    const url = `${this.BaseUrl}/check-email-exists/${email}`
    return this.http.get<boolean>(url);
  }

  checkLoginExists(Login: string): Observable<boolean> {
    const url = `${this.BaseUrl}/check-login-exists/${Login}`
    return this.http.get<boolean>(url);
  }
}
