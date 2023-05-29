import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-single-category',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './single-category.component.html',
  styleUrls: ['./single-category.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SingleCategoryComponent {

}
