import {ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {Category} from "@components/add-offer/interfaces/Category";

@Component({
  selector: 'pp-test',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TestComponent {
  @Input() item: any;
  @Input() headName: any;
  @Input() children: Category[];
  @Input() categoryIcon?: string;
  @Input() categoryId: number;
  @Output() componentClicked = new EventEmitter<number>();

  onClick(id: number) {
    this.componentClicked.emit(id);
  }
}
