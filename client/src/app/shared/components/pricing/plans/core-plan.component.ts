import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { PlanType } from 'src/app/models/subscription';

@Component({
  selector: 'app-core-plan',
  templateUrl: './core-plan.component.html',
  imports: [NgIcon],
  standalone: true,
})
export class CorePlanComponent {
  constructor(private router: Router) {}

  navigateRegister() {
    this.router.navigate(['/register'], {
      queryParams: {
        intent: PlanType.CORE.toString(),
      },
    });
  }
}
