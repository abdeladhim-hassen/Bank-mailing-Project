import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageConvertService {

  constructor() { }
  public getBase64(file: File): Promise<string | ArrayBuffer> {
    return new Promise<string | ArrayBuffer>((resolve, reject) => {
      if (!file) {
        reject('File is null');
      } else {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result as string);
        reader.onerror = error => reject(error);
      }
    });
  }
}
