import { Directive, forwardRef, Input } from '@angular/core';
import { NG_ASYNC_VALIDATORS, AsyncValidator, AbstractControl } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[remote][formControlName],[remote][formControl],[remote][ngModel]',
  providers: [
    { provide: NG_ASYNC_VALIDATORS, useExisting: forwardRef(() => RemoteValidator), multi: true }
  ]
})
export class RemoteValidator implements AsyncValidator {

  // tslint:disable-next-line:no-input-rename
  @Input('remote') url: string;

  constructor(private http: HttpClient) { }

  validate(elem: AbstractControl): Promise<{ [key: string]: any; }> | Observable<{ [key: string]: any; }> {
    let url = elem.value;
    url = this.url.replace(/:value/ig, url);

    return new Promise(resolve => {
      this.http.get(url).subscribe(res => {
        if (res !== true) {
          resolve(null);
        } else {
          resolve({ remote: true });
        }
      }, error => {
        resolve({ remote: true });
        throw error;
      });
    });
  }
  registerOnValidatorChange?(fn: () => void): void { }
}
