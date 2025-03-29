import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  constructor(private authService: AuthService, private router: Router) {}

  email = '';
  password = '';
  passwordConfirmation = '';
  error = '';

  register() {
    if (this.password !== this.passwordConfirmation) {
      this.error = 'Passwords must match.';
      return;
    }

    this.authService
      .register({ email: this.email, password: this.password })
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
