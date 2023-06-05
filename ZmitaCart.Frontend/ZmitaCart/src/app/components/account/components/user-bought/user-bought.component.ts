import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-user-bought',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-bought.component.html',
  styleUrls: ['./user-bought.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserBoughtComponent {

}
