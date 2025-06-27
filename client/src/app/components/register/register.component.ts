import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// Components
import { InputComponent } from 'src/app/shared/components/input/input.component';

// Services
import { AuthService } from '../../services/auth.service';
import { Intent } from 'src/app/models/pricing';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent],
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;
  error = '';
  intent?: Intent;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe((params) => {
      this.intent = params['intent'];
    });
  }

  navigateHome() {
    this.router.navigate(['/']);
  }

  isValidIntent(): boolean {
    if (this.intent)
      return Object.values(Intent).includes(this.intent as Intent);

    return false;
  }

  navigateSubscription() {
    if (this.isValidIntent())
      return this.router.navigate(['/dashboard/subscriptions'], {
        queryParams: { intent: this.intent },
      });

    return this.router.navigate(['/dashboard/workflows']);
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn() && this.intent)
      this.navigateSubscription();

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
      .subscribe((success) => {
        if (success) {
          this.navigateSubscription();
        } else {
          this.error = 'Registration failed.';
        }
      });
  }
}
