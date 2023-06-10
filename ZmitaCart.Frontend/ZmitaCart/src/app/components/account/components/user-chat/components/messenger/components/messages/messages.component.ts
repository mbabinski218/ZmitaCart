import { MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '@core/services/authorization/user.service';

@Component({
  selector: 'pp-messages',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessagesComponent {
  @Input() set messages(message: MessageStream) {
    if (message) {
      this.message = message;

      this.isFromUser = String(message.authorId) === this.userService.userAuthorization().id;

      // console.log(message);
      // console.log(this.isFromUser);
    }
  }

  constructor(private userService: UserService) { }

  message: MessageStream;
  isFromUser = false;

}
