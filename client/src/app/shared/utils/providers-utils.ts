export function getProviderImage(providerId: number): string {
  return (
    {
      0: '/img/providers/openai.svg',
      1: '/img/providers/anthropic.svg',
      2: '/img/providers/voyage.svg',
      3: '/img/providers/gemini.svg',
      4: '/img/providers/deepseek.svg',
      5: '/img/providers/xai.svg',
      6: '/img/providers/mistral.svg',
    }[providerId] ?? ''
  );
}
