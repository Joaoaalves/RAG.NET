export function getProviderImage(providerId: number): string {
  return (
    {
      0: '/img/providers/openai.svg',
      1: '/img/providers/anthropic.svg',
      2: '/img/providers/voyage.svg',
      3: '/img/providers/qdrant.svg',
      4: '/img/providers/gemini.svg',
      5: '/img/providers/deepseek.svg',
      6: '/img/providers/xai.svg',
      7: '/img/providers/mistral.svg',
    }[providerId] ?? ''
  );
}
