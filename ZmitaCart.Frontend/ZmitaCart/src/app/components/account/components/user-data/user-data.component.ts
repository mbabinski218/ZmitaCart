import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { AccountService } from '@components/account/api/account.service';
import { UserCredentials } from '@components/account/interfaces/account.interface';

@Component({
  selector: 'pp-user-data',
  standalone: true,
  imports: [CommonModule],
  providers: [AccountService],
  templateUrl: './user-data.component.html',
  styleUrls: ['./user-data.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserDataComponent implements OnInit {

  userData$: Observable<UserCredentials>;

  constructor(
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.userData$ = this.accountService.getUserData();


    this.accountService.getUserData().subscribe((res) => console.log(res));
  }

}
