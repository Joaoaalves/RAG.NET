import { Component } from '@angular/core';
import { AscendPlanComponent } from './plans/ascend-plan.component';
import { CorePlanComponent } from './plans/core-plan.component';
import { EnhancedPlanComponent } from './plans/enhanced-plan.component';

@Component({
  selector: 'app-plans',
  templateUrl: './plans.component.html',
  imports: [AscendPlanComponent, CorePlanComponent, EnhancedPlanComponent],
  standalone: true,
})
export class PlansComponent {}
