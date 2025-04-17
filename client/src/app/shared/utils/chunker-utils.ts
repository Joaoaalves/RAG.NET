import { ChunkerStrategy } from 'src/app/models/chunker';
import { ProviderOption } from 'src/app/models/provider';

export const mapChunkerStrategies = (): ProviderOption[] => {
  return Object.entries(ChunkerStrategy)
    .filter(([key, value]) => !isNaN(Number(value)))
    .map(([key, value]) => ({
      label: key.charAt(0).toUpperCase() + key.slice(1).toLowerCase(),
      value: Number(value),
    }));
};
