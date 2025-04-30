import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, signal } from '@angular/core';

// Models
import { JobItem } from 'src/app/models/job';

// Components
import { JobComponent } from './job.component';
import { map, Observable } from 'rxjs';

@Component({
  selector: 'app-job-bar',
  templateUrl: './job-bar.component.html',
  imports: [CommonModule, JobComponent],
  standalone: true,
})
export class JobBarComponent implements OnInit {
  @Input() jobs!: Observable<JobItem[]>;
  counter$!: Observable<number>;
  isOpen: boolean = false;

  ngOnInit(): void {
    this.counter$ = this.jobs.pipe(map((jobs) => jobs.length));
  }

  toggleJobsBar() {
    this.isOpen = !this.isOpen;
  }
}
