import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { JobNotificationResponse } from '../models/job';

@Injectable({ providedIn: 'root' })
export class JobNotificationService {
  private hub!: HubConnection;

  constructor() {
    this.hub = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/jobstatus`, {
        accessTokenFactory: () => localStorage.getItem('accessToken')!,
      })
      .configureLogging(LogLevel.None)
      .withAutomaticReconnect()
      .build();
  }

  async start(): Promise<void> {
    if (this.hub.state !== 'Connected') {
      await this.hub.start();
    }
  }

  joinJobGroup(jobId: string): Promise<void> {
    return this.hub.invoke('JoinJobGroup', jobId);
  }

  onProgress(callback: (response: JobNotificationResponse) => void): void {
    this.hub.on('JobProgress', callback);
  }

  onCompleted(callback: (response: JobNotificationResponse) => void): void {
    this.hub.on('JobCompleted', callback);
  }

  onFailed(
    callback: (response: JobNotificationResponse, errorMessage: string) => void
  ): void {
    this.hub.on('JobFailed', callback);
  }

  offAll(): void {
    this.hub.off('JobProgress');
    this.hub.off('JobCompleted');
    this.hub.off('JobFailed');
  }
}
