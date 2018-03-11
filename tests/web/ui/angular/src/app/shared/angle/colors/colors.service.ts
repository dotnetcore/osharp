import { Injectable } from '@angular/core';

@Injectable()
export class ColorsService {

    private APP_COLORS = {
          'primary':                '#3F51B5',
          'success':                '#4CAF50',
          'info':                   '#2196F3',
          'warning':                '#FF9800',
          'danger':                 '#F44336',
          'inverse':                '#607D8B',
          'green':                  '#009688',
          'pink':                   '#E91E63',
          'purple':                 '#673AB7',
          'dark':                   '#263238',
          'yellow':                 '#FFEB3B',
          'gray-darker':            '#232735',
          'gray-dark':              '#3a3f51',
          'gray':                   '#dde6e9',
          'gray-light':             '#e4eaec',
          'gray-lighter':           '#edf1f2'
    };

    constructor() { }

    byName(name) {
        // console.log(name +' -> ' + this.APP_COLORS[name])
        return (this.APP_COLORS[name] || '#fff');
    }

}
