import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { ChatsStream } from '../../interfaces/chat.interfaces';
import { IMAGE_URL } from '@shared/constants/shared.constants';

@Component({
  selector: 'pp-offer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './offer.component.html',
  styleUrls: ['./offer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferComponent {

  @Input() currentChat: ChatsStream;

  readonly imageUrl = IMAGE_URL;

  constructor(
    private router: Router,
  ) { }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
