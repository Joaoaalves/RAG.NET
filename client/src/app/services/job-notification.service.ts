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
  private _onProgress?: (r: JobNotificationResponse) => void;
  private _onCompleted?: (r: JobNotificationResponse) => void;
  private _onFailed?: (r: JobNotificationResponse, e: string) => void;

  constructor() {
    this.hub = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/jobstatus`, {
        accessTokenFactory: () => localStorage.getItem('accessToken')!,
      })
      .configureLogging(LogLevel.None)
      .withAutomaticReconnect()
      .build();

    this.hub.on('JobProgress', (r: JobNotificationResponse) =>
      this._onProgress?.(r)
    );
    this.hub.on('JobCompleted', (r: JobNotificationResponse) =>
      this._onCompleted?.(r)
    );
    this.hub.on('JobFailed', (r: JobNotificationResponse, e: string) =>
      this._onFailed?.(r, e)
    );

    this.hub.onclose(() =>
      this.start().catch((err) =>
        console.error('Error while starting SignalR:', err)
      )
    );

    this.start().catch((err) =>
      console.error('Error while starting SignalR:', err)
    );
  }

  get isConnected(): boolean {
    return this.hub.state === HubConnectionState.Connected;
  }

  async start(): Promise<void> {
    if (
      this.hub.state !== HubConnectionState.Connected &&
      this.hub.state !== HubConnectionState.Connecting
    ) {
      await this.hub.start();
    }
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
