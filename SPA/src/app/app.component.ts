
import { Component, OnInit } from '@angular/core';
import { AuthService } from './Services/auth.service';
import { initFlowbite } from 'flowbite';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{


  constructor(public auth: AuthService) {}
  ngOnInit(): void {
    initFlowbite()
  }
}
