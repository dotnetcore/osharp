import { Directive, forwardRef, Input } from '@angular/core';
import { NG_ASYNC_VALIDATORS, AsyncValidator, AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { _HttpClient } from '@delon/theme';

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
  private timeout;

  constructor(private http: _HttpClient) { }

  validate(elem: AbstractControl): Promise<{ [key: string]: any; }> | Observable<{ [key: string]: any; }> {
    let value = elem.value;
    let url = this.url.replace(/:value/ig, value);

    clearTimeout(this.timeout);
    return new Promise((resolve) => {
      this.timeout = setTimeout(() => {
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
      }, 500);
    });
  }
  registerOnValidatorChange?(fn: () => void): void { }
}
