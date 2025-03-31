import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { CommonModule } from '@angular/common';

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
        console.log('Error');
        this.error = 'Email and/or password invalid.';
      }
    );
  }
}
