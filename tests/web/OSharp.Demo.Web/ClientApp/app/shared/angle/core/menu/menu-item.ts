export class menuItem {
    constructor(text: string) {
        this.text = text;
    }

    text: string;
    heading?: boolean;
    link?: string;
    icon?: string;
    alert?: string;
    submenu?: Array<menuItem>;
}
