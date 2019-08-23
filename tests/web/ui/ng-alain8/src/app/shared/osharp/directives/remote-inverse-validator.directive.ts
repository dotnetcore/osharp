import { Directive, forwardRef, Input } from '@angular/core';
import { NG_ASYNC_VALIDATORS, AsyncValidator, AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { _HttpClient } from '@delon/theme';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[remoteInverse][formControlName],[remoteInverse][formControl],[remoteInverse][ngModel]',
  providers: [
    { provide: NG_ASYNC_VALIDATORS, useExisting: forwardRef(() => RemoteInverseValidator), multi: true }
  ]
})
export class RemoteInverseValidator implements AsyncValidator {

  // tslint:disable-next-line: no-input-rename
  @Input('remoteInverse') url: string;
  private timeout;

  constructor(
    private http: _HttpClient,
    private osharp: OsharpService
  ) { }

  validate(elem: AbstractControl): Promise<{ [key: string]: any; }> | Observable<{ [key: string]: any; }> {
    let value = elem.value;
    if (this.url.indexOf("value&verifycodeid=") > 0) {
      // 拼验证码
      const id = this.osharp.subStr(this.url, "value&verifycodeid=", "");
      value = `${value}&id=${id}`;
    }
    const url = this.url.replace(/:value\S*/, value);

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
