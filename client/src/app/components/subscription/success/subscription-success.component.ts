import {
  Component,
  ElementRef,
  AfterViewInit,
  ViewChild,
  inject,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { gsap } from 'gsap';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideCheck } from '@ng-icons/lucide';
import { UserService } from 'src/app/services/user.service';
import { PlanType } from 'src/app/models/subscription';

@Component({
  standalone: true,
  imports: [CommonModule, NgIcon],
  providers: [
    provideIcons({
      lucideCheck,
    }),
  ],
  templateUrl: './subscription-success.component.html',
})
export class SubscriptionSuccessComponent implements AfterViewInit {
  @ViewChild('checkIcon', { static: false })
  checkIcon!: ElementRef<SVGSVGElement>;
  isSubscribed: boolean = false;
  paymentId: string | null = null;

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngAfterViewInit(): void {
    this.paymentId = this.route.snapshot.queryParamMap.get('paymentId');
    if (!this.paymentId) {
      this.router.navigate(['/dashboard/workflows']);
      return;
    }

    if (this.checkIcon) {
      const path = this.checkIcon.nativeElement.querySelector('path');
      if (path) {
        gsap.set(path, {
          strokeDasharray: path.getTotalLength(),
          strokeDashoffset: path.getTotalLength(),
        });
        gsap.to(path, {
          strokeDashoffset: 0,
          duration: 1.2,
          ease: 'power2.out',
          delay: 0.3,
        });
      }
    }
  }

  goToDashboard() {
    this.router.navigate(['/dashboard/workflows']);
  }
}
