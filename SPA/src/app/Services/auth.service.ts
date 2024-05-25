import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { UserLogin } from "../DTOS/UserLogin";
import { ServiceResponse } from "../DTOS/ServiceResponse";
import { Router } from "@angular/router";
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly BaseUrl = environment.apiUrl + '/auth';

  constructor(private http: HttpClient, private router: Router) {}

  isUserAuthenticated(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const expirationTime = decoded.exp;
        const currentTime = Date.now() / 1000; // Convert milliseconds to seconds
        return of(expirationTime > currentTime); // Token is considered valid if expiration time is in the future
      } catch (error) {
        console.error('Error decoding token:', error);
        return of(false);
      }
    } else {
      return of(false);
    }
  }

  isAdmin(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        return of(role === 'Admin');
      } catch (error) {
        console.error('Error decoding token:', error);
        return of(false);
      }
    } else {
      return of(false);
    }
  }

  login(request: UserLogin): Observable<ServiceResponse<string>> {
    const url = `${this.BaseUrl}/login`
    return this.http.post<ServiceResponse<string>>
      (url, request).pipe(
      map((response: ServiceResponse<string>) => {
        if (!response.data) {
          throw new Error('Wrong credentials');
        }
        localStorage.setItem('token', response.data);
        return response;
      })
    );
  }



  logout($event?: Event)
  {
    if ($event) {
      $event.preventDefault();
    }
    localStorage.removeItem("token");
   this.router.navigateByUrl('auth')
  }
}
