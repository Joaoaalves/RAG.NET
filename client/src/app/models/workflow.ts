import { CallbackUrl } from './callback-url';
import { ChunkerSettings, ChunkerStrategy } from './chunker';
import { EmbeddingProvider } from './embedding';
import { Filter } from './filter';
import { ProviderModel } from './provider';
import { QueryEnhancer } from './query-enhancer';

export interface Workflow {
  id: string;
  name: string;
  description: string;
  isActive: boolean;
  documentsCount: number;
  strategy: ChunkerStrategy;
  settings: ChunkerSettings;
  apiKey: string;
  embeddingProvider: ProviderModel;
  conversationProvider: ProviderModel;
  queryEnhancers: QueryEnhancer[];
  filter?: Filter;
  callbackUrls?: CallbackUrl[];
}

export interface WorkflowsInfoResponse {
  workflows: Workflow[];
}

export interface CreateWorkflowRequest {
  name: string;
  description: string;
  strategy: ChunkerStrategy;
  settings: ChunkerSettings;
  embeddingProvider: EmbeddingProvider;
  conversationProvider: ProviderModel;
}

export interface QueryEnhancerUpdateResponse {
  message: string;
  queryEnhancer: QueryEnhancer;
}

export interface CreateWorkflowResponse {
  message: string;
  workflowId: string;
}

export interface WorkflowUpdateRequest {
  name?: string;
  description?: string;
  embeddingProvider?: {
    providerId: number;
    model: string;
  };
  conversationProvider?: {
    providerId: number;
    model: string;
  };
}
