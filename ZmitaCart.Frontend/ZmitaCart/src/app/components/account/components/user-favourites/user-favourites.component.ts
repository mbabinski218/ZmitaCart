import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-user-favourites',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-favourites.component.html',
  styleUrls: ['./user-favourites.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserFavouritesComponent {


}
