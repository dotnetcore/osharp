import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'layout-footer',
  templateUrl: './footer.component.html',
  styles: [`

footer {
  padding: 12px;
  font-size: 14px;
  margin-top: 5px;
}


  `]
})
export class FooterComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
