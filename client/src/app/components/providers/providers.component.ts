import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import {
  GetProvidersResponse,
  Provider,
  SupportedProvider,
} from 'src/app/models/provider';
import { ProvidersService } from 'src/app/services/providers.service';
import { ProviderComponent } from 'src/app/shared/components/provider/provider.component';

@Component({
  standalone: true,
  imports: [CommonModule, HlmToasterComponent, ProviderComponent],
  templateUrl: './providers.component.html',
})
export class ProvidersComponent implements OnInit {
  providers: GetProvidersResponse = [];

  providerMap: Record<SupportedProvider, Provider> = {
    openai: this.getDefaultProvider('openai'),
    anthropic: this.getDefaultProvider('anthropic'),
    gemini: this.getDefaultProvider('gemini'),
    voyage: this.getDefaultProvider('voyage'),
    qdrant: this.getDefaultProvider('qdrant'),
  };

  constructor(private providersService: ProvidersService) {}

  ngOnInit(): void {
    this.getUserProviders();
  }

  getUserProviders() {
    this.providersService.getUserProviders().subscribe((response) => {
      this.providers = response;

      for (const p of response) {
        const key = p.provider.toLowerCase() as SupportedProvider;
        this.providerMap[key] = p;
      }
    });
  }

  getDefaultProvider(provider: string): Provider {
    return {
      id: '',
      apiKey: '',
      userId: '',
      provider: provider as SupportedProvider,
    };
  }
}
