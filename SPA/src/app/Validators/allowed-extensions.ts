import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AllowedExtensionsValidator implements AsyncValidator {

  validate = (control: AbstractControl): Observable<ValidationErrors | null> => {
    const files: FileList | null = (control.value as HTMLInputElement)?.files;
    if (!files || files.length === 0) {
      return of(null);
    }
    const file = files.item(0);
    if (!file || !(file instanceof File)) {
      return of(null);
    }


    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
    const fileType = file.type.toLowerCase();
    const isValidType = allowedTypes.includes(fileType);
    return of(isValidType ? null : { invalidFileType: true });
  }
}
