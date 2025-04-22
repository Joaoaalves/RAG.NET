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
import { heroPencil, heroXMark } from '@ng-icons/heroicons/outline';
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

  startUpdating() {
    this.isUpdating = true;
    this.form.setValue({ apiKey: '' });
  }

  cancelUpdating() {
    this.isUpdating = false;
    this.form.setValue({
      apiKey: this.providerData?.keyTemplate + this.Provider.apiKey,
    });
  }

  update() {
    const apiKey = this.form.get('apiKey')?.value;

    if (!apiKey) {
      toast.error('Invalid API Key', {
        description: 'You should provide a valid API Key.',
      });
      return;
    }

    this.providersService.updateProvider(this.Provider.id, apiKey).subscribe({
      next: () => {
        toast.success('Provider updated successfully');
      },
    });
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
        },
        error: (msg) => {
          toast.error('An error occurred', {
            description: msg,
          });
        },
      });
  }
}
