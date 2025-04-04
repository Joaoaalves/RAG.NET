import { Injectable } from '@angular/core';
import {
  QueryEnhancerEnableResponse,
  QueryEnhancer,
} from '../models/query-enhancer';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class QueryEnhancerService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  enableAutoQuery(
    autoQuery: QueryEnhancer,
    workflowId: string
  ): Observable<string> {
    return this.httpClient
      .post<QueryEnhancerEnableResponse>(
        `${this.apiUrl}/api/workflows/${workflowId}/query-enhancer/auto-query`,
        autoQuery
      )
      .pipe(map((response) => response.queryEnhancerId));
  }

  enableHyde(hyde: QueryEnhancer, workflowId: string): Observable<string> {
    return this.httpClient
      .post<QueryEnhancerEnableResponse>(
        `${this.apiUrl}/api/workflows/${workflowId}/query-enhancer/auto-query`,
        hyde
      )
      .pipe(map((response) => response.queryEnhancerId));
  }
}
