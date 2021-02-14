import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private httpClient: HttpClient) { }

  private productList: IProduct[] = [
    { id: 1, name: 'Orange', quantity: 2, location: 'Aisle 1' },
    { id: 2, name: 'Grape', quantity: 5, location: 'Aisle 2' },
    { id: 3, name: 'Banana', quantity: 42, location: 'Aisle 3' },
  ];

  userLoggedIn: boolean = false;
  username: string;

  getProducts() {
    return of(this.productList);
  }

  getProductsFromHttp(url: string) {
    this.httpClient.get(url);
  }

  getProduct(id: number): IProduct | undefined {
    return this.productList.find(p => p.id === id);
  }
}

export interface IProduct {
  id: number;
  name: string;
  quantity: number;
  location: string;
}
