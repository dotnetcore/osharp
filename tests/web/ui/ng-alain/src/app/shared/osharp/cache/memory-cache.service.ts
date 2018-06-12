import { Injectable } from '@angular/core';
import { ICacheStore, ICacheItem } from '@shared/osharp/cache/cache.service';

@Injectable()
export class MemoryCacheService implements ICacheStore {

  private readonly memory: Map<string, ICacheItem> = new Map<string, ICacheItem>();

  get(key: string): ICacheItem {
    let value = this.memory.get(key);
    return value;
  }
  set(key: string, value: ICacheItem) {
    this.memory.set(key, value);
  }
  remove(key: string) {
    this.memory.delete(key);
  }
}
