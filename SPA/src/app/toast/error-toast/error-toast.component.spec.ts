import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorToastComponent } from './error-toast.component';

describe('ErrorToastComponent', () => {
  let component: ErrorToastComponent;
  let fixture: ComponentFixture<ErrorToastComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ErrorToastComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ErrorToastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
