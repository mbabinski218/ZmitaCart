import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddressFormComponent } from '@components/account/components/address-form/address-form.component';
import { FavouritesComponent } from '@components/account/components/favourites/favourites.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, filter, tap } from 'rxjs';
import { UserDataComponent } from '@components/account/components/user-data/user-data.component';
import { UserBoughtComponent } from '@components/account/components/user-bought/user-bought.component';
import { UserOffersComponent } from '@components/account/components/user-offers/user-offers.component';
import { USER_SWITCHES } from '@components/account/constants/user-switch.const';

@Component({
  selector: 'pp-account',
  standalone: true,
  imports: [CommonModule, AddressFormComponent, FavouritesComponent, MatButtonToggleModule, UserDataComponent, UserBoughtComponent, UserOffersComponent],
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class AccountComponent implements OnInit {
  currentView$ = new BehaviorSubject<string>('credentials');

  readonly USER_SWITCHES = USER_SWITCHES;

  switch(value: string) {
    this.currentView$.next(value);
    this.navigateTo(value);
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.route.fragment.pipe(
      filter((res) => !!res),
      tap((res) => this.currentView$.next(res)),
    ).subscribe();
  }

  navigateTo(fragment: string): void {
    void this.router.navigate(['.'], {
      relativeTo: this.route,
      fragment,
    });
  }
}
