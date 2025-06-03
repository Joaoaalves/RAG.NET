import { Injectable } from '@angular/core';
import { ReplaySubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { WorkflowService } from 'src/app/services/workflow.service';
import { ConversationModel, EmbeddingModel } from '../models/models';
import { ProviderResponse } from '../models/provider';

import { SelectOption } from '../models/select';

@Injectable({ providedIn: 'root' })
export class ProviderSelectService {
  private embResp$ = new ReplaySubject<ProviderResponse<EmbeddingModel>[]>(1);
  private convResp$ = new ReplaySubject<ProviderResponse<ConversationModel>[]>(
    1
  );

  constructor(private workflow: WorkflowService) {
    this.workflow.getEmbeddingModels().subscribe(this.embResp$);
    this.workflow.getConversationModels().subscribe(this.convResp$);
  }

  getEmbeddingProvidersAsSelectOptions(): Observable<SelectOption[]> {
    return this.embResp$.pipe(
      map((providers) =>
        providers.map((prov) => {
          return {
            label: prov.providerName,
            value: prov.providerId,
          };
        })
      )
    );
  }

  getConversationProvidersAsSelectOptions(): Observable<SelectOption[]> {
    return this.convResp$.pipe(
      map((providers) =>
        providers.map((prov) => {
          return {
            label: prov.providerName,
            value: prov.providerId,
          };
        })
      )
    );
  }

  getEmbeddingModels(providerId: number): Observable<EmbeddingModel[]> {
    return this.embResp$.pipe(
      map((res) => {
        const provider = res.find((curr) => curr.providerId === providerId);

        return provider ? provider.models : [];
      })
    );
  }

  getConversationModels(providerId: number): Observable<ConversationModel[]> {
    return this.convResp$.pipe(
      map((res) => {
        const provider = res.find((curr) => curr.providerId === providerId);

        return provider ? provider.models : [];
      })
    );
  }
}
