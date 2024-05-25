import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ServiceResponse } from '../DTOS/ServiceResponse';

@Injectable()
export class ResponseErrorHandlerInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<ServiceResponse<any>>> {
        const token = localStorage.getItem('token'); // Replace with your actual token retrieval logic

        // Clone the request and add the token to the headers
        const authReq = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`,
            },
        });

        return next.handle(authReq).pipe(
            catchError((error: HttpErrorResponse) => {
                // Log the error
                console.error('Error occurred while calling the backend API:', error);

                // Create the error instance with a factory function
                const response: ServiceResponse<any> = {
                    data: null,
                    message: error.error.message || 'An unknown error occurred',
                };

                // Throw the error
                return throwError(() => response); // Return the ServiceResponse object
            })
        );
    }
}
