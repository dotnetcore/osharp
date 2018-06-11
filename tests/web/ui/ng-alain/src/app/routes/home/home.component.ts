import { Component, } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {

  flag: boolean = false;
  url = "/api/user/read";

  constructor(private osharp: OsharpService) {

  }

  checkAuth() {

    this.osharp.checkUrlAuth(this.url).subscribe(res => {
      console.log(res);
      this.flag = res;
      this.url = res ? '/api/user/read' : '/api/admin/user/read';
    });
  }
}
