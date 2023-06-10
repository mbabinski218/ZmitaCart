import {ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ConditionType} from "@components/add-offer/interfaces/ConditionType";
import {Condition} from "@core/enums/condition.enum";

@Component({
  selector: 'pp-condition-wrapper',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './condition-wrapper.component.html',
  styleUrls: ['./condition-wrapper.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConditionWrapperComponent {
  @Input() items: ConditionType[];
  @Input() condition: Condition;
  @Output() componentClicked = new EventEmitter<Condition>();
  selectedIndex: number;

  selectItem(index: number) {
    this.selectedIndex = index;
    this.condition = this.items[index].condition;
    this.componentClicked.emit(this.condition);
  }

}
