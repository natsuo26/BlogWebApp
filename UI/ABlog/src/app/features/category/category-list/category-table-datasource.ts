import { MatTableDataSource } from '@angular/material/table';
import { Category } from '../models/category.model';

export class CategoryDataSource extends MatTableDataSource<Category> {
  constructor(categories: Category[]) {
    super(categories);
  }
}
