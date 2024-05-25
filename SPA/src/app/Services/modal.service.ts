import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalSubject = new BehaviorSubject<any>(null);

  constructor() {}

  openModal(component: any, data: any = null) {
    this.modalSubject.next({ component, data });
  }

  closeModal() {
    this.modalSubject.next(null);
  }

  getModalState(): Observable<any> {
    return this.modalSubject.asObservable();
  }
}
