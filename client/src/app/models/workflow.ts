import { ChunkerSettings, ChunkerStrategy } from './chunker';
import { EmbeddingProvider } from './embedding';

export interface Workflow {
  id: string;
  name: string;
  description: string;
  documents: number;
  strategy: ChunkerStrategy;
  settings: ChunkerSettings;
  apiKey: string;
  embeddingProvider: EmbeddingProvider;
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
}

export interface CreateWorkflowResponse {
  message: string;
  workflowId: string;
}
