import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideBell, lucideSearch, lucidePlus } from '@ng-icons/lucide';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-workflows-nav-bar',
  templateUrl: './workflow-nav-bar.component.html',
  imports: [NgIcon, CommonModule],
  providers: [provideIcons({ lucideBell, lucideSearch, lucidePlus })],
  standalone: true,
})
export class WorkflowNavBarComponent implements OnInit {
  user$: Observable<User | null>;

  constructor(private router: Router, private userService: UserService) {
    this.user$ = this.userService.user$;
  }

  ngOnInit(): void {
    this.userService.getInfo().subscribe();
  }

  navigateNewWorkflow() {
    this.router.navigate(['/dashboard/workflows/new']);
  }
}
