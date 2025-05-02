import { CallbackUrl } from './callback-url';
import { ConversationProvider } from './chat';
import { ChunkerSettings, ChunkerStrategy } from './chunker';
import { EmbeddingProvider } from './embedding';
import { Filter } from './filter';
import { QueryEnhancer } from './query-enhancer';

export interface Workflow {
  id: string;
  name: string;
  description: string;
  documentsCount: number;
  strategy: ChunkerStrategy;
  settings: ChunkerSettings;
  apiKey: string;
  embeddingProvider: EmbeddingProvider;
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
  conversationProvider: ConversationProvider;
}

export interface QueryEnhancerUpdateResponse {
  message: string;
  queryEnhancer: QueryEnhancer;
}

export interface CreateWorkflowResponse {
  message: string;
  workflowId: string;
}
