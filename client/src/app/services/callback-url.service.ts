import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddCallbackUrlResponse, CallbackUrl } from '../models/callback-url';

@Injectable({
  providedIn: 'root',
})
export class CallbackUrlService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  private getEndpoint(workflowId: string): string {
    return `${this.apiUrl}/api/workflows/${workflowId}/callback-urls`;
  }

  addCallbackUrl(workflowId: string, url: string): Observable<CallbackUrl> {
    return this.httpClient
      .post<AddCallbackUrlResponse>(this.getEndpoint(workflowId), {
        url,
      })
      .pipe(map((response) => response.url));
  }

  updateCallbackUrl(
    workflowId: string,
    callbackUrl: CallbackUrl
  ): Observable<CallbackUrl> {
    return this.httpClient
      .put<CallbackUrl>(
        `${this.getEndpoint(workflowId)}/${callbackUrl.id}`,
        callbackUrl
      )
      .pipe(map((response) => response));
  }

  deleteCallbackUrl(
    workflowId: string,
    callbackUrlId: string
  ): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.getEndpoint(workflowId)}/${callbackUrlId}`
    );
  }
}
