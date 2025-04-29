import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

// Models
import { JobItem } from 'src/app/models/job';

// Components
import { JobComponent } from './job.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-job-bar',
  templateUrl: './job-bar.component.html',
  imports: [CommonModule, JobComponent],
  standalone: true,
})
export class JobBarComponent {
  @Input() jobs!: Observable<JobItem[]>;
}
