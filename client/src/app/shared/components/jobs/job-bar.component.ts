import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  Renderer2,
} from '@angular/core';
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
export class JobBarComponent implements AfterViewInit, OnDestroy {
  @Input() jobs$!: Observable<JobItem[]>;
  isOpen = false;

  private globalClickListener: (() => void) | undefined;

  constructor(private elRef: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    this.globalClickListener = this.renderer.listen(
      'document',
      'click',
      (event: Event) => {
        if (this.isOpen && !this.elRef.nativeElement.contains(event.target)) {
          this.isOpen = false;
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.globalClickListener) {
      this.globalClickListener();
    }
  }

  toggleJobsBar() {
    this.isOpen = !this.isOpen;
  }

  trackByJobId(_: number, job: JobItem) {
    return job.jobId;
  }
}
