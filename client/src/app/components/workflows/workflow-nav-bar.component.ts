import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideBell, lucideSearch, lucidePlus } from '@ng-icons/lucide';

import { debounceTime, distinctUntilChanged, Observable, Subject } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-workflows-nav-bar',
  templateUrl: './workflow-nav-bar.component.html',
  imports: [NgIcon, CommonModule, FormsModule],
  providers: [provideIcons({ lucideBell, lucideSearch, lucidePlus })],
  standalone: true,
})
export class WorkflowNavBarComponent implements OnInit {
  @Output() queryEvent = new EventEmitter<string>();
  user$: Observable<User | null>;
  query: string = '';

  private searchSubject = new Subject<string>();

  constructor(private router: Router, private userService: UserService) {
    this.user$ = this.userService.user$;
  }

  ngOnInit(): void {
    this.userService.getInfo().subscribe();

    this.searchSubject
      .pipe(debounceTime(100), distinctUntilChanged())
      .subscribe((value: string) => {
        this.query = value;
        this.queryEvent.emit(value);
      });
  }

  onChange(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchSubject.next(input.value);
  }

  navigateNewWorkflow() {
    this.router.navigate(['/dashboard/workflows/new']);
  }
}
