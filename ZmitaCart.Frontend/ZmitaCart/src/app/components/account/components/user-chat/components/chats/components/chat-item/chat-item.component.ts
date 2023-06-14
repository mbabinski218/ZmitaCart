import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatsStream, MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { MessengerService } from '../../../messenger/services/messenger.service';
import { distinctUntilChanged, filter, take, tap, BehaviorSubject } from 'rxjs';
import { UserChatService } from '@components/account/components/user-chat/services/user-chat.service';

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
  @Input() set previousMessages(message: MessageStream[]) {
    // console.log(2,message)
    // if (message.length) {
    //   this.message$.next(message.find((res) => res.chatId === this.previousChat.id));
    // }
  }

  readonly imageUrl = 'http://localhost:5102/File?name=';

  message$ = new BehaviorSubject<MessageStream>(null);
  receivedMessages$ = new BehaviorSubject<MessageStream[]>([]);
}
