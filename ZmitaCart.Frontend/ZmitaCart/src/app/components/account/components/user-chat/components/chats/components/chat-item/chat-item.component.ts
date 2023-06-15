import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatsStream, MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';

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

  readonly imageUrl = 'http://localhost:5102/File?name=';

}
