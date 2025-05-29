import { EmbeddingService } from 'src/app/services/embedding.service';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { JobItem } from 'src/app/models/job';
import { JobComponent } from './job.component';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronDown, lucideX } from '@ng-icons/lucide';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-job-bar',
  standalone: true,
  imports: [CommonModule, JobComponent, NgIcon],
  providers: [provideIcons({ lucideChevronDown, lucideX })],
  templateUrl: './job-bar.component.html',
})
export class JobBarComponent implements OnInit, OnDestroy {
  @Input() jobs$!: Observable<JobItem[]>;
  isOpen = false;

  private previousJobsLength = 0;
  private subscription!: Subscription;

  constructor(private embeddingService: EmbeddingService) {}

  ngOnInit() {
    this.subscription = this.jobs$.subscribe((jobs) => {
      if (jobs.length === 0) {
        this.isOpen = false;
      }
      if (jobs.length > this.previousJobsLength) {
        this.isOpen = true;
      }
      this.previousJobsLength = jobs.length;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  toggleJobsBar() {
    this.isOpen = !this.isOpen;
  }

  trackByJobId(_: number, job: JobItem) {
    return job.jobId;
  }

  deleteJob(jobId: string) {
    this.embeddingService.deleteJob(jobId);
  }
}
