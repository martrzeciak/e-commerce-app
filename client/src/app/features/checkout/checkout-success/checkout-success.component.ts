import { CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { Component, inject, OnDestroy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CardPipe } from '../../../shared/pipes/card.pipe';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { MatButton } from '@angular/material/button';
import { SignalrService } from '../../../core/services/signalr.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-checkout-success',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    CardPipe,
    CurrencyPipe,
    AddressPipe,
    MatButton,
    MatProgressSpinnerModule,
    NgIf
  ],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent implements OnDestroy {
 signalrService = inject(SignalrService);
  private orderService = inject(OrderService);

  ngOnDestroy(): void {
    this.orderService.orderComplete = false;
    this.signalrService.orderSignal.set(null);
  }
}
