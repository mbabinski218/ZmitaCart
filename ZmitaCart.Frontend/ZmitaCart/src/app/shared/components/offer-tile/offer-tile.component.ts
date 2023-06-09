import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FavouriteItem } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { OfferMainService } from '@components/offers-main/api/offers-main.service';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-offer-tile',
  standalone: true,
  imports: [CommonModule, MatIconModule, ppFixPricePipe],
  providers: [OfferMainService],
  templateUrl: './offer-tile.component.html',
  styleUrls: ['./offer-tile.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferTileComponent {

  @Input() items: FavouriteItem[];
  @Input() origin: 'favourites' | 'bought' | 'filtered' | 'users';

  readonly imageUrl = 'http://localhost:5102/File?name=';

  constructor(
    private offerMainService: OfferMainService,
    private router: Router,
  ) { }

  observe(item: FavouriteItem): void {
    this.offerMainService.addToFavourites(item.id).pipe(
    ).subscribe();

    item.isFavourite = !item.isFavourite;
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
