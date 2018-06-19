import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IdentityService } from '../../shared/osharp/services/identity.service';

@Component({
  selector: 'layout-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.css']
})
export class LayoutDefaultComponent {

  isHandset$: Observable<boolean>;

  constructor(
    breakpointObserver: BreakpointObserver,
    private identity: IdentityService
  ) {
    this.isHandset$ = breakpointObserver.observe(Breakpoints.Handset).pipe(map(result => result.matches));
  }

}
