export interface Category {
  id: number,
  name: string,
  parentId?: number;
  children: Category[]
  iconName: string;
}
