import { Component } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
import {
  lucideCheck,
  lucideCrown,
  lucideDatabase,
  lucideRocket,
  lucideX,
  lucideZap,
} from '@ng-icons/lucide';
import { PlansComponent } from './plans.component';
import { CommonModule } from '@angular/common';
import { ComparisonsComponent } from './comparisons.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pricing',
  templateUrl: './pricing.component.html',
  imports: [PlansComponent, ComparisonsComponent, CommonModule],
  providers: [
    provideIcons({
      lucideCheck,
      lucideZap,
      lucideCrown,
      lucideRocket,
      lucideX,
      lucideDatabase,
    }),
  ],
  standalone: true,
})
export class PricingComponent {
  constructor(private router: Router) {}

  navigateRegister() {
    this.router.navigate(['/login']);
  }
}
