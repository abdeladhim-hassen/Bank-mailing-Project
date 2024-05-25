import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ServiceResponse } from '../DTOS/ServiceResponse';
import { TemplateType } from '../DTOS/TemplateType ';
import { TemplateDto } from '../DTOS/TemplateDto ';


@Injectable({
  providedIn: 'root'
})
export class TemplateService {
  private baseUrl = environment.apiUrl + '/template';

  constructor(private http: HttpClient) {}

  getAllTemplates(categoryId: number, templateType: TemplateType): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/templates/${categoryId}?templateType=${templateType}`);
  }

  updateTemplate(model: TemplateDto): Observable<ServiceResponse<number>> {
    return this.http.put<ServiceResponse<number>>(`${this.baseUrl}/UpdateTemplate/`, model);
  }
}
