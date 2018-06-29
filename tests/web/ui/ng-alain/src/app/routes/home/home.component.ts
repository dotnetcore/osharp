import { Component, OnInit, } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [`
  .ant-table-thead > tr > th, .ant-table-tbody > tr > td{
    padding:5px;
  }
  `]
})
export class HomeComponent implements OnInit {

  systemInfo = null;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get('api/common/systeminfo').subscribe(res => this.systemInfo = res);
  }
}
