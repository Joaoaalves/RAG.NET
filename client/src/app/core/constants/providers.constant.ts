import { ProviderData, SupportedProvider } from 'src/app/models/provider';

export const PROVIDERS_DATA: Record<SupportedProvider, ProviderData> = {
  openai: {
    id: 0,
    title: 'OpenAI',
    description:
      'OpenAI is an AI research and deployment company. You can find the API Key in your OpenAI account settings. On https://platform.openai.com/api-keys',
    icon: 'img/providers/openai.svg',
    keyTemplate: 'sk-proj-***************',
  },
  anthropic: {
    id: 1,
    title: 'Anthropic',
    description:
      'Anthropic is an AI safety and research company. You can find the API Key in your Anthropic account settings. On https://console.anthropic.com/settings/keys',
    icon: 'img/providers/anthropic.svg',
    keyTemplate: 'sk-***************',
  },
  voyage: {
    id: 2,
    title: 'Voyage',
    description:
      'Voyage is a conversational AI platform. You can find the API Key in your Voyage account settings. On https://voyage.ai/account/settings',
    icon: 'img/providers/voyage.svg',
    keyTemplate: 'proj-***************',
  },
  qdrant: {
    id: 3,
    title: 'QDrant',
    description:
      'QDrant is an open-source vector search engine. You can find the API Key in your QDrant account settings. On https://dashboard.voyageai.com/api-keys',
    icon: 'img/providers/qdrant.svg',
    keyTemplate: '***************',
  },
};
