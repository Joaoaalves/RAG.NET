import { ChunkerSettings, ChunkerStrategy } from './chunker';
import { EmbeddingProvider } from './workflow';

export interface CreateWorkflowRequest {
  name: string;
  description: string;
  strategy: ChunkerStrategy;
  settings: ChunkerSettings;
  embeddingProvider: EmbeddingProvider;
}

export interface CreateWorkflowResponse {
  message: string;
  workflowId: string;
}
