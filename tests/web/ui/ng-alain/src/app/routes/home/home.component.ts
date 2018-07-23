import { Component, OnInit, Injector, } from '@angular/core';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { HttpClient } from '@angular/common/http';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [`
  .ant-table-thead > tr > th, .ant-table-tbody > tr > td{
    padding:5px;
  }
  `]
})
export class HomeComponent extends ComponentBase implements OnInit {

  systemInfo = null;

  constructor(private http: HttpClient, injector: Injector) {
    super(injector);
    super.checkAuth();
  }

  ngOnInit(): void {
    this.http.get('api/common/systeminfo').subscribe(res => this.systemInfo = res);
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Site.Home', [])
  }
}
