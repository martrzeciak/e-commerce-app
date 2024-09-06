import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CardPipe } from '../../../shared/pipes/card.pipe';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-checkout-success',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    CardPipe,
    CurrencyPipe,
    AddressPipe,
    MatButton
  ],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent {

}
