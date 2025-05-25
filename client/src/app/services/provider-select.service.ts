import { Injectable } from '@angular/core';
import { ReplaySubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { WorkflowService } from 'src/app/services/workflow.service';
import {
  ProviderData,
  ProviderOption,
  ProvidersResponse,
} from 'src/app/models/provider';
import { EmbeddingModel } from 'src/app/models/embedding';
import { ConversationModel } from 'src/app/models/chat';
import {
  getProviderKeyByValueFromResponse,
  mapValidProviders,
} from 'src/app/shared/utils/providers-utils';

@Injectable({ providedIn: 'root' })
export class ProviderSelectService {
  private embResp$ = new ReplaySubject<ProvidersResponse<EmbeddingModel>>(1);
  private convResp$ = new ReplaySubject<ProvidersResponse<ConversationModel>>(
    1
  );

  constructor(private workflow: WorkflowService) {
    this.workflow.getEmbeddingModels().subscribe(this.embResp$);
    this.workflow.getConversationModels().subscribe(this.convResp$);
  }

  getEmbeddingProviders(): Observable<
    { value: string | number; label: string }[]
  > {
    return this.embResp$.pipe(
      map((res) =>
        mapValidProviders(res).map((p: ProviderData) => ({
          label: p.title,
          value: p.id,
        }))
      )
    );
  }

  getConversationProviders(): Observable<
    { value: string | number; label: string }[]
  > {
    return this.convResp$.pipe(
      map((res) =>
        mapValidProviders(res).map((p: ProviderData) => ({
          label: p.title,
          value: p.id,
        }))
      )
    );
  }

  getEmbeddingModels(providerId: number): Observable<EmbeddingModel[]> {
    return this.embResp$.pipe(
      map((res) => {
        const key = getProviderKeyByValueFromResponse(providerId, res);
        return key && res[key] ? res[key] : [];
      })
    );
  }

  getConversationModels(providerId: number): Observable<ConversationModel[]> {
    return this.convResp$.pipe(
      map((res) => {
        const key = getProviderKeyByValueFromResponse(providerId, res);
        return key && res[key] ? res[key] : [];
      })
    );
  }
}
