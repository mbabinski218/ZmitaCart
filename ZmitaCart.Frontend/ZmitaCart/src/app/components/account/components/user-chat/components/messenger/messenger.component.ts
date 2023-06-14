import { ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild, OnDestroy, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MessengerService } from './services/messenger.service';
import { AccountService } from '@components/account/api/account.service';
import { BehaviorSubject, filter, Subject, takeUntil, map, tap, Observable } from 'rxjs';
import { ChatsStream, MessageStream } from '../../interfaces/chat.interfaces';
import { MessagesComponent } from './components/messages/messages.component';
import { UserChatService } from '../../services/user-chat.service';

@Component({
  selector: 'pp-messenger',
  standalone: true,
  imports: [CommonModule, MatIconModule, MessagesComponent],
  templateUrl: './messenger.component.html',
  styleUrls: ['./messenger.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessengerComponent implements OnInit, OnDestroy {

  @Input() set offerId(id: string) {
    if (id)
      this.accountService.createNewChat(id).subscribe((res) => {
        this.messengerService.restoreMessages(res);
      });
  }

  @ViewChild('myTextarea', { static: false }) myTextarea: ElementRef<HTMLTextAreaElement>;

  allMessages$ = new BehaviorSubject<MessageStream[]>([]);
  onDestroy$ = new Subject<void>();
  currentChat$ = new BehaviorSubject<ChatsStream>(null);

  constructor(
    private messengerService: MessengerService,
    private accountService: AccountService,
    private userChatService: UserChatService,
  ) { }

  ngOnInit(): void {
    this.userChatService.getCurrentChat().pipe(
      filter((res) => !!res),
      tap((res) => this.currentChat$.next(res)),
      tap(() => this.allMessages$.next([])),
      tap((res) => this.messengerService.restoreMessages(res.id)),
    ).subscribe();

    this.userChatService.getMessageStream().pipe(
      filter(() => !!this.currentChat$.value),
      filter((res) => !!res),
      map((res) => ({
        ...res,
        date: new Date(res.date),
      })),
      takeUntil(this.onDestroy$),
    ).subscribe((res) => {
      if (res.chatId === this.currentChat$.value.id)
        this.allMessages$.next([
          ...this.allMessages$.value,
          res,
        ]);
    });
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  sendMessage() {
    const textareaValue = this.myTextarea.nativeElement.value;
    if (textareaValue) {
      this.messengerService.sendMessage(textareaValue, this.currentChat$.value.id);
      this.myTextarea.nativeElement.value = '';
    }
  }
}
