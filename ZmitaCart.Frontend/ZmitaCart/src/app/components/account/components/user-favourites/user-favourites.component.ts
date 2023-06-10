import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, switchMap, tap } from 'rxjs';
import { AccountService } from '@components/account/api/account.service';
import { AccountOffers } from '@components/account/interfaces/account.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';
import { MatIconModule } from '@angular/material/icon';
import { PaginationService } from '@shared/services/pagination.service';

@Component({
  selector: 'pp-user-favourites',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, OfferTileComponent, MatIconModule],
  providers: [AccountService, PaginationService],
  templateUrl: './user-favourites.component.html',
  styleUrls: ['./user-favourites.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class UserFavouritesComponent implements OnInit {

  favourites$: Observable<AccountOffers>;

  totalPages = 1;
  currentPage = 1;
  areAnyOffers = false;
  canGo = {
    right: false,
    left: false,
  };

  constructor(
    private accountService: AccountService,
    protected paginationService: PaginationService,
  ) { }

  ngOnInit(): void {
    this.favourites$ = this.paginationService.getCurrentPage().pipe(
      switchMap((res) => this.accountService.getFavourites(res)),
      tap((res) => this.paginationService.setTotalPages(res.totalPages)),
      tap((res) => this.setAll(res)),
    );
  }

  private setAll(data: AccountOffers): void {
    this.totalPages = data.totalPages;
    this.currentPage = data.pageNumber;
    this.areAnyOffers = !!data.totalCount;
    this.canGo = { right: data.hasNextPage, left: data.hasPreviousPage };
  }
}
