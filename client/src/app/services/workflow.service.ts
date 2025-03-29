import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Workflow, WorkflowsResponse } from '../models/workflow';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class WorkflowService {
  private apiUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) {}

  getWorkflows(): Observable<Workflow[]> {
    return this.httpClient
      .get<WorkflowsResponse>(`${this.apiUrl}/api/workflows`)
      .pipe(map((response) => response.workflows));
  }
}
