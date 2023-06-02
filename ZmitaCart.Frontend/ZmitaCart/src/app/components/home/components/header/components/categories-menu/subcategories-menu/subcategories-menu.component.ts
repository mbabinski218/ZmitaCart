import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubCategories } from '@components/home/components/header/interfaces/header.interface';

@Component({
  selector: 'pp-subcategories-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subcategories-menu.component.html',
  styleUrls: ['./subcategories-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SubcategoriesMenuComponent {
  @Input() subcategories: SubCategories[];

  openCategory(categoryName: string): void {
    console.log(categoryName);
  }
}
