import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  QueryEnhancer,
  QueryEnhancerEnableResponse,
  QueryEnhancerUpdateResponse,
} from '../models/query-enhancer';

@Injectable({
  providedIn: 'root',
})
export class QueryEnhancerService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  private getEndpoint(workflowId: string, type: string): string {
    return `${this.apiUrl}/api/workflows/${workflowId}/query-enhancer/${type}`;
  }

  enableQueryEnhancer(
    qe: QueryEnhancer,
    workflowId: string,
    type: string
  ): Observable<QueryEnhancer> {
    return this.httpClient
      .post<QueryEnhancerEnableResponse>(this.getEndpoint(workflowId, type), qe)
      .pipe(map((response) => response.queryEnhancer));
  }

  updateQueryEnhancer(
    qe: QueryEnhancer,
    workflowId: string,
    type: string
  ): Observable<QueryEnhancer> {
    return this.httpClient
      .put<QueryEnhancerUpdateResponse>(this.getEndpoint(workflowId, type), {
        maxQueries: qe.maxQueries,
        guidance: qe.guidance ?? undefined,
        isEnabled: qe.isEnabled,
      })
      .pipe(map((response) => response.queryEnhancer));
  }

  deleteQueryEnhancer(workflowId: string, type: string): Observable<boolean> {
    return this.httpClient.delete(this.getEndpoint(workflowId, type)).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }

  toggleQueryEnhancer(
    qe: QueryEnhancer,
    workflowId: string,
    type: string,
    newStatus: boolean
  ): Observable<QueryEnhancer> {
    const updatedQE = { ...qe, isEnabled: newStatus };
    return this.updateQueryEnhancer(updatedQE, workflowId, type);
  }
}
