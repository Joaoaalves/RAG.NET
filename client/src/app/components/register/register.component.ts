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
import { PlanType } from 'src/app/models/subscription';
import { SubscriptionService } from 'src/app/services/subscription.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent],
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;
  error = '';
  intent?: PlanType;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private subscriptionService: SubscriptionService
  ) {
    this.route.queryParams.subscribe((params) => {
      this.intent = Number(params['intent']) as PlanType;
    });
  }

  navigateHome() {
    this.router.navigate(['/']);
  }

  isValidIntent(): boolean {
    return (this.intent as number) in PlanType;
  }

  handleSubscription() {
    if (this.isValidIntent()) {
      this.subscriptionService
        .startSubscription(this.intent as PlanType)
        .subscribe({
          next: (url) => (window.location.href = url),
          error: (err) => console.error('Failed to start subscription', err),
        });
    } else {
      this.router.navigate(['/dashboard/workflows']);
    }
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn() && this.intent) this.handleSubscription();

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
          this.handleSubscription();
        } else {
          this.error = 'Registration failed.';
        }
      });
  }
}
