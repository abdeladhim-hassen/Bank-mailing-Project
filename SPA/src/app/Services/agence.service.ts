import { Injectable } from '@angular/core';
import { AgenceDetails } from '../DTOS/AgenceDetails';
import { ServiceResponse } from '../DTOS/ServiceResponse';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AgenceService {
  private readonly BaseUrl = environment.apiUrl + '/agence';
  constructor(private http: HttpClient) {}

  getAllAgences(): Observable<ServiceResponse<AgenceDetails[]>> {
    return this.http.get<ServiceResponse<AgenceDetails[]>>(this.BaseUrl);
}
}
