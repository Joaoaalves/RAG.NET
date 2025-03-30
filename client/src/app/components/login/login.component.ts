import { Router } from '@angular/router';
import { LoginRequest } from '../../models/login-request';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, InputComponent, CommonModule],
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}
  error?: string;
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
    if (!this.credentials.email) {
      this.error = 'You sould provide email';
      return;
    }

    if (!this.credentials.password) {
      this.error = 'You should provide password';
      return;
    }

    this.error = '';
    this.authService.login(this.credentials).subscribe(
      (result) => {
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        console.log('Error');
        this.error = 'Email and/or password invalid.';
      }
    );
  }
}
