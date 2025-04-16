import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ContentItem, QueryRequest, QueryResponse } from '../models/query';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root',
})
export class QueryService extends BaseApiService {
  chunks: ContentItem[] = [];
  filteredContent: string[] = [];

  constructor(http: HttpClient) {
    super(http);
  }

  getQueryEndpoint(): string {
    return this.buildUrl('/api/query');
  }

  query(data: QueryRequest, apiKey: string): Observable<QueryResponse> {
    this.chunks = [];
    this.filteredContent = [];

    return this.http
      .post<QueryResponse>(this.getQueryEndpoint(), data, {
        headers: {
          'x-api-key': apiKey,
        },
        responseType: 'json',
      })
      .pipe(
        map((response) => {
          this.chunks = response.chunks;
          this.filteredContent = response.filteredContent;
          return response;
        })
      );
  }
}
