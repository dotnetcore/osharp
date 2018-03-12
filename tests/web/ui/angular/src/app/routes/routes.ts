import { LayoutComponent } from '../layout/layout.component';

export const routes = [

    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', loadChildren: './home/home.module#HomeModule' },
            { path: 'material', loadChildren: './material/material.module#MaterialModule' },
            { path: 'admin', loadChildren: '../admin/admin.module#AdminModule' },
        ]
    },

    // Not found
    { path: '**', redirectTo: 'home' }

];
