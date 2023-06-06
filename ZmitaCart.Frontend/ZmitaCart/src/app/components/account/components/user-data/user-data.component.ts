import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, map } from 'rxjs';
import { AccountService } from '@components/account/api/account.service';
import { UserCredentialsShow } from '@components/account/interfaces/account.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DetailTileComponent } from '@shared/components/detail-tile/detail-tile.component';

@Component({
  selector: 'pp-user-data',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, DetailTileComponent],
  providers: [AccountService],
  templateUrl: './user-data.component.html',
  styleUrls: ['./user-data.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserDataComponent implements OnInit {

  userData$: Observable<UserCredentialsShow[]>;

  constructor(
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.userData$ = this.accountService.getUserData().pipe(
      map((res) => ([
        {
          name: 'Imię',
          value: res.firstName,
          icon: 'badge',
        },
        {
          name: 'Nazwisko',
          value: res.lastName,
          icon: 'person',
        },
        {
          name: 'Email',
          value: res.email,
          icon: 'mail',
        },
        {
          name: 'Numer telefonu',
          value: this.applyMask('Numer telefonu', res.phoneNumber),
          icon: 'dialpad',
        },
        {
          name: 'Państwo',
          value: res.address.country,
          icon: 'flag',
        },
        {
          name: 'Miasto',
          value: res.address.city,
          icon: 'domain',
        },
        {
          name: 'Ulica',
          value: res.address.street,
          icon: 'add_road',
        },
        {
          name: 'Kod pocztowy',
          value: this.applyMask('Kod pocztowy', String(res.address.postalCode)),
          icon: 'markunread_mailbox',
        },
        {
          name: 'Numer domu',
          value: res.address.houseNumber,
          icon: 'house',
        },
        {
          name: 'Numer mieszkania',
          value: res.address.apartmentNumber,
          icon: 'apartment',
        },
      ]))
    );
  }

  applyMask(type:string, value: string): string {
    switch(type) {
      case 'Numer telefonu': {
        return `${value.slice(0, 3)}-${value.slice(3, 6)}-${value.slice(6)}`;
      }
      case 'Kod pocztowy': {
        return `${value.slice(0, 2)}-${value.slice(2)}`;
      }
    }
  }
}
