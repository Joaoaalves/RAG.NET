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

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent],
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;
  error = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required],
        passwordConfirmation: ['', Validators.required],
      },
      { validators: this.passwordsMatchValidator }
    );
  }

  passwordsMatchValidator(group: FormGroup) {
    const password = group.get('password')?.value;
    const passwordConfirmation = group.get('passwordConfirmation')?.value;
    return password === passwordConfirmation
      ? null
      : { passwordsMismatch: true };
  }

  register() {
    if (this.form.invalid) {
      if (this.form.errors?.['passwordsMismatch']) {
        this.error = 'Passwords must match.';
      } else {
        this.error = 'You must fill all fields';
      }
      return;
    }

    this.error = '';
    const { firstName, lastName, email, password } = this.form.value;
    this.authService
      .register({ firstName, lastName, email, password })
      .subscribe(
        (success) => {
          if (success) {
            this.router.navigate(['/login']);
          } else {
            this.error = 'Registration failed.';
          }
        },
        () => {
          this.error = 'An error occurred during registration.';
        }
      );
  }
}
