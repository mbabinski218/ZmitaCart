import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-offer-seller',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './offer-seller.component.html',
  styleUrls: ['./offer-seller.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferSellerComponent {
  @Input() details: SingleOffer;
}
