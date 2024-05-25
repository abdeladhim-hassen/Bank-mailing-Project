import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { UserService } from '../../Services/user.service';
import { CurrentUser } from '../../DTOS/CurrentUser';
import { ToastrService } from 'ngx-toastr';
import { ErrorToastComponent } from '../../toast/error-toast/error-toast.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  constructor(public auth: AuthService, private userService: UserService,
    private toastr: ToastrService
  ) {}

  currentUser: CurrentUser | null = null;
  isDarkMode: boolean = false;

  ngOnInit(): void {
    this.loadCurrentUser();
    this.initializeTheme();
  }

  loadCurrentUser(): void {
    this.userService.getCurrentUser().subscribe({
      next: (user: CurrentUser) => {
        this.currentUser = user;
      },
      error: (error: any) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      }
    });
  }

  initializeTheme(): void {
    const storedMode = localStorage.getItem('color-theme');
    if (storedMode === 'dark') {
      this.isDarkMode = true;
    } else if (storedMode === 'light') {
      this.isDarkMode = false;
    } else {
      this.isDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
    this.applyTheme();
  }

  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    this.applyTheme();
    localStorage.setItem('color-theme', this.isDarkMode ? 'dark' : 'light');
  }

  applyTheme(): void {
    if (this.isDarkMode) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

}
