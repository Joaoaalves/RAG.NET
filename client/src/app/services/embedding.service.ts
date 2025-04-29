import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmbeddingResponse } from '../models/embedding';
import { JobNotificationService } from './job-notification.service';
import { environment } from '../../environments/environment';
import { BehaviorSubject, from } from 'rxjs';
import { tap, concatMap, mapTo, catchError } from 'rxjs/operators';
import { JobItem, JobNotificationResponse } from '../models/job';

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
    status: JobItem['status'],
    error?: string
  ) {
    const updated = this.jobs
      .getValue()
      .map((job) =>
        job.jobId === response.jobId ? { ...job, status, error } : job
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
          const job: JobItem = {
            jobId: response.jobId,
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
