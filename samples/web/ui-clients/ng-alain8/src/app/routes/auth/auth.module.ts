import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { AuthRoutingModule } from "./auth.routing";
import { ModuleComponent } from "./module/module.component";
import { FunctionComponent } from "./function/function.component";
import { EntityinfoComponent } from "./entityinfo/entityinfo.component";
import { RoleEntityinfoComponent } from "./role-entityinfo/role-entityinfo.component";
import { SharedModule } from "@shared";

@NgModule({
	declarations: [ ModuleComponent, FunctionComponent, EntityinfoComponent, RoleEntityinfoComponent ],
	imports: [ CommonModule, SharedModule, AuthRoutingModule ],
	entryComponents: [ ModuleComponent ]
})
export class AuthModule {}
