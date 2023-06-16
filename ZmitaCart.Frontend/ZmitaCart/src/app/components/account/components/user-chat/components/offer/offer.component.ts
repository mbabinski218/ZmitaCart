import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ChatsStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { IMAGE_URL } from '@shared/constants/shared.constants';
import { goToDetails } from '@shared/utils/offer-details';

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
  details = goToDetails;

  constructor(
    protected router: Router,
  ) { }
}
