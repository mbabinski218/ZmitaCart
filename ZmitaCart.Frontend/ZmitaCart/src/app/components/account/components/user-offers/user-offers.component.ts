import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '@components/account/api/account.service';
import { MatIconModule } from '@angular/material/icon';
import { Observable, switchMap, tap } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AccountOffers } from '@components/account/interfaces/account.interface';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';
import { PaginationService } from '@shared/services/pagination.service';

@Component({
  selector: 'pp-user-offers',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule, OfferTileComponent],
  providers: [AccountService, PaginationService],
  templateUrl: './user-offers.component.html',
  styleUrls: ['./user-offers.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class UserOffersComponent implements OnInit {

  myOffers$: Observable<AccountOffers>;

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
    this.myOffers$ = this.paginationService.getCurrentPage().pipe(
      switchMap((res) => this.accountService.getUserOffers(res)),
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
