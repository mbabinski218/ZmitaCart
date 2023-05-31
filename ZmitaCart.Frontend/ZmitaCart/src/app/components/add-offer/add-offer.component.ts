import {ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {SingleCategoryComponent} from "@components/add-offer/single-category/single-category.component";
import {SuperiorCategory} from "@components/add-offer/interfaces/SuperiorCategory";
import {CategoryService} from "@components/add-offer/api/category.service";
import {Observable} from "rxjs";
import {Category} from "@components/add-offer/interfaces/Category";
import {Condition} from "@core/enums/condition.enum";
import {ConditionWrapperComponent} from "@components/add-offer/condition-wrapper/condition-wrapper.component";
import {ToggleTestComponent} from "@components/add-offer/toggle-test/toggle-test.component";
import {ConditionType} from "@components/add-offer/interfaces/ConditionType";

@Component({
  selector: 'pp-add-offer',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, ReactiveFormsModule, MatInputModule, SingleCategoryComponent, ConditionWrapperComponent, ToggleTestComponent],
  templateUrl: './add-offer.component.html',
  styleUrls: ['./add-offer.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddOfferComponent implements OnInit {
  createOffer: FormGroup;
  characterCount: number;
  price: number;
  quantity: number;
  condition: Condition;
  // condition: string;
  superiorCategories$: Observable<SuperiorCategory[]>;
  subCategories$: Category[] = [];
  items: ConditionType[] = [{
    title: "Używany",
    description: "Widoczne ślady używania lub uszkodzenia, które zostały uwzględnione w opisie tego przedmiotu i/lub na zdjęciach.",
    condition: Condition.Used
  }, {
    title: "Bardzo dobry",
    description: "W bardzo dobrym stanie technicznym i wizualnym. Nie nosi śladów używania lub są one znikome.",
    condition: Condition.Good,
  }, {
    title: "Nowy",
    description: "Fabrycznie nowy, nieużywany, nieuszkodzony, zapakowany w oryginalne opakowanie od producenta.",
    condition: Condition.New
  }];

  constructor(private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    this.createOffer = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
      description: new FormControl(null as string, []),
      price: new FormControl(null as number, Validators.required),
      quantity: new FormControl(1)
    });

    this.createOffer.get('title').valueChanges.subscribe((res: string) => {
      this.characterCount = res.length;
    });

    this.superiorCategories$ = this.categoryService.getAllSupCategories();
  }

  public getFewBySuperiorId(supId: number, childrenCount: number) {
    this.categoryService.getFewBySuperiorId(supId, childrenCount).subscribe(res => {
      this.subCategories$ = res;
      console.log(res);
    });
  }

  public SetCondition(event: Condition) {
    console.log(event);
    this.condition = event;
    console.log(Condition[this.condition]);
  }

  protected readonly Condition = Condition;
}

