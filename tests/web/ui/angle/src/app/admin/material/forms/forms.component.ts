import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-forms',
    templateUrl: './forms.component.html',
    styleUrls: ['./forms.component.scss']
})
export class FormsComponent implements OnInit {

    user: any = {};
    username: any = {};
    phone: any = {};

    constructor() { }

    ngOnInit() {
    }

}
