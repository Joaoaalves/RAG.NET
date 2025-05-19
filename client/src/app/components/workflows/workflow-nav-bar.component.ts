import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideBell, lucideSearch, lucidePlus } from '@ng-icons/lucide';
@Component({
  selector: 'app-workflows-nav-bar',
  templateUrl: './workflow-nav-bar.component.html',
  imports: [NgIcon],
  providers: [provideIcons({ lucideBell, lucideSearch, lucidePlus })],
  standalone: true,
})
export class WorkflowNavBarComponent {
  constructor(private router: Router) {}

  navigateNewWorkflow() {
    this.router.navigate(['/dashboard/workflows/new']);
  }
}
