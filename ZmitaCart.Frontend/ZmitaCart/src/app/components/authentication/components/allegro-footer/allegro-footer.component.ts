import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'pp-allegro-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './allegro-footer.component.html',
  styleUrls: ['./allegro-footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AllegroFooterComponent {

}
