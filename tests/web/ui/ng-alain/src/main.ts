import { enableProdMode, ViewEncapsulation } from '@angular/core';
import { registerLocaleData } from "@angular/common";
import zh from "@angular/common/locales/zh";
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import { preloaderFinished } from '@delon/theme';

registerLocaleData(zh);

preloaderFinished();

if (environment.production) {
  enableProdMode();
}

const bootstrap = () => {
  return platformBrowserDynamic().bootstrapModule(AppModule, {
    defaultEncapsulation: ViewEncapsulation.Emulated,
    preserveWhitespaces: false,
  });
};

bootstrap().then(() => {
  if ((<any>window).appBootstrap) {
    (<any>window).appBootstrap();
  }
});
