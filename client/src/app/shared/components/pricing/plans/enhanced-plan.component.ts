import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { PlanType } from 'src/app/models/subscription';

@Component({
  selector: 'app-enhanced-plan',
  templateUrl: './enhanced-plan.component.html',
  imports: [NgIcon],
  standalone: true,
})
export class EnhancedPlanComponent {
  constructor(private router: Router) {}

  navigateRegister() {
    this.router.navigate(['/register'], {
      queryParams: {
        intent: PlanType.ENHANCED.toString(),
      },
    });
  }
}
