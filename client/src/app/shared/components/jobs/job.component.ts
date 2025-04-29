import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { JobItem } from 'src/app/models/job';

// Components
import { JobPendingComponent } from 'src/app/shared/components/jobs/status/job-pending.component';
import { JobDoneComponent } from 'src/app/shared/components/jobs/status/job-done.component';
import { JobErrorComponent } from 'src/app/shared/components/jobs/status/job-error.component';

@Component({
  selector: 'app-job',
  imports: [
    CommonModule,
    JobPendingComponent,
    JobDoneComponent,
    JobErrorComponent,
  ],
  templateUrl: './job.component.html',
  standalone: true,
})
export class JobComponent {
  @Input() job!: JobItem;
}
