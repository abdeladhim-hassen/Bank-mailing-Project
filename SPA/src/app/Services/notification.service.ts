import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ServiceResponse } from '../DTOS/ServiceResponse';
import { Observable } from 'rxjs';
import { AddNotification } from '../DTOS/AddNotification';
import { UpdateNotificationDto } from '../DTOS/UpdateNotificationDto';
import { NotificationDto } from '../DTOS/NotificationDto';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private BaseUrl = `${environment.apiUrl}/notification`;

  constructor(private http: HttpClient) {}

  createNotification(notification: AddNotification): Observable<ServiceResponse<number>> {
    return this.http.post<ServiceResponse<number>>(this.BaseUrl, notification);
  }

  updateNotification(notification: UpdateNotificationDto): Observable<ServiceResponse<NotificationDto>> {
    return this.http.put<ServiceResponse<NotificationDto>>(`${this.BaseUrl}/update`, notification);
  }

  getAllNotifications(): Observable<ServiceResponse<NotificationDto[]>> {
    return this.http.get<ServiceResponse<NotificationDto[]>>(this.BaseUrl);
  }

}
