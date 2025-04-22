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
  constructor(private providersService: ProvidersService) {}

  ngOnInit(): void {
    this.getUserProviders();
  }

  getProvider(provider: string): Provider {
    if (!this.providers || this.providers.length === 0) {
      return this.getDefaultProvider(provider);
    }

    const prov = this.providers.find(
      (prov) => prov.provider.toLowerCase() === provider.toLowerCase()
    );

    return prov ?? this.getDefaultProvider(provider);
  }

  getDefaultProvider(provider: string): Provider {
    return {
      id: '',
      apiKey: '',
      userId: '',
      provider: provider as SupportedProvider,
    };
  }

  getUserProviders() {
    this.providersService.getUserProviders().subscribe((response) => {
      this.providers = response;
    });
  }
}
