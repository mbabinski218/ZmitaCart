import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-user-offers',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-offers.component.html',
  styleUrls: ['./user-offers.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserOffersComponent {

}
