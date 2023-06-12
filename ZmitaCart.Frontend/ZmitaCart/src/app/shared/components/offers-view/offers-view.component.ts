import { ChangeDetectionStrategy, Component, ViewEncapsulation, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, map, switchMap, tap } from 'rxjs';
import { BoughtOffers } from '@components/account/interfaces/account.interface';
import { AccountService } from '@components/account/api/account.service';
import { PaginationService } from '@shared/services/pagination.service';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';

@Component({
  selector: 'pp-offers-view',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule, OfferTileComponent],
  providers: [AccountService, PaginationService],
  templateUrl: './offers-view.component.html',
  styleUrls: ['./offers-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class OffersViewComponent implements OnInit {

  @Input() title: string;
  @Input() noDataText: string;
  @Input() origin: 'favourites' | 'bought' | 'filtered' | 'user-offers';

  offers$: Observable<BoughtOffers>;

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
    this.offers$ = this.setData();
  }

  private setData(): Observable<BoughtOffers> {
    if (this.origin === 'bought') {
      return this.paginationService.getCurrentPage().pipe(
        switchMap((res) => this.accountService.getBought(res)),
        tap((res) => this.paginationService.setTotalPages(res.totalPages)),
        tap((res) => this.setAll(res)),
      );
    }

    return this.paginationService.getCurrentPage().pipe(
      switchMap((res) => {
        switch (this.origin) {
          case "favourites": {
            return this.accountService.getFavourites(res);
          }
          case "filtered": {
            return;
          }
          case "user-offers": {
            return this.accountService.getUserOffers(res);
          }
        }
      }),
      map((res) => {
        return {
          ...res,
          items: res.items.map(item => ({
            offer: {
              ...item,
            },
          })),
        };
      }),
      tap((res) => this.paginationService.setTotalPages(res.totalPages)),
      tap((res) => this.setAll(res)),
    );
  }

  private setAll(data: BoughtOffers): void {
    this.totalPages = data.totalPages;
    this.currentPage = data.pageNumber;
    this.areAnyOffers = !!data.totalCount;
    this.canGo = { right: data.hasNextPage, left: data.hasPreviousPage };
  }
}
