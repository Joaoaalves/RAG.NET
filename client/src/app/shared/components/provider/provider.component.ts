import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { InputComponent } from '../input/input.component';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { toast } from 'ngx-sonner';
import {
  Provider,
  ProviderData,
  SupportedProvider,
} from 'src/app/models/provider';
import { ProvidersService } from 'src/app/services/providers.service';
import { provideIcons } from '@ng-icons/core';
import {
  heroArrowTopRightOnSquare,
  heroPencil,
  heroXMark,
} from '@ng-icons/heroicons/outline';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-provider',
  templateUrl: './provider.component.html',
  imports: [
    CommonModule,
    InputComponent,
    ReactiveFormsModule,
    FormsModule,
    NgIcon,
  ],
  providers: [
    provideIcons({
      heroPencil,
      heroXMark,
      heroArrowTopRightOnSquare,
    }),
  ],
  standalone: true,
})
export class ProviderComponent implements OnInit {
  @Input() Provider!: Provider;

  providerData: ProviderData | undefined;
  form: FormGroup;
  hasApiKey: boolean = false;
  isUpdating: boolean = false;

  constructor(
    private fb: FormBuilder,
    private providersService: ProvidersService
  ) {
    this.form = this.fb.group({
      apiKey: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.providerData = this.providersService.mapProviderData(
      this.Provider.provider.toLowerCase() as SupportedProvider
    );

    this.hasApiKey = !!this.Provider.apiKey;

    if (this.Provider.apiKey && this.providerData?.keyTemplate) {
      const visibleKey =
        this.providerData?.keyTemplate.slice(0, -this.Provider.apiKey.length) +
        this.Provider.apiKey;
      this.form.patchValue({ apiKey: visibleKey });
    }

    this.form.get('apiKey')?.valueChanges.subscribe((value: string) => {
      if (value && this.providerData?.keyTemplate) {
        const lastPart = value.slice(-4);
        this.Provider.apiKey = lastPart;
      }
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();
    if (this.form.valid) {
      this.hasApiKey ? this.update() : this.add();
      return;
    }

    toast.error('Invalid API Key', {
      description: 'You should provide a valid API Key.',
    });
  }

  navigateToProvider() {
    if (this.providerData?.apiKeyUrl) {
      window.open(this.providerData.apiKeyUrl, '_blank');
    } else {
      toast.error('Unable to open link', {
        description: 'No URL found for this provider.',
      });
    }
  }
  startEdition() {
    this.isUpdating = true;
    this.form.setValue({ apiKey: '' });
  }

  finishApiKeyEdition(apiKey?: string) {
    this.isUpdating = false;
    const suffix = apiKey?.slice(0, -4) ?? this.Provider.apiKey;
    this.form.setValue({
      apiKey: this.providerData?.keyTemplate.slice(0, -4) + suffix,
    });
  }

  update() {
    const apiKey: string = this.form.get('apiKey')?.value;

    if (!apiKey) {
      toast.error('Invalid API Key', {
        description: 'You should provide a valid API Key.',
      });

      this.finishApiKeyEdition();
      return;
    }

    try {
      this.providersService
        .updateProvider(
          this.Provider.id,
          apiKey,
          this.Provider.provider.toLowerCase() as SupportedProvider
        )
        .subscribe({
          next: () => {
            toast.success('Provider updated successfully');

            this.finishApiKeyEdition();
          },
          error: (msg) => {
            toast.error('Update failed', {
              description: `${msg}`,
            });
          },
        });
    } catch (err) {
      toast.error('Update failed', {
        description: `${err}`,
      });
    }
  }

  delete() {
    this.providersService.deleteProvider(this.Provider.id).subscribe({
      next: () => {
        toast.success('Provider deleted successfully', {
          description: 'The provider has been deleted successfully.',
        });

        this.form.reset();
        this.hasApiKey = false;
        this.Provider.apiKey = '';
      },
      error: (msg) => {
        toast.error('An error occurred', {
          description: msg,
        });
      },
    });
  }

  add() {
    const apiKey = this.form.get('apiKey')?.value;

    if (!apiKey) {
      toast.error('Invalid API Key', {
        description: 'You should provide a valid API Key.',
      });
      return;
    }
    try {
      this.providersService
        .addProvider(
          this.Provider.provider.toLowerCase() as SupportedProvider,
          apiKey
        )
        .subscribe({
          next: () => {
            toast.success('Provider saved successfully', {
              description: 'The provider has been saved successfully.',
            });
            this.hasApiKey = true;

            this.finishApiKeyEdition(apiKey);
          },
          error: (msg) => {
            toast.error('An error occurred', {
              description: msg,
            });
          },
        });
    } catch (err) {
      toast.error('An error occurred', {
        description: `${err}`,
      });
    }
  }
}
