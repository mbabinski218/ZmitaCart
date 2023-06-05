import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '@components/account/api/account.service';

@Component({
  selector: 'pp-offers',
  standalone: true,
  imports: [CommonModule],
  providers: [AccountService],//TODO do wywalenia
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OffersComponent implements OnInit {

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    
  }

  pobierz() {
    this.accountService.getUserOffers().subscribe((res) => console.log(res))
    // this.accountService.getUserData().subscribe((res) => console.log(res))
  }
}
