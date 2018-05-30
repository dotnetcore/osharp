import { Component, OnInit, OnDestroy, } from '@angular/core';
import { RegisterService, } from './register.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'app-identity-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {

  constructor(private _service: RegisterService) { }

  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
