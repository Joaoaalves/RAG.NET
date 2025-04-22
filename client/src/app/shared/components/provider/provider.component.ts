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
import { ProviderData, SupportedProvider } from 'src/app/models/provider';
import { ProvidersService } from 'src/app/services/providers.service';

@Component({
  selector: 'app-provider',
  templateUrl: './provider.component.html',
  imports: [CommonModule, InputComponent, ReactiveFormsModule, FormsModule],
  standalone: true,
})
export class ProviderComponent implements OnInit {
  @Input() Provider: SupportedProvider = 'openai';
  @Input() ApiKey: string = '';

  providerData: ProviderData | undefined;
  form: FormGroup;
  hasApiKey: boolean = false;

  constructor(
    private fb: FormBuilder,
    private providersService: ProvidersService
  ) {
    this.form = this.fb.group({
      apiKey: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.providerData = this.providersService.getProviderData(this.Provider);

    this.hasApiKey = !!this.ApiKey;

    if (this.ApiKey && this.providerData?.keyTemplate) {
      const visibleKey =
        this.providerData?.keyTemplate.slice(0, -this.ApiKey.length) +
        this.ApiKey;
      this.form.patchValue({ apiKey: visibleKey });
    }

    this.form.get('apiKey')?.valueChanges.subscribe((value: string) => {
      if (value && this.providerData?.keyTemplate) {
        const lastPart = value.slice(-4);
        this.ApiKey = lastPart;
      }
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();
    if (this.form.valid) {
      this.hasApiKey ? this.save() : this.add();
      return;
    }

    toast.error('Invalid API Key', {
      description: 'You should provide a valid API Key.',
    });
  }

  save() {
    toast.success('Provider saved successfully', {
      description: 'The provider has been saved successfully.',
    });
  }

  delete() {
    toast.error('Provider deleted successfully', {
      description: 'The provider has been deleted successfully.',
    });

    this.form.reset();
    this.hasApiKey = false;
    this.ApiKey = '';
  }

  add() {
    toast.success('Provider added successfully', {
      description: 'The provider has been added successfully.',
    });
    this.hasApiKey = true;
  }
}
