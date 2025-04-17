import { ProviderOption } from 'src/app/models/provider';

export const PROVIDER_MAP: Record<string, ProviderOption> = {
  openai: { label: 'OpenAI', value: 0 },
  anthropic: { label: 'Anthropic', value: 1 },
  voyage: { label: 'Voyage', value: 2 },
  qdrant: { label: 'QDrant', value: 3 },
};
