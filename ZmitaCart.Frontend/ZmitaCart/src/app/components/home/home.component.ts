import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from '@components/home/components/layout/layout.component';

@Component({
  selector: 'pp-home',
  standalone: true,
  imports: [CommonModule, RouterModule, LayoutComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {

}
