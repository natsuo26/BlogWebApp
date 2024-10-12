import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.model';
import { BlogPostService } from '../services/blog-post.service';
import { Observable, Subject, takeUntil } from 'rxjs';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.scss',
})
export class AddBlogpostComponent implements OnInit,  OnDestroy {
  model: AddBlogPost;
  private destroy$ = new Subject<void>();
  categories$ = new Observable<Category[]>();

  constructor(private blogPostService: BlogPostService, private categoryService: CategoryService) {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featureImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(),
      categories:[]
    };
  }
  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();
  }
  onFormSubmit(): void {
    this.blogPostService
      .createBlogPost(this.model)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          console.log(response);
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
