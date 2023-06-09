import {ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {CategoryService} from "@components/add-offer/api/category.service";
import {Category} from "@components/add-offer/interfaces/Category";
import {Condition} from "@core/enums/condition.enum";
import {ConditionWrapperComponent} from "@components/add-offer/condition-wrapper/condition-wrapper.component";
import {ConditionType} from "@components/add-offer/interfaces/ConditionType";
import {CategorySelectorComponent} from "@components/add-offer/category-selector/category-selector.component";
import {OfferService} from "@components/add-offer/api/offer.service";

@Component({
  selector: 'pp-add-offer',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, ReactiveFormsModule, MatInputModule, ConditionWrapperComponent, CategorySelectorComponent],
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
  subCategories: Category[];
  children: Category[];
  pickedCategory: Category = null;
  headCategory: Category;
  parentCategory?: Category;
  isInSubCategories: boolean;
  loadCategories: boolean;
  selectedImages: File[];
  previews: string[] = [];

  items: ConditionType[] = [{
    title: "Używany",
    description: "Widoczne ślady używania lub uszkodzenia, które zostały uwzględnione w opisie tego przedmiotu i/lub na zdjęciach.",
    condition: Condition.Używany
  }, {
    title: "Bardzo dobry",
    description: "W bardzo dobrym stanie technicznym i wizualnym. Nie nosi śladów używania lub są one znikome.",
    condition: Condition.Dobry,
  }, {
    title: "Nowy",
    description: "Fabrycznie nowy, nieużywany, nieuszkodzony, zapakowany w oryginalne opakowanie od producenta.",
    condition: Condition.Nowy
  }];

  constructor(private categoryService: CategoryService, private cdr: ChangeDetectorRef, private offerService: OfferService) {
  }

  ngOnInit(): void {
    this.createOffer = new FormGroup({
      title: new FormControl(null as string, [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
      description: new FormControl(null as string, []),
      price: new FormControl(null as number, Validators.required),
      quantity: new FormControl(1),

    });

    this.createOffer.get('title').valueChanges.subscribe((res: string) => {
      this.characterCount = res.length;
    });

    //TODO zrobic żeby ładowały sie po kliknięciu a nie w inicie
    this.categoryService.getSuperiorsWithChildren(1)
      .subscribe(res => {
        this.subCategories = res;
        this.children = this.subCategories;
      });

  }

  public SetCondition(event: Condition) {
    this.condition = event;
  }

  public getParentCategory(parentId: number) {
    //patrzymy na kategorie nadrzędne
    console.log("pobieranie dzieci");
    if (parentId === null) {
      this.parentCategory = null;
      return;
    }

    return this.categoryService.getParentCategory(parentId).subscribe(
      res => {
        this.parentCategory = res;
      }
    );
  }

  goForward(event: Category) {
    this.isInSubCategories = true;
    if (event.parentId !== null && event.children)
      this.getParentCategory(event.parentId);

    this.categoryService.getFewBySuperiorId(event.id, 2).subscribe(res => {
      if (res[0].children !== null) {
        console.log("sprawdzam czy ma dzieci");
        this.subCategories = res;
        this.headCategory = this.subCategories[0];
        this.children = this.subCategories[0].children;
      } else {

        this.pickedCategory = res[0];
      }
      this.cdr.detectChanges();
      console.log(this.pickedCategory);
    });
  }

  goBack(event: Category) {
    if (event !== undefined && event !== null) {
      this.getParentCategory(event.parentId);

      this.categoryService.getFewBySuperiorId(event.id, 2).subscribe(res => {
        this.subCategories = res;
        this.headCategory = this.subCategories[0];
        this.children = this.subCategories[0].children;
        this.cdr.detectChanges();
      });

    } else {
      this.isInSubCategories = false;
      this.categoryService.getSuperiorsWithChildren(1)
        .subscribe(res => {
          this.subCategories = res;
          this.children = this.subCategories;
          this.cdr.detectChanges();
        });
    }
  }


  onFileSelected(event: any) {
    this.selectedImages = event.target.files;
    for (let i = 0; i < this.selectedImages.length; i++) {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        const url = e.target.result;
        this.previews.push(url);
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(this.selectedImages[i]);
    }
  }

  deleteImage(index: number) {
    this.selectedImages = Array.from(this.selectedImages).filter((file, i) => i !== index);
    this.previews.splice(index, 1);
  }

  // title: string, desc: string, price: number, quantity: number, condition: string, categoryId: number, pics: File[]
  addOffer() {
    const title: string = this.createOffer.value.title;
    const desc: string = this.createOffer.value.description;
    const price: number = this.createOffer.value.price;
    const quantity: number = this.createOffer.value.quantity;

    console.log("title: " + title);
    console.log("desc: " + desc);
    console.log("price: " + price.toString());
    console.log("quantity: " + quantity.toString());
    console.log("condition: " + Condition[this.condition]);

    this.offerService.createOffer(title, desc, price, quantity, Condition[this.condition], this.pickedCategory.id, this.selectedImages).subscribe(res =>
      console.log(res));
  }
}
