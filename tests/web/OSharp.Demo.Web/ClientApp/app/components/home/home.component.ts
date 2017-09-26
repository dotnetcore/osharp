import { Component } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    constructor(){
        console.log("constructor: "+this.chsstr);
    }
    chsstr: string = "TS输出的一串中文来捣乱的";
}
