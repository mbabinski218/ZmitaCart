import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferMainService } from '@components/offers-main/api/offers-main.service';

@Component({
  selector: 'pp-offers-main',
  standalone: true,
  imports: [CommonModule],
  providers: [OfferMainService],
  templateUrl: './offers-main.component.html',
  styleUrls: ['./offers-main.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OffersMainComponent implements OnInit{

  constructor(private offerMainService: OfferMainService) { }

  ngOnInit(): void {
    this.offerMainService.getOffers().subscribe()
  }
}
