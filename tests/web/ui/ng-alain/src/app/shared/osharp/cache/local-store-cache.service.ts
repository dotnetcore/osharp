import { Injectable } from '@angular/core';
import { ICacheStore, ICacheItem } from '@shared/osharp/cache/cache.service';

@Injectable()
export class LocalStoreCacheService implements ICacheStore {

  get(key: string): ICacheItem {
    return JSON.parse(localStorage.getItem(key) || null) || null;
  }

  set(key: string, value: ICacheItem) {
    localStorage.setItem(key, JSON.stringify(value));
  }

  remove(key: string) {
    localStorage.removeItem(key);
  }
}
