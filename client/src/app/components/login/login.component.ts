import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

// Components
import { InputComponent } from 'src/app/shared/components/input/input.component';

// Services
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent, CommonModule],
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  error?: string;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  navigateHome() {
    this.router.navigate(['/']);
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/dashboard']);
    }

    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  login() {
    if (this.form.invalid) {
      this.error = 'Please fill in all required fields correctly.';
      return;
    }

    this.error = '';
    const credentials = this.form.value;
    this.authService.login(credentials).subscribe(
      () => this.router.navigate(['/dashboard']),
      () => {
        this.error = 'Email and/or password invalid.';
      }
    );
  }
}
