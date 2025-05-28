import { PROVIDERS_DATA } from 'src/app/core/constants/providers.constant';
import { ProviderData, SupportedProvider } from 'src/app/models/provider';

/**
 * Get the numeric ID for a given provider name.
 */
export function getProviderIdFromName(name: string): number {
  const key = name.toLowerCase() as SupportedProvider;
  return PROVIDERS_DATA[key]?.id ?? -1;
}

/**
 * Get the provider name from its numeric ID.
 */
export function getProviderNameFromId(id: number): SupportedProvider | null {
  const entry = Object.entries(PROVIDERS_DATA).find(
    ([_, data]) => data.id === id
  );
  return entry ? (entry[0] as SupportedProvider) : null;
}

/**
 * Get label and value for a given provider name.
 * Returns null if provider is not in the map.
 */
export function getProviderOption(provider: SupportedProvider): ProviderData {
  return PROVIDERS_DATA[provider];
}

/**
 * Extract valid provider options from a dict of providers with model arrays.
 * Only returns providers that have at least one model.
 */
export function mapValidProviders<T extends Record<string, unknown[]>>(
  response: T
): ProviderData[] {
  return Object.entries(response)
    .filter(([_, models]) => Array.isArray(models) && models.length > 0)
    .map(([provider]) => {
      const option = getProviderOption(
        provider.toLowerCase() as SupportedProvider
      );
      return option ? option : null;
    })
    .filter((option): option is ProviderData => option !== null);
}

export function getProviderKeyByValueFromResponse(
  value: number,
  response: Record<string, unknown[]>
): string | null {
  // Search by matching the mapped label to the provider key in the response
  const providerEntry = Object.entries(response).find(([key]) => {
    const normalizedKey = key.toLowerCase();
    const mapped = PROVIDERS_DATA[normalizedKey as SupportedProvider];
    return mapped?.id === value;
  });

  return providerEntry ? providerEntry[0] : null; // preserve original casing
}
