import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, from } from 'rxjs';
import { tap, concatMap, mapTo, catchError, map } from 'rxjs/operators';

import { EmbeddingResponse } from '../models/embedding';
import { JobItem, JobNotificationResponse, JobStatus } from '../models/job';

import { JobNotificationService } from './job-notification.service';

import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class EmbeddingService {
  private apiUrl = environment.apiUrl;
  private jobs = new BehaviorSubject<JobItem[]>([]);
  readonly jobs$ = this.jobs.asObservable();

  constructor(
    private http: HttpClient,
    private notifier: JobNotificationService
  ) {
    this.initSignalRListeners();
    this.startCleanupTimer();
  }

  sendFile(file: File, apiKey: string): void {
    const formData = new FormData();
    formData.append('file', file);

    this.http
      .post<EmbeddingResponse>(
        `${this.apiUrl}/api/workflows/embedding`,
        formData,
        { headers: { 'x-api-key': apiKey } }
      )
      .pipe(
        tap((response) => {
          const filename = this.getFileName(file);

          const job: JobItem = this.createNewJobItem(response.jobId, filename);

          this.jobs.next([...this.jobs.getValue(), job]);
        }),

        concatMap((response) =>
          from(this.notifier.start()).pipe(map(() => response.jobId))
        ),
        catchError((err) => {
          console.error('Error while connecting to SignalR Hub', err);
          return [];
        })
      )
      .subscribe();
  }

  deleteJob(jobId: string): void {
    const currentJobs = this.jobs.getValue();
    const updatedJobs = currentJobs.filter((job) => job.jobId !== jobId);
    this.jobs.next(updatedJobs);
  }

  private initSignalRListeners() {
    this.notifier.onProgress((response) => {
      this.updateJobStatus(response, 'Processing');
    });
    this.notifier.onCompleted((response) => {
      this.updateJobStatus(response, 'Done');
    });
    this.notifier.onFailed((response, errorMessage) => {
      this.updateJobStatus(response, 'Error', errorMessage);
    });
  }

  private createNewJobItem(jobId: string, title: string): JobItem {
    return {
      jobId: jobId,
      process: {
        title: 'Uploading File',
        progress: 0,
      },
      document: {
        id: '',
        title: title,
        pages: 0,
      },
      status: 'Pending',
      updatedAt: Date.now(),
    };
  }

  private updateJobStatus(
    response: JobNotificationResponse,
    status: JobStatus,
    error?: string
  ) {
    const currentJobs = this.jobs.getValue();
    const jobIndex = currentJobs.findIndex((j) => j.jobId === response.jobId);

    let updatedJobs: JobItem[];

    if (jobIndex > -1) {
      updatedJobs = currentJobs.map((job) =>
        job.jobId === response.jobId
          ? {
              ...job,
              document: response.document,
              process: response.process,
              status,
              error,
              updatedAt: Date.now(),
            }
          : job
      );
    } else {
      const newJob: JobItem = {
        jobId: response.jobId,
        process: response.process,
        document: response.document,
        status,
        error,
        updatedAt: Date.now(),
      };
      updatedJobs = [...currentJobs, newJob];
    }

    this.jobs.next(updatedJobs);
  }

  private getFileName(file: File): string {
    return file.name.replace(/\.[^/.]+$/, '');
  }

  private startCleanupTimer() {
    setInterval(() => {
      const now = Date.now();
      const oneMinute = 60 * 1000;

      const currentJobs = this.jobs.getValue();
      const filtered = currentJobs.filter((job) => {
        if (job.status === 'Done' || job.status === 'Error') {
          return now - job.updatedAt < oneMinute;
        }
        return true;
      });

      if (filtered.length !== currentJobs.length) {
        this.jobs.next(filtered);
      }
    }, 5000);
  }
}
