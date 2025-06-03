import { ProviderModel } from './provider';

export enum ConversationProviderEnum {
  OPENAI = 0,
  ANTHROPIC = 1,
}

export type ConversationProvider = ProviderModel;
