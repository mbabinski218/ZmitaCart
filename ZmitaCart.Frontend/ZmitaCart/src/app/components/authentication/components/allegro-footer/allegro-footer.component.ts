import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'pp-allegro-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './allegro-footer.component.html',
  styleUrls: ['./allegro-footer.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AllegroFooterComponent {

}
