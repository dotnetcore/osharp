import { Component, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Component({
    selector: 's-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent {
    startupInfo: any = null;

    constructor(private http: HttpClient) {

    }

    ngOnInit() {
        this.http.get("api/admin/system/info").subscribe(res => this.startupInfo = res);
    }
}
