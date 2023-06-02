import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubCategories, SuperiorCategories } from '@components/home/components/header/interfaces/header.interface';
import { MatIconModule } from '@angular/material/icon';
import { HeaderService } from '@components/home/components/api/header.service';
import { Observable, tap } from 'rxjs';
import { SubcategoriesMenuComponent } from '@components/home/components/header/components/categories-menu/subcategories-menu/subcategories-menu.component';

@Component({
  selector: 'pp-categories-menu',
  standalone: true,
  imports: [CommonModule, MatIconModule, SubcategoriesMenuComponent],
  providers: [HeaderService],
  templateUrl: './categories-menu.component.html',
  styleUrls: ['./categories-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoriesMenuComponent {

  @Input() categories: SuperiorCategories[];

  subcategoriesLoaded: SubCategories[];

  subcategories$: Observable<SubCategories>;

  constructor(
    private headerService: HeaderService,
  ) { }

  openSubCategories(category: SuperiorCategories): void {
    this.subcategories$ = this.headerService.getSubCategories(category.id).pipe(
      tap((res) => {
        if (!(res.children && res.children.length > 0)) {
          this.categories.find((res) => res === category).isClickable = true;
        }
      })
    );
  }

  openCategory(categoryName: string): void {
    console.log(categoryName);
  }
}
