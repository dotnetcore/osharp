import { Component } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'passport-register-result',
  templateUrl: './register-result.component.html',
})
export class RegisterResultComponent {
  params = { email: '' };
  email = '';
  constructor(
    route: ActivatedRoute,
    public msg: NzMessageService,
  ) {
    this.params.email = this.email = route.snapshot.queryParams.email || 'ng-alain@example.com';
  }
}
