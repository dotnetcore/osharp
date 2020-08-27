import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { SimpleGuard } from "@delon/auth";
import { environment } from "@env/environment";
// layout
import { LayoutDefaultComponent } from "../layout/default/default.component";
import { LayoutFullScreenComponent } from "../layout/fullscreen/fullscreen.component";
import { LayoutPassportComponent } from "../layout/passport/passport.component";
// dashboard pages
import { DashboardComponent } from "./dashboard/dashboard.component";
// single pages
import { CallbackComponent } from "./callback/callback.component";
import { ACLGuard } from "@delon/acl";

const routes: Routes = [
	{
		path: "",
		component: LayoutDefaultComponent,
		canActivate: [ SimpleGuard ],
		children: [
			{ path: "", redirectTo: "dashboard", pathMatch: "full" },
			{ path: "dashboard", component: DashboardComponent, data: { title: "仪表盘" } },
			{ path: "exception", loadChildren: () => import("./exception/exception.module").then((m) => m.ExceptionModule) },
			// 业务子模块
			{ path: "identity", loadChildren: () => import("./identity/identity.module").then((m) => m.IdentityModule) },
			{ path: "auth", loadChildren: () => import("./auth/auth.module").then((m) => m.AuthModule) },
			{ path: "systems", loadChildren: () => import("./systems/systems.module").then((m) => m.SystemsModule) },
			{
				path: "infos",
				loadChildren: () => import("./infos/infos.module").then((m) => m.InfosModule),
				canActivateChild: [ ACLGuard ],
				data: { guard: "Root.Admin.Infos" }
			}
		]
	},
	// 全屏布局
	// {
	//     path: 'fullscreen',
	//     component: LayoutFullScreenComponent,
	//     children: [
	//     ]
	// },
	// passport
	{
		path: "passport",
		component: LayoutPassportComponent,
		loadChildren: () => import("./passport/passport.module").then((m) => m.PassportModule)
	},
	// 单页不包裹Layout
	{ path: "callback/:type", component: CallbackComponent },
	{ path: "**", redirectTo: "exception/404" }
];

@NgModule({
	imports: [
		RouterModule.forRoot(routes, {
			useHash: environment.useHash,
			// NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
			// Pls refer to https://ng-alain.com/components/reuse-tab
			scrollPositionRestoration: "top"
		})
	],
	exports: [ RouterModule ]
})
export class RouteRoutingModule {}
