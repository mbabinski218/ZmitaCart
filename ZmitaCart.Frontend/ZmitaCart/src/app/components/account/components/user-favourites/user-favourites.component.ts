import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { AccountService } from '@components/account/api/account.service';
import { FavouriteOffers } from '@components/account/interfaces/account.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-user-favourites',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, OfferTileComponent, MatIconModule],
  providers: [AccountService],
  templateUrl: './user-favourites.component.html',
  styleUrls: ['./user-favourites.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class UserFavouritesComponent implements OnInit {

  favourites$: Observable<FavouriteOffers>;
  currentPage$ = new BehaviorSubject<number>(1);

  allPages = 1;
  currentPage = 1;
  canGo = {
    right: false,
    left: false,
  };

  constructor(
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.favourites$ = this.currentPage$.pipe(
      switchMap((res) => this.accountService.getFavourites(res)),
      tap((res) => {
        this.allPages = res.totalPages;
        this.canGo = { right: res.hasNextPage, left: res.hasPreviousPage };
      })
    );
  }

  nextPage(side: number): void {
    this.currentPage += side;
    this.applyPageChanged();
  }

  changeToPage(pageNumber: string): void {
    let number = Number(pageNumber);
    if (number > this.allPages)
      number = this.allPages;

    if (number < 1)
      number = 1;

    this.currentPage = number;
    this.applyPageChanged();
  }

  applyPageChanged(): void {
    this.currentPage$.next(this.currentPage);
  }
}
