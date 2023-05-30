import {ChangeDetectionStrategy, Component, Input, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';

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
  @Input() title: string;
  @Input() description: string;
  @Input() condition: string;
}
