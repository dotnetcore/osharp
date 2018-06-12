import { NgModule, ModuleWithProviders, } from '@angular/core';
import { OSHARP_LOCAL_CACHE_TOKEN, CacheService, OSHARP_MEMORY_CACHE_TOKEN } from '@shared/osharp/cache/cache.service';
import { LocalStoreCacheService } from '@shared/osharp/cache/local-store-cache.service';
import { MemoryCacheService } from '@shared/osharp/cache/memory-cache.service';

@NgModule({})
export class OsharpCacheModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: OsharpCacheModule,
      providers: [
        { provide: OSHARP_LOCAL_CACHE_TOKEN, useClass: LocalStoreCacheService },
        { provide: OSHARP_MEMORY_CACHE_TOKEN, useClass: MemoryCacheService },
        CacheService,
      ]
    };
  }
}
