import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ServiceResponse } from '../DTOS/ServiceResponse';
import { EvenementDto } from '../DTOS/EvenementDto ';

@Injectable({
  providedIn: 'root'
})
export class EventConfigurationService {
  private readonly BaseUrl = environment.apiUrl + '/event';
  constructor(private http: HttpClient) { }

  getAllEvent(categoryId: number): Observable<ServiceResponse<EvenementDto[]>> {
    return this.http.get<ServiceResponse<EvenementDto[]>>(`${this.BaseUrl}/${categoryId}`);
  }


  updateEvent(event: EvenementDto): Observable<ServiceResponse<EvenementDto>> {
    return this.http.put<ServiceResponse<EvenementDto>>(`${this.BaseUrl}/UpdateEvent`, event);
  }


}
