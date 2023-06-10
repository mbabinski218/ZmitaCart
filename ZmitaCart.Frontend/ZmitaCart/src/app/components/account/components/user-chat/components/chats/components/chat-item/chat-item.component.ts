import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SingleChat } from '@components/account/interfaces/account.interface';

@Component({
  selector: 'pp-chat-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-item.component.html',
  styleUrls: ['./chat-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatItemComponent {
  @Input() currentChat: SingleChat;

  readonly imageUrl = 'http://localhost:5102/File?name=';
}
