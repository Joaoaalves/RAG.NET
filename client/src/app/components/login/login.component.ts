import { Router } from '@angular/router';
import { LoginRequest } from '../../models/login-request';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}

  credentials: LoginRequest = {
    email: '',
    password: '',
  };

  ngOnInit(): void {
    if (this.isLoggedIn()) {
      this.router.navigate(['/dashboard']);
    }
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  login() {
    this.authService.login(this.credentials).subscribe(() => {
      this.router.navigate(['/dashboard']);
    });
  }
}
