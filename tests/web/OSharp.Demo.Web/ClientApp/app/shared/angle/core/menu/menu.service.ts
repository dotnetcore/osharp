import { Injectable } from '@angular/core';
import { List } from "linqts";

import { menuItem } from "./menu-item";

@Injectable()
export class MenuService {

    constructor() {
        this.menuItems = [];
    }

    menuItems: Array<menuItem>;

    addMenu(items: Array<menuItem>) {
        items.forEach(item => this.menuItems.push(item));
    }

    removeMenu(item: menuItem) {
        let list = new List<menuItem>(this.menuItems);
        this.menuItems = list.RemoveAll(m => m.text == item.text).ToArray();
    }

    getMenu() {
        return this.menuItems;
    }
}