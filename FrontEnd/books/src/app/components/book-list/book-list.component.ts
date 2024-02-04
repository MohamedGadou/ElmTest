import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  Subject,
  Subscription,
  debounceTime,
  distinctUntilChanged,
  switchMap,
} from 'rxjs';
import { Book } from 'src/app/models/book';
import { BooksService } from 'src/app/services/books.service';

@Component({
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css'],
})
export class BookListComponent implements OnInit, OnDestroy {
  books: Book[] = [];
  searchKey: string = '';
  pageSize: number = 10;
  pageNumber: number = 1;

  searchSubject: Subject<string> = new Subject();

  private getAllBooksSubscription!: Subscription;
  constructor(private booksService: BooksService) {}

  ngOnInit(): void {
    this.loadBooks();

    this.searchSubject
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        switchMap(() => {
          return this.booksService.getAllBooks(
            this.pageSize,
            this.pageNumber,
            this.searchKey
          );
        })
      )
      .subscribe((books) => {
        this.books = books;
      });
  }

  private loadBooks() {
    this.getAllBooksSubscription = this.booksService
      .getAllBooks(this.pageSize, this.pageNumber, this.searchKey)
      .subscribe((books) => {
        books.forEach((b) => {
          this.books.push(b);
        });
      });
  }

  ngOnDestroy(): void {
    this.getAllBooksSubscription?.unsubscribe();
  }

  onLoadMoreClick() {
    this.pageNumber++;
    this.loadBooks();
  }

  onSearch(event: string) {
    this.searchSubject.next(event);
  }
}
