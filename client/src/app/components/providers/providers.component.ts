import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import { Provider, SupportedProvider } from 'src/app/models/provider';
import { ProvidersService } from 'src/app/services/providers.service';
import { ProviderComponent } from 'src/app/shared/components/provider/provider.component';

@Component({
  standalone: true,
  imports: [CommonModule, HlmToasterComponent, ProviderComponent],
  templateUrl: './providers.component.html',
})
export class ProvidersComponent implements OnInit {
  providers: Provider[] = [];
  constructor(private providersService: ProvidersService) {}

  ngOnInit(): void {
    this.getUserProviders();
  }

  getProvider(provider: string): Provider {
    const prov = this.providers.find(
      (prov) => prov.name.toLowerCase() === provider.toLowerCase()
    );

    return prov ?? this.getDefaultProvider(provider);
  }

  getDefaultProvider(provider: string): Provider {
    return {
      apiKey: '',
      providerId: 0,
      name: provider as SupportedProvider,
      pattern: '',
      prefix: '',
      url: '',
    };
  }

  getUserProviders() {
    this.providersService.getUserProviders().subscribe((response) => {
      this.providers = response;
    });
  }
}
