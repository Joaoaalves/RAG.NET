import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { JobItem } from 'src/app/models/job';
import { JobComponent } from './job.component';

@Component({
  selector: 'app-job-bar',
  standalone: true,
  imports: [CommonModule, JobComponent],
  templateUrl: './job-bar.component.html',
})
export class JobBarComponent {
  @Input() jobs$!: Observable<JobItem[]>;
  isOpen = false;

  toggleJobsBar() {
    this.isOpen = !this.isOpen;
  }

  trackByJobId(_: number, job: JobItem) {
    return job.jobId;
  }
}
