import { Component, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Component({
    selector: 's-home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    apiValues = [];

    constructor(private http: HttpClient) {

    }

    ngOnInit() {
        this.http.get<string[]>("api/admin/test/getlines").subscribe(res => this.apiValues = res);
    }
}
