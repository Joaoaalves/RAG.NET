import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError, map, Observable, of } from 'rxjs';
import {
  Filter,
  FilterEnableResponse,
  FilterStrategyEnum,
  FilterUpdateResponse,
} from '../models/filter';

@Injectable({
  providedIn: 'root',
})
export class FilterService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  private getEndpoint(workflowId: string, type: string): string {
    return `${this.apiUrl}/api/workflows/${workflowId}/content-filter/${type}`;
  }

  enableFilter(
    filter: Filter,
    workflowId: string,
    strategy: string | FilterStrategyEnum
  ): Observable<Filter> {
    return this.httpClient
      .post<FilterEnableResponse>(
        this.getEndpoint(workflowId, this.mapFilterStrategyToString(strategy)),
        filter
      )
      .pipe(map((response) => response.filter));
  }

  updateFilter(
    filter: Filter,
    workflowId: string,
    strategy: string | FilterStrategyEnum
  ): Observable<Filter> {
    return this.httpClient
      .put<FilterUpdateResponse>(
        this.getEndpoint(workflowId, this.mapFilterStrategyToString(strategy)),
        {
          maxItems: filter.maxItems,
          isEnabled: filter.isEnabled,
        }
      )
      .pipe(map((response) => response.filter));
  }

  toggleFilter(
    filter: Filter,
    workflowId: string,
    strategy: string | FilterStrategyEnum,
    newStatus: boolean
  ): Observable<Filter> {
    const updatedFilter = { ...filter, isEnabled: newStatus };
    return this.updateFilter(
      updatedFilter,
      workflowId,
      this.mapFilterStrategyToString(strategy)
    );
  }

  mapFilterStrategyToString(
    filterStrategy: FilterStrategyEnum | string
  ): string {
    switch (filterStrategy) {
      case FilterStrategyEnum.RSE:
        return 'rse';
      default:
        return 'rse';
    }
  }
}
