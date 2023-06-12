import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferMainService } from '@components/offers-main/api/offers-main.service';
import { OfferItem } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { filter, tap } from 'rxjs';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-favourite-buy',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  providers: [OfferMainService],
  templateUrl: './favourite-buy.component.html',
  styleUrls: ['./favourite-buy.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FavouriteBuyComponent {

  @Input() item: OfferItem;

  constructor(
    private offerMainService: OfferMainService,
    private ref: ChangeDetectorRef,
    private router: Router,
  ) { }

  observe(item: OfferItem): void {
    this.offerMainService.addToFavourites(item.id).pipe(
      filter((res) => !!res),
      tap(() => item.isFavourite = !item.isFavourite),
      tap(() => this.ref.detectChanges()),
    ).subscribe();
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
