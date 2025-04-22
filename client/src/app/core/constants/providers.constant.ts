import { ProviderData, SupportedProvider } from 'src/app/models/provider';

export const PROVIDERS_DATA: Record<SupportedProvider, ProviderData> = {
  openai: {
    id: 0,
    title: 'OpenAI',
    description:
      'OpenAI provides powerful language models used for text generation and reasoning in our RAG pipelines. You can retrieve your API key in your OpenAI account settings at https://platform.openai.com/api-keys.',
    icon: 'img/providers/openai.svg',
    keyTemplate: 'sk-proj-***************',
    regex: '^sk-proj-[A-Za-z0-9_-]{120,200}$',
  },
  anthropic: {
    id: 1,
    title: 'Anthropic',
    description:
      'Anthropic offers advanced language models like Claude, which are used in our system for generating high-quality completions and responses within RAG workflows. API keys are available in your Anthropic dashboard at https://console.anthropic.com/settings/keys.',
    icon: 'img/providers/anthropic.svg',
    keyTemplate: 'sk-ant-***************',
    regex: '^sk-ant-[a-z0-9-]+-[A-Za-z0-9_-]{80,140}$',
  },
  voyage: {
    id: 2,
    title: 'Voyage',
    description:
      'Voyage AI provides embedding models and rerankers for semantic search and retrieval-augmented generation (RAG) systems. Embedding models convert unstructured data—such as documents or queries—into dense vectors that represent semantic meaning. Rerankers evaluate the relevance between a query and a set of documents to refine retrieval results. These components can be integrated with vector databases and language models in RAG pipelines. API keys are available at https://voyage.ai/account/settings.',
    icon: 'img/providers/voyage.svg',
    keyTemplate: 'pa-***************',
    regex: '^pa-[A-Za-z0-9-]{30,60}$',
  },
  qdrant: {
    id: 3,
    title: 'QDrant',
    description:
      'QDrant is a vector database optimized for similarity search, used to store and retrieve embeddings as part of our RAG architecture. You can get your API key at https://cloud.qdrant.io/api-keys.',
    icon: 'img/providers/qdrant.svg',
    keyTemplate: '***************',
    regex: '^[A-Za-z0-9_-]+\\.[A-Za-z0-9_-]+\\.[A-Za-z0-9_-]+$',
  },
};
