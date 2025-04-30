import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { JobNotificationResponse } from '../models/job';

@Injectable({ providedIn: 'root' })
export class JobNotificationService {
  private hub: HubConnection;
  private joinedGroups = new Set<string>();

  constructor() {
    this.hub = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/jobstatus`)
      .configureLogging(LogLevel.None)
      .withAutomaticReconnect()
      .build();

    this.hub.onreconnected(() => {
      this.joinedGroups.forEach((jobId) => {
        this.hub.invoke('JoinJobGroup', jobId).catch(console.error);
      });
    });

    this.hub.on('JobProgress', (r: JobNotificationResponse) =>
      this._onProgress?.(r)
    );
    this.hub.on('JobCompleted', (r: JobNotificationResponse) =>
      this._onCompleted?.(r)
    );
    this.hub.on('JobFailed', (r: JobNotificationResponse, e: string) =>
      this._onFailed?.(r, e)
    );

    this.start().catch((err) => console.error('SignalR start error', err));
  }

  private _onProgress?: (r: JobNotificationResponse) => void;
  private _onCompleted?: (r: JobNotificationResponse) => void;
  private _onFailed?: (r: JobNotificationResponse, e: string) => void;

  get isConnected() {
    return this.hub.state === HubConnectionState.Connected;
  }

  async start(): Promise<void> {
    if (!this.isConnected) {
      await this.hub.start();
    }
  }

  async joinJobGroup(jobId: string): Promise<void> {
    await this.start();
    await this.hub.invoke('JoinJobGroup', jobId);
    this.joinedGroups.add(jobId);
  }

  onProgress(callback: (r: JobNotificationResponse) => void): void {
    this._onProgress = callback;
  }
  onCompleted(callback: (r: JobNotificationResponse) => void): void {
    this._onCompleted = callback;
  }
  onFailed(callback: (r: JobNotificationResponse, err: string) => void): void {
    this._onFailed = callback;
  }
}
