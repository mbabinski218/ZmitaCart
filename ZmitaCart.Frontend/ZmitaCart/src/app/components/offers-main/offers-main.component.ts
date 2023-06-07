import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferMainService } from '@components/offers-main/api/offers-main.service';
import { Observable, filter, map, switchMap, tap } from 'rxjs';
import { MainOffers, Offers } from '@components/offers-main/interfaces/offers-main.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CarouselComponent } from '@shared/components/carousel/carousel.component';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { UserService } from '@core/services/authorization/user.service';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-offers-main',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, CarouselComponent, MatIconModule, ppFixPricePipe],
  providers: [OfferMainService],
  templateUrl: './offers-main.component.html',
  styleUrls: ['./offers-main.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OffersMainComponent implements OnInit {

  offers$: Observable<Offers[]>;
  readonly imageUrl = 'http://localhost:5102/File?name=';

  constructor(
    private offerMainService: OfferMainService,
    private userService: UserService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.offers$ = this.offerMainService.getMostPopularCategories().pipe(
      switchMap((res) => this.offerMainService.getOffers(res)),
    );
  }

  observe(item: MainOffers): void {
    if (!this.userService.isAuthenticated())
      return void this.router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);

    this.offerMainService.addToFavourites(item.id).pipe(
      // filter((res) => !!res),
      // tap(() => item.isFavourite = !item.isFavourite),
    ).subscribe();

    item.isFavourite = !item.isFavourite;
    //TODO czemu to nie działa w tapie lub subscribie??
  }
}