import { Component, OnInit, AfterViewInit, NgZone, ElementRef, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { osharp } from "../../../shared/osharp";
import { kendoui } from '../../../shared/kendoui';


@Component({
  selector: 'security-role-function',
  templateUrl: './role-function.component.html'
})
export class RoleFunctionComponent extends kendoui.TreeListComponentBase implements OnInit, AfterViewInit {

  constructor(protected zone: NgZone, protected element: ElementRef, private http: HttpClient) {
    super(zone, element);
  }

  protected GetModel() {
    throw new Error("Method not implemented.");
  }
  protected GetTreeListColumns(): kendo.ui.TreeListColumn[] {
    throw new Error("Method not implemented.");
  }

  ngOnInit() {
    super.InitBase();
  }

  ngAfterViewInit(): void {
    super.ViewInitBase();
  }
}
