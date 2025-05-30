import { ProviderData, SupportedProvider } from 'src/app/models/provider';

export const PROVIDERS_DATA: Record<SupportedProvider, ProviderData> = {
  openai: {
    id: 0,
    title: 'OpenAI',
    description:
      'OpenAI provides language models used for tasks such as text generation, summarization, and reasoning in retrieval-augmented generation (RAG) pipelines. These models are commonly used alongside vector search systems to generate context-aware responses based on retrieved data.',
    icon: 'img/providers/openai.svg',
    apiKeyUrl: 'https://platform.openai.com/api-keys',
    keyTemplate: 'sk-proj-***************',
    regex: '^sk-proj-[A-Za-z0-9_-]{120,200}$',
  },
  anthropic: {
    id: 1,
    title: 'Anthropic',
    description:
      'Anthropic provides language models such as Claude, which are used for generating text and handling instructions in RAG-based systems. These models can process large contexts and are often used for chatbots and document analysis.',
    icon: 'img/providers/anthropic.svg',
    apiKeyUrl: 'https://console.anthropic.com/settings/keys',
    keyTemplate: 'sk-ant-***************',
    regex: '^sk-ant-[a-z0-9-]+-[A-Za-z0-9_-]{80,140}$',
  },
  voyage: {
    id: 2,
    title: 'Voyage',
    description:
      'Voyage AI provides embedding models and rerankers for semantic search and retrieval-augmented generation (RAG) systems. Embedding models convert unstructured data into dense semantic vectors, while rerankers evaluate relevance between queries and documents to improve retrieval accuracy.',
    icon: 'img/providers/voyage.svg',
    apiKeyUrl: 'https://voyage.ai/account/settings',
    keyTemplate: 'pa-***************',
    regex: '^pa-[A-Za-z0-9-]{30,60}$',
  },
  qdrant: {
    id: 3,
    title: 'QDrant',
    description:
      'QDrant is a vector database used for storing and searching dense embeddings. It supports similarity search operations, making it a key component in RAG architectures where relevant context is retrieved based on vector similarity before LLM processing.',
    icon: 'img/providers/qdrant.svg',
    apiKeyUrl: 'https://cloud.qdrant.io/',
    keyTemplate: '***************',
    regex: '^[\\w-]*.[\\w-]*.[\\w-]*$',
  },
  gemini: {
    id: 4,
    title: 'Gemini',
    description: '',
    icon: 'img/providers/gemini.svg',
    apiKeyUrl: 'https://aistudio.google.com/app/apikey',
    keyTemplate: '*****************',
    regex: '^AIza[0-9A-Za-z_-]{35}$',
  },
};
