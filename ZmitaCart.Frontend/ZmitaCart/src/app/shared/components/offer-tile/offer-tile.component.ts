import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoughtOffer } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { FavouriteBuyComponent } from './components/favourite-buy/favourite-buy.component';
import { EditDeleteComponent } from './components/edit-delete/edit-delete.component';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';
import { Observable, Subject, filter, takeUntil, tap } from 'rxjs';
import { ToastMessageService } from '../toast-message/services/toast-message.service';
import { BoughtInfoComponent } from './components/bought-info/bought-info.component';

@Component({
  selector: 'pp-offer-tile',
  standalone: true,
  imports: [CommonModule, MatIconModule, ppFixPricePipe, FavouriteBuyComponent, EditDeleteComponent, BoughtInfoComponent],
  providers: [OfferSingleService],
  templateUrl: './offer-tile.component.html',
  styleUrls: ['./offer-tile.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OfferTileComponent implements OnDestroy {

  @Input() items: BoughtOffer[];
  @Input() origin: 'favourites' | 'bought' | 'filtered' | 'user-offers';

  readonly imageUrl = 'http://localhost:5102/File?name=';
  private onDestroy$ = new Subject<void>();

  siema: Observable<boolean>;

  constructor(
    private router: Router,
    private offerSingleService: OfferSingleService,
    private toastMessageService: ToastMessageService,
    private ref: ChangeDetectorRef,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }

  deleteOffer(item: BoughtOffer) {
    this.offerSingleService.deleteOffer(String(item.offer.id)).pipe(
      filter((res) => !!res),
      tap(() => this.items = this.items.filter((res) => res !== item)),
      tap(() => this.toastMessageService.notifyOfSuccess('Usunięto ofertę')),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }
}
