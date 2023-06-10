import { Injectable } from '@angular/core';
import { UserService } from '@core/services/authorization/user.service';
import { HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';

@Injectable()
export class MessengerService {

  constructor(private userService: UserService) { }

  hubConnection: signalR.HubConnection;

  chatId: number;
  authorId: string;
  authorName: string;

  private messageStream$ = new BehaviorSubject<MessageStream>(null);

  buildConnection(chatId: number): void {
    if (!this.hubConnection) {
      this.authorId = this.userService.userAuthorization().id;
      this.authorName = this.userService.userAuthorization().firstName;
      this.hubConnection = new HubConnectionBuilder().configureLogging(LogLevel.None).withUrl('http://localhost:5102/ChatHub').build();
    }

    if (this.hubConnection.state === HubConnectionState.Disconnected || this.hubConnection.state === HubConnectionState.Disconnecting) {
      this.startConnection(chatId);
    } else {
      this.hubConnection.stop()
        .finally(() => this.startConnection(chatId));
    }
  }

  sendMessage(message: string) {
    this.hubConnection.invoke("SendMessage", this.chatId, this.authorId, this.authorName, message)
      .catch((err) => console.log(err));
  }

  getMessageStream(): Observable<MessageStream> {
    return this.messageStream$.asObservable();
  }

  private startConnection(chatId: number) {
    this.chatId = chatId;

    this.hubConnection.start()
      .then(() => {
        this.hubConnection.invoke("Join", this.chatId, this.authorId)
          .catch((err) => console.log(err));
      })
      .catch((err) => console.log(err));

    this.setReceiver();
  }

  private setReceiver(): void {
    this.hubConnection.on("ReceiveMessage", (authorId: number, authorName: string, date: Date, content: string) => {
      this.messageStream$.next({ authorId, authorName, date, content });
    });
  }
}