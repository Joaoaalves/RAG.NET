import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';

import {
  Workflow,
  WorkflowsInfoResponse,
  CreateWorkflowRequest,
  CreateWorkflowResponse,
  WorkflowUpdateRequest,
} from '../models/workflow';
import { ProviderResponse } from '../models/provider';
import { ConversationModel, EmbeddingModel } from '../models/models';

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

  updateWorkflow(
    data: WorkflowUpdateRequest,
    workflowId: string
  ): Observable<Workflow> {
    return this.httpClient
      .put<Workflow>(`${this.apiUrl}/api/workflows/${workflowId}`, data)
      .pipe(map((response) => response));
  }

  toggleWorkflow(isActive: boolean, workflowId: string): Observable<Workflow> {
    return this.httpClient
      .put<Workflow>(`${this.apiUrl}/api/workflows/${workflowId}`, {
        isActive,
      })
      .pipe(map((response) => response));
  }

  getEmbeddingModels(): Observable<ProviderResponse<EmbeddingModel>[]> {
    return this.httpClient
      .get<ProviderResponse<EmbeddingModel>[]>(
        `${this.apiUrl}/api/models/embedding`
      )
      .pipe(map((response) => response));
  }

  getConversationModels(): Observable<ProviderResponse<ConversationModel>[]> {
    return this.httpClient
      .get<ProviderResponse<ConversationModel>[]>(
        `${this.apiUrl}/api/models/conversation`
      )
      .pipe(map((response) => response));
  }
}
