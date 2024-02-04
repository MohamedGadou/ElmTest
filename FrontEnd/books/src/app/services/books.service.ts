import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import { Book } from "../models/book";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: "root"
})
export class BooksService {

    private baseUrl: string = environment.baseUrl;
    constructor(private httpClient:  HttpClient) {
    }

    getAllBooks(pageSize: number, pageNumber: number, searchKey: string): Observable<Book[]> {
        const url = this.baseUrl + '/Books/getAllBooks';
        return this.httpClient.get<Book[]>(url, {
            params: {
                "pageSize":pageSize,
                "pageNumber":pageNumber,
                "searchKey":searchKey
            }
        })
    }
}