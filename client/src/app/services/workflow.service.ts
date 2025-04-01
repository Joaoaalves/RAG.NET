import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';

import {
  Workflow,
  WorkflowsInfoResponse,
  CreateWorkflowRequest,
  CreateWorkflowResponse,
} from '../models/workflow';

import { EmbeddingModelsResponse } from '../models/embedding';

@Injectable({
  providedIn: 'root',
})
export class WorkflowService {
  private apiUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) {}

  createWorkflow(
    workflowDetails: Partial<CreateWorkflowRequest>
  ): Observable<string> {
    return this.httpClient
      .post<CreateWorkflowResponse>(
        `${this.apiUrl}/api/workflows`,
        workflowDetails
      )
      .pipe(map((response) => response.workflowId));
  }

  getWorkflows(): Observable<Workflow[]> {
    return this.httpClient
      .get<WorkflowsInfoResponse>(`${this.apiUrl}/api/workflows`)
      .pipe(map((response) => response.workflows));
  }

  getWorkflow(workflowId: string): Observable<Workflow> {
    return this.httpClient
      .get<Workflow>(`${this.apiUrl}/api/workflows/${workflowId}`)
      .pipe(map((response) => response));
  }

  deleteWorkflow(workflowId: string): Observable<boolean> {
    return this.httpClient
      .delete<Workflow>(`${this.apiUrl}/api/workflows/${workflowId}`)
      .pipe(
        map(() => {
          return true;
        }),
        catchError(() => {
          return of(false);
        })
      );
  }

  getEmbeddingModels(): Observable<EmbeddingModelsResponse> {
    return this.httpClient
      .get<EmbeddingModelsResponse>(`${this.apiUrl}/api/models/embedding`)
      .pipe(map((response) => response));
  }
}
