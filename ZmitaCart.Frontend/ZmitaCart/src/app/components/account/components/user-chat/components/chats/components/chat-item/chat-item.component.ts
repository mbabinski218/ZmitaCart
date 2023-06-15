import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatsStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { IMAGE_URL } from '@shared/constants/shared.constants';

@Component({
  selector: 'pp-chat-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-item.component.html',
  styleUrls: ['./chat-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatItemComponent {

  @Input() previousChat: ChatsStream;

  readonly imageUrl = IMAGE_URL;
}
