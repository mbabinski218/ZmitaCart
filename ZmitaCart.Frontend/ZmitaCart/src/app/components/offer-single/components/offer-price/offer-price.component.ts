import { Router } from '@angular/router';
import { UserService } from '@core/services/authorization/user.service';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { OfferMainService } from '@components/offers-main/api/offers-main.service';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';
import { MatButtonModule } from '@angular/material/button';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { RoutingService } from '@shared/services/routing.service';

@Component({
  selector: 'pp-offer-price',
  standalone: true,
  imports: [CommonModule, MatIconModule, ppFixPricePipe, MatButtonModule, MatIconModule],
  providers: [OfferMainService],
  templateUrl: './offer-price.component.html',
  styleUrls: ['./offer-price.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferPriceComponent {

  @Input() details: SingleOffer;

  currentQuantity = 1;

  constructor(
    private offerMainService: OfferMainService,
    private userService: UserService,
    private router: Router,
    private routingService: RoutingService,
  ) { }

  observe(): void {
    if (!this.userService.isAuthenticated())
      return void this.router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);

    this.offerMainService.addToFavourites(this.details.id).pipe(
    ).subscribe();

    this.details.isFavourite = !this.details.isFavourite;
  }

  add(val: number) {
    this.currentQuantity += val;
  }

  navigateTo(fragment?: string): void {
    this.routingService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.ACCOUNT}`, fragment, { name: 'id', value: this.details.id});
  }
}