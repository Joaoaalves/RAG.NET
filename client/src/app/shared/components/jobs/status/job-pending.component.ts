import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { JobStatus } from 'src/app/models/job';

@Component({
  selector: 'app-job-pending',
  templateUrl: './job-pending.component.html',
  imports: [CommonModule],
  standalone: true,
})
export class JobPendingComponent {
  @Input() status!: JobStatus;
  @Input() progress: number = 0;
  Math = Math;
}
