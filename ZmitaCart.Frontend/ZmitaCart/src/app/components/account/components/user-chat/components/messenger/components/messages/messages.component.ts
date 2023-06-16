import { ppMessengerDatePipe } from '@shared/pipes/messenger-date.pipe';
import { MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ppFixAuthorDisplayPipe } from '@shared/pipes/fix-author-display.pipe';

@Component({
  selector: 'pp-messages',
  standalone: true,
  imports: [CommonModule, ppMessengerDatePipe, ppFixAuthorDisplayPipe],
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessagesComponent {
  @Input() allMessages: MessageStream[];
}
