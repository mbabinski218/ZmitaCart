import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SingleChat } from '@components/account/interfaces/account.interface';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-offer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './offer.component.html',
  styleUrls: ['./offer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferComponent {

  @Input() currentChat: SingleChat;

  readonly imageUrl = 'http://localhost:5102/File?name=';

  constructor(
    private router: Router,
  ) { }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
