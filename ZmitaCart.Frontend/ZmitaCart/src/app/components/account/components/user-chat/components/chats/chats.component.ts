import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chats, SingleChat } from '@components/account/interfaces/account.interface';
import { Observable, map, of, tap } from 'rxjs';
import { AccountService } from '@components/account/api/account.service';
import { ChatItemComponent } from './components/chat-item/chat-item.component';

@Component({
  selector: 'pp-chats',
  standalone: true,
  imports: [CommonModule, ChatItemComponent],
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatsComponent implements OnInit {

  @Input() currentChat: SingleChat;

  @Output() newCurrentChat = new EventEmitter<SingleChat>();

  previousChats$: Observable<Chats>;
  chat: SingleChat;

  readonly imageUrl = 'http://localhost:5102/File?name=';

  constructor(
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.previousChats$ = this.accountService.getAllChats().pipe(
      map((res) => {
        if (!this.currentChat)
          return res;

        return this.filterChats(this.currentChat, res);
      })
    );
  }

  changeCurrentChat(currentChat: SingleChat, allChats: Chats): void {
    this.previousChats$ = of(this.filterChats(currentChat, allChats));

    this.newCurrentChat.emit(currentChat);
  }

  private filterChats(currentChat: SingleChat, allChats: Chats): Chats {
    return {
      ...allChats,
      items: allChats.items.map((res) => {
        if (res.offerId === currentChat.offerId)
          return {
            ...res,
            hidden: true,
          };

        return {
          ...res,
          hidden: false,
        };
      })
    };
  }
}
