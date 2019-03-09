import { Injectable, InjectionToken, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { addSeconds } from 'date-fns';
import { tap, map } from 'rxjs/operators';
import { _HttpClient } from '@delon/theme';
export const OSHARP_LOCAL_CACHE_TOKEN = new InjectionToken<ICacheStore>('OSHARP_LOCAL_CACHE_TOKEN');
export const OSHARP_MEMORY_CACHE_TOKEN = new InjectionToken<ICacheStore>('OSHARP_MEMORY_CACHE_TOKEN');

@Injectable()
export class CacheService {

  private meta: Set<string> = new Set<string>();
  private metaKey = "__osharp_cache_meta";

  constructor(
    @Inject(OSHARP_LOCAL_CACHE_TOKEN) private local: ICacheStore,
    @Inject(OSHARP_MEMORY_CACHE_TOKEN) private memory: ICacheStore,
    private http: _HttpClient
  ) {
    this.loadMeta();
  }

  //#region Set
  set<T>(key: string, data: Observable<T>, options?: { type: 's' | 'm'; expire?: number });

  set(key: string, data: Observable<any>, options?: { type: 's' | 'm'; expire?: number });

  set(key: string, data: Object, options: { type: 'm' | 's'; expire?: number }): void;

  set(
    key: string,
    data: any | Observable<any>,
    options: {
      /** 存储类型，'m' 表示内存，'s' 表示持久 */
      type?: 'm' | 's';
      /** 过期时间，单位 `秒` */
      expire?: number;
    } = {},
  ): any {
    let e = 0;
    if (options.expire) {
      e = addSeconds(new Date(), options.expire).valueOf();
    }
    if (!(data instanceof Observable)) {
      this.save(options.type, key, { v: data, e: e });
      return;
    }
    return data.pipe(tap((v: any) => {
      this.save(options.type, key, { v, e });
    }));
  }

  private save(type: 'm' | 's', key: string, value: ICacheItem) {
    if (type === 'm') {
      this.memory.set(key, value);
      return;
    }
    this.local.set(key, value);
    this.pushMeta(key);
  }
  //#endregion

  //#region Get

  get<T>(key: string, options?: { mode?: 'promise'; type?: 'm' | 's'; expire?: number }): Observable<T>;

  get(key: string, options?: { mode?: 'promise'; type?: 'm' | 's'; expire?: number; }): Observable<any>;

  get(key: string, options: { mode?: 'none'; type?: 'm' | 's'; expire?: number; }): any;

  get(key: string, options: { mode?: 'promise' | 'none'; type?: 'm' | 's'; expire?: number; } = {}, ): Observable<any> | any {
    const isPromise = options.mode !== 'none';
    const type = options.type || 's';
    const expire = options.expire != null ? options.expire : 0;

    const value: ICacheItem = type == 'm' ? this.memory.get(key) : this.local.get(key);
    if (!value || (value.e && value.e > 0 && value.e < new Date().valueOf())) {
      if (isPromise) {
        return this.http.get(key).pipe(
          map((ret: any) => this._deepGet(ret, [] as string[], null)),
          tap(v => this.set(key, v, { type, expire }))
        );
      }
      return null;
    }
    return isPromise ? of(value.v) : value.v;
  }

  getNone<T>(key: string): T;

  getNone(key: string): any {
    return this.get(key, { mode: 'none' });
  }

  //#endregion

  // #region Remove

  private _remove(key: string) {
    this.memory.remove(key);
    this.local.remove(key);
    this.removeMeta(key);
  }

  remove(key) {
    return this._remove(key);
  }

  clear() {
    this.meta.forEach(key => this._remove(key));
  }
  // #endregion

  //#region Meta
  private loadMeta() {
    let ret = this.local.get(this.metaKey);
    if (ret && ret.v) {
      this.meta.clear();
      (ret.v as string[]).forEach(key => this.meta.add(key));
    }
  }
  private saveMeta() {
    let metaData: string[] = [];
    this.meta.forEach(key => metaData.push(key));
    this.local.set(this.metaKey, { v: metaData, e: 0 });
  }
  private removeMeta(key) {
    if (!this.meta.has(key)) {
      return;
    }
    this.meta.delete(key);
    this.saveMeta();
  }
  private pushMeta(key: string) {
    if (this.meta.has(key)) {
      return;
    }
    this.meta.add(key);
    this.saveMeta();
  }
  _deepGet(obj: any, path: string[], defaultValue?: any) {
    if (!obj) return defaultValue;
    if (path.length <= 1) {
      const checkObj = path.length ? obj[path[0]] : obj;
      return typeof checkObj === 'undefined' ? defaultValue : checkObj;
    }
    return path.reduce((o, k) => o[k], obj) || defaultValue;
  }
  //#endregion
}


/**缓存项 */
export interface ICacheItem {
  /**缓存值 */
  v: any;
  /**过期时间，秒，0表示不过期 */
  e: number;
}

/**缓存存储 */
export interface ICacheStore {
  get(key: string): ICacheItem;
  set(key: string, value: ICacheItem);
  remove(key: string);
}


