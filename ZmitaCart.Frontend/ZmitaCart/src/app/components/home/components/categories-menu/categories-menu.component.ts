import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-categories-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './categories-menu.component.html',
  styleUrls: ['./categories-menu.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoriesMenuComponent {
  // categories = [
  //   {
  //     name: 'Elektronika',
  //     children: [
  //       {
  //         name: 'Fotografia',
  //         children: [
  //           {
  //             name: 'Akcesoria',
  //           },
  //           {
  //             name: 'Aparaty',
  //           },
  //           {
  //             name: 'Obiektywy',
  //           },
  //         ]
  //       },
  //       {
  //         name: 'Komputery',
  //         children: [
  //           {
  //             name: 'Części',
  //           },
  //           {
  //             name: 'Internet',
  //           },
  //           {
  //             name: 'Tablety',
  //           },
  //         ]
  //       },
  //     ]
  //   }
  // ];
}
