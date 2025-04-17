import { PROVIDER_MAP } from 'src/app/core/constants/providers.constant';
import { ProviderOption } from 'src/app/models/provider';

/**
 * Get label and value for a given provider name.
 * Returns null if provider is not in the map.
 */
export function getProviderOption(provider: string): ProviderOption | null {
  const key = provider.toLowerCase();
  return PROVIDER_MAP[key] ?? null;
}

/**
 * Extract valid provider options from a dict of providers with model arrays.
 * Only returns providers that have at least one model.
 */
export function mapValidProviders<T extends Record<string, unknown[]>>(
  response: T
): ProviderOption[] {
  return Object.entries(response)
    .filter(([_, models]) => Array.isArray(models) && models.length > 0)
    .map(([provider]) => {
      const option = getProviderOption(provider);
      return option ? option : null;
    })
    .filter((option): option is ProviderOption => option !== null);
}

export function getProviderKeyByValueFromResponse(
  value: number,
  response: Record<string, unknown[]>
): string | null {
  // Search by matching the mapped label to the provider key in the response
  const providerEntry = Object.entries(response).find(([key]) => {
    const normalizedKey = key.toLowerCase();
    const mapped = PROVIDER_MAP[normalizedKey];
    return mapped?.value === value;
  });

  return providerEntry ? providerEntry[0] : null; // preserve original casing
}
