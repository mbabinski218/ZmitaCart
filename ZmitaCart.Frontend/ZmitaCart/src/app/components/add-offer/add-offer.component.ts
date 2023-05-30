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

@Component({
  selector: 'pp-add-offer',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, ReactiveFormsModule, MatInputModule, SingleCategoryComponent, ConditionWrapperComponent],
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
  superiorCategories$: Observable<SuperiorCategory[]>;
  subCategories$: Category[] = [];

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

  public setCondition(con: Condition) {
    this.condition = con;
    console.log(Condition[con]);
  }

  protected readonly Condition = Condition;
}

