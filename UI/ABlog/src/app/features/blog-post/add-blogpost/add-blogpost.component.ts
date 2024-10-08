import { Component, OnDestroy } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.model';
import { BlogPostService } from '../services/blog-post.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.scss',
})
export class AddBlogpostComponent implements OnDestroy {
  model: AddBlogPost;
  private destroy$ = new Subject<void>();

  constructor(private blogPostService: BlogPostService) {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featureImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(),
    };
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
