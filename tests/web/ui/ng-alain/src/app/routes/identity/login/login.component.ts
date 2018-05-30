import { Component, OnInit, OnDestroy, } from '@angular/core';
import { LoginService, } from './login.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'app-identity-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  constructor(private _service: LoginService) { }

  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
