import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { QueryRequest, QueryResponse } from '../models/query';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root',
})
export class QueryService extends BaseApiService {
  constructor(http: HttpClient) {
    super(http);
  }

  private getQueryEndpoint() {
    return this.buildUrl('/api/query');
  }

  query(data: QueryRequest, apiKey: string): Observable<QueryResponse> {
    return this.http.post<QueryResponse>(this.getQueryEndpoint(), data, {
      headers: {
        'x-api-key': apiKey,
      },
      responseType: 'json',
    });
  }
}
