import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { Intent } from 'src/app/models/pricing';

@Component({
  selector: 'app-ascend-plan',
  templateUrl: './ascend-plan.component.html',
  imports: [NgIcon],
  standalone: true,
})
export class AscendPlanComponent {
  constructor(private router: Router) {}

  navigateRegister() {
    this.router.navigate(['/register'], {
      queryParams: {
        intent: Intent.ASCEND,
      },
    });
  }
}
