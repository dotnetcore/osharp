import { Directive, forwardRef, Input } from '@angular/core';
import { NG_ASYNC_VALIDATORS, AsyncValidator, AbstractControl } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[remoteInverse][formControlName],[remoteInverse][formControl],[remoteInverse][ngModel]',
  providers: [
    { provide: NG_ASYNC_VALIDATORS, useExisting: forwardRef(() => RemoteInverseValidator), multi: true }
  ]
})
export class RemoteInverseValidator implements AsyncValidator {

  // tslint:disable-next-line:no-input-rename
  @Input('remoteInverse') url: string;
  private timeout;

  constructor(private http: HttpClient) { }

  validate(elem: AbstractControl): Promise<{ [key: string]: any; }> | Observable<{ [key: string]: any; }> {
    let url = elem.value;
    url = this.url.replace(/:value/ig, url);

    clearTimeout(this.timeout);
    return new Promise((resolve) => {
      this.timeout = setTimeout(() => {
        this.http.get(url).subscribe(res => {
          if (res !== true) {
            resolve({ remoteInverse: true });
          } else {
            resolve(null);
          }
        }, error => {
          resolve({ remoteInverse: false });
          throw error;
        });
      }, 500);
    });
  }
  registerOnValidatorChange?(fn: () => void): void { }
}
