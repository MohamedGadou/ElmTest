import { Component, Input } from '@angular/core';
import { Book } from 'src/app/models/book';

@Component({
  selector: 'book-card',
  templateUrl: './book-card.component.html',
  styleUrls: ['./book-card.component.css']
})
export class BookCardComponent {

  @Input() book: Book = {} as Book;

}
