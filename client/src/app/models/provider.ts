import { ConversationModel, EmbeddingModel } from './models';

export interface ProviderOption {
  label: string;
  value: string | number;
}

export interface ProviderModel {
  providerId: number;
  providerName: string;
  model: string;
}

export type SupportedProvider =
  | 'openai'
  | 'anthropic'
  | 'voyage'
  | 'qdrant'
  | 'gemini';

export interface ProviderResponse<
  T extends ConversationModel | EmbeddingModel
> {
  providerId: number;
  providerName: string;
  models: T[];
}

export interface Provider {
  id?: string;
  apiKey: string;
  providerId: number;
  name: string;
  pattern: string;
  prefix: string;
  url: string;
}
