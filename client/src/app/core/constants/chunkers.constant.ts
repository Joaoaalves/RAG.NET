import { ProviderOption } from 'src/app/models/provider';

export const CHUNKER_MAP: Record<string, ProviderOption> = {
  proposition: { label: 'Proposition', value: 0 },
  semantic: { label: 'Semantic', value: 1 },
  paragraph: { label: 'Paragraph', value: 2 },
};
