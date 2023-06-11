import { Pipe, PipeTransform } from '@angular/core';
import { MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';

@Pipe({
  name: 'ppFixAuthorDisplay',
  standalone: true,
})
export class ppFixAuthorDisplayPipe implements PipeTransform {

  transform(author: string, data: { allMessages: MessageStream[], currentIndex: number }): string {

    if (data.currentIndex - 1 < 0)
      return author;

    if (author === data.allMessages[data.currentIndex - 1].authorName)
      return null;

    return author;
  }
}
