import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { IProduct, DataService } from 'src/app/Services/data.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class ProductsComponent implements OnInit {
  productList: IProduct[] = [];

  constructor(private dataService: DataService, private router: Router) {}

  ngOnInit(): void {
    this.dataService.getProducts().subscribe(
      (response) => (this.productList = response),
      (err) => console.log(err)
    );

    if (!this.dataService.userLoggedIn) {
      this.router.navigate(['login']);
    }
  }

  showProductDetails(id: number) {
    this.router.navigate(['product-details', { id: id }]);
  }
}
