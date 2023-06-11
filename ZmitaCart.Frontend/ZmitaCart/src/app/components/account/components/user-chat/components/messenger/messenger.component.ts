import { ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MessengerService } from './services/messenger.service';
import { AccountService } from '@components/account/api/account.service';
import { SingleChat } from '@components/account/interfaces/account.interface';
import { BehaviorSubject, Observable, filter, tap, Subject, takeUntil, map } from 'rxjs';
import { MessageStream } from '../../interfaces/chat.interfaces';
import { MessagesComponent } from './components/messages/messages.component';

@Component({
  selector: 'pp-messenger',
  standalone: true,
  imports: [CommonModule, MatIconModule, MessagesComponent],
  providers: [MessengerService],
  templateUrl: './messenger.component.html',
  styleUrls: ['./messenger.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessengerComponent implements OnInit, OnDestroy {

  @Input() set currentChat(chat: SingleChat) {
    if (chat) {
      this.allMessages$.next([]);
      this.messengerService.buildConnection(chat.id);
      this.isChat = !!chat;
    }
  }

  @Input() set offerId(id: string) {
    if (id)
      this.accountService.createNewChat(id).subscribe((res) => {
        this.messengerService.buildConnection(res);
      });
  }

  @ViewChild('myTextarea', { static: false }) myTextarea: ElementRef<HTMLTextAreaElement>;

  allMessages$ = new BehaviorSubject<MessageStream[]>([]);
  onDestroy$ = new Subject<void>();
  isChat: boolean;

  constructor(
    private messengerService: MessengerService,
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.messengerService.getMessageStream().pipe(
      filter((res) => !!res),
      map((res) => ({
        ...res,
        date: new Date(res.date),
      })),
      takeUntil(this.onDestroy$),
    ).subscribe((res) => {
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
      this.messengerService.sendMessage(textareaValue);
      this.myTextarea.nativeElement.value = '';
    }
  }
}
