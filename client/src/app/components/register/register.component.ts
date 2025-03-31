import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { InputComponent } from 'src/app/shared/components/input/input.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, InputComponent],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  constructor(private authService: AuthService, private router: Router) {}

  email = '';
  firstName = '';
  lastName = '';
  password = '';
  passwordConfirmation = '';
  error = '';

  register() {
    if (this.password !== this.passwordConfirmation) {
      this.error = 'Passwords must match.';
      return;
    }

    if (!this.email || !this.password || !this.firstName || !this.lastName) {
      this.error = 'You must fill all fields';
      return;
    }

    this.authService
      .register({
        email: this.email,
        password: this.password,
        firstName: this.firstName,
        lastName: this.lastName,
      })
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
