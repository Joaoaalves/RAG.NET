import { ConversationModel } from './chat';
import { EmbeddingModel } from './embedding';

export interface ProviderOption {
  label: string;
  value: number;
}

export type SupportedProvider = 'openai' | 'anthropic' | 'voyage' | 'qdrant';

export type GetProvidersResponse = Provider[];

export interface ProvidersResponse<
  T extends EmbeddingModel | ConversationModel
> {
  [key: string]: T[];
}

export interface Provider {
  id: string;
  apiKey: string;
  userId: string;
  provider: string;
}

export interface ProviderData {
  id: number;
  title: string;
  description: string;
  icon: string;
  keyTemplate: string;
  regex: string;
}
