import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, from } from 'rxjs';
import { tap, concatMap, mapTo, catchError } from 'rxjs/operators';

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

  private updateJobStatus(
    response: JobNotificationResponse,
    status: JobStatus,
    error?: string
  ) {
    const updated = this.jobs.getValue().map((job) =>
      job.jobId === response.jobId
        ? {
            ...job,
            document: response.document,
            process: response.process,
            status,
            error,
          }
        : job
    );
    this.jobs.next(updated);
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
          const title = file.name.replace(/\.[^/.]+$/, '');

          const job: JobItem = {
            jobId: response.jobId,
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
          };
          this.jobs.next([...this.jobs.getValue(), job]);
        }),

        concatMap((response) =>
          from(this.notifier.start()).pipe(mapTo(response.jobId))
        ),

        concatMap((jobId) => from(this.notifier.joinJobGroup(jobId))),

        catchError((err) => {
          console.error('Error while connecting to SignalR Hub', err);
          return [];
        })
      )
      .subscribe();
  }
}
