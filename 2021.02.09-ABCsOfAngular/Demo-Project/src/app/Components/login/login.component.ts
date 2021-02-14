import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/Services/data.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  invalidLogin: boolean = false;

  @Input() message;
  @Output() outgoing = new EventEmitter();

  constructor(private router: Router, public dataService: DataService) { }

  ngOnInit(): void {
    this.outgoing.emit('an output event happened')
  }

  login() : void {
    if (this.username == 'user' && this.password == 'pass') {
      this.invalidLogin = false;
      this.dataService.userLoggedIn = true;
      this.dataService.username = this.username;
      this.router.navigate(['products']);
    }
    else {
      this.invalidLogin = true;
    }
  }
}
