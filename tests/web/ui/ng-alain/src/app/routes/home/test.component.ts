import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-test',
    template: `<pre>{{text}}</pre>`,
    styles: [``]
})
export class TestComponent implements OnInit {
    text: string;

    constructor(private http: HttpClient) { }

    ngOnInit(): void {
        this.http.get("api/common/test").subscribe((res: any) => this.text = JSON.stringify(res));
    }
}
