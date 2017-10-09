import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'osharp';
  apiValues: string[];

  constructor(private http: Http) { };

  ngOnInit() {
    this.http.get('/api/values').subscribe(values => {
      this.apiValues = values.json() as string[];
    });
  }
}
