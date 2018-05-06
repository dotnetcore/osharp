import { Directive, forwardRef, Input } from '@angular/core';
import { NG_ASYNC_VALIDATORS, AsyncValidator, AbstractControl } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';

@Directive({
  selector: '[remote][formControlName],[remote][formControl],[remote][ngModel]',
  providers: [
    { provide: NG_ASYNC_VALIDATORS, useExisting: forwardRef(() => RemoteValidator), multi: true }
  ]
})
export class RemoteValidator implements AsyncValidator {

  @Input('remote') url: string;

  constructor(private http: HttpClient) { }

  validate(elem: AbstractControl): Promise<{ [key: string]: any; }> | Observable<{ [key: string]: any; }> {
    let value = elem.value;
    let url = this.url.replace(/:value/ig, value);

    return new Promise(resolve => {
      this.http.get(url).subscribe(res => {
        if (res != true) {
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
