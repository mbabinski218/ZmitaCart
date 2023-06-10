import { ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MessengerService } from './services/messenger.service';
import { AccountService } from '@components/account/api/account.service';
import { SingleChat } from '@components/account/interfaces/account.interface';
import { Observable, filter, tap } from 'rxjs';
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
export class MessengerComponent implements OnInit {

  @Input() set currentChat(chat: SingleChat) {
    if (chat)
      this.messengerService.buildConnection(chat.id);
  }

  @Input() set offerId(id: string) {
    if (id)
      this.accountService.createNewChat(id).subscribe((res) => {
        this.messengerService.buildConnection(res);
      });
  }

  @ViewChild('myTextarea', { static: false }) myTextarea: ElementRef<HTMLTextAreaElement>;

  messages$: Observable<MessageStream>;

  constructor(
    private messengerService: MessengerService,
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.messages$ = this.messengerService.getMessageStream().pipe(
      filter((res) => !!res),
      tap((res) => console.log(res))
    );
  }


  sendMessage() {
    const textareaValue = this.myTextarea.nativeElement.value;
    this.messengerService.sendMessage(textareaValue);
    this.myTextarea.nativeElement.value = '';
  }
}
