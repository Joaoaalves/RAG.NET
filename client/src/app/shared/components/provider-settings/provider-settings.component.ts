import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { ionStar, ionStarHalf, ionStarOutline } from '@ng-icons/ionicons';
import {
  lucideChevronDown,
  lucideClock,
  lucideCode,
  lucideDatabase,
  lucideDollarSign,
  lucideInfo,
  lucideMessageCircle,
  lucideStar,
  lucideZap,
} from '@ng-icons/lucide';
import { ConversationModel } from 'src/app/models/chat';
import { EmbeddingModel } from 'src/app/models/embedding';
import { ProviderOption } from 'src/app/models/provider';

import { SelectComponent } from '../select/select.component';

import { Observable, startWith, switchMap, take, tap } from 'rxjs';
import { ProviderSelectService } from 'src/app/services/provider-select.service';

@Component({
  selector: 'app-provider-settings',
  templateUrl: './provider-settings.component.html',
  imports: [NgIcon, CommonModule, ReactiveFormsModule, SelectComponent],
  providers: [
    provideIcons({
      lucideZap,
      lucideDatabase,
      lucideInfo,
      lucideDollarSign,
      lucideClock,
      lucideStar,
      ionStar,
      ionStarHalf,
      ionStarOutline,
      lucideChevronDown,
      lucideCode,
      lucideMessageCircle,
    }),
  ],
  standalone: true,
})
export class ProviderSettingsComponent implements OnInit {
  @Input() currentEmbedding!: number;
  @Input() currentConversation!: number;

  embeddingForm!: FormGroup;
  conversationForm!: FormGroup;

  embeddingOptions$!: Observable<{ value: string | number; label: string }[]>;
  conversationOptions$!: Observable<
    { value: string | number; label: string }[]
  >;
  embeddingModels$!: Observable<EmbeddingModel[]>;
  conversationModels$!: Observable<ConversationModel[]>;

  selectedEmbeddingModel: EmbeddingModel | null = null;
  selectedConversationModel: ConversationModel | null = null;

  constructor(private fb: FormBuilder, private ps: ProviderSelectService) {}

  ngOnInit(): void {
    this.initForms();
    this.embeddingOptions$ = this.ps.getEmbeddingProviders();
    this.conversationOptions$ = this.ps.getConversationProviders();

    const embProv = this.embeddingForm.get('provider')!;
    this.embeddingModels$ = embProv.valueChanges.pipe(
      startWith(this.currentEmbedding),
      switchMap((id) => this.ps.getEmbeddingModels(id)),
      tap(() => this.embeddingForm.get('model')!.reset())
    );

    this.embeddingForm.get('model')!.valueChanges.subscribe((val) => {
      this.ps
        .getEmbeddingModels(embProv.value)
        .pipe(take(1))
        .subscribe((list) => {
          this.selectedEmbeddingModel =
            list.find((m) => m.value === val) || null;
        });
    });

    const convProv = this.conversationForm.get('provider')!;
    this.conversationModels$ = convProv.valueChanges.pipe(
      startWith(this.currentConversation),
      switchMap((id) => this.ps.getConversationModels(id)),
      tap(() => this.conversationForm.get('model')!.reset())
    );

    this.conversationForm.get('model')!.valueChanges.subscribe((val) => {
      this.ps
        .getConversationModels(convProv.value)
        .pipe(take(1))
        .subscribe((list) => {
          this.selectedConversationModel =
            list.find((m) => m.value === val) || null;
        });
    });
  }

  private initForms(): void {
    this.embeddingForm = this.fb.group({
      provider: [this.currentEmbedding, Validators.required],
      model: [null, Validators.required],
    });
    this.conversationForm = this.fb.group({
      provider: [this.currentConversation, Validators.required],
      model: [null, Validators.required],
    });
  }
}
