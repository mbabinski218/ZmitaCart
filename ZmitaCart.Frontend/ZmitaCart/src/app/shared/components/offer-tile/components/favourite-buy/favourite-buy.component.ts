import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferItem } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { Subject, filter, takeUntil, tap } from 'rxjs';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { SharedService } from '@shared/services/shared.service';

@Component({
  selector: 'pp-favourite-buy',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './favourite-buy.component.html',
  styleUrls: ['./favourite-buy.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FavouriteBuyComponent implements OnDestroy {

  @Input() item: OfferItem;

  private onDestroy$ = new Subject<void>();

  constructor(
    private sharedService: SharedService,
    private ref: ChangeDetectorRef,
    private router: Router,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  observe(item: OfferItem): void {
    this.sharedService.addToFavourites(item.id, item.isFavourite).pipe(
      filter((res) => !!res),
      tap(() => item.isFavourite = !item.isFavourite),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
