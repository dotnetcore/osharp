import { Component, OnInit, Injector, ViewChild } from "@angular/core";
import { STComponentBase } from "@shared/osharp/components/st-component-base";
import { SFUISchema } from "@delon/form";
import { ModalTreeComponent } from "@shared/components/modal-tree/modal-tree.component";
import { FilterGroup } from "@shared/osharp/osharp.model";
import { STData, STColumn, ReuseTabService } from "@delon/abc";
import { NzTreeNode } from "ng-zorro-antd";
import { FunctionViewComponent } from "@shared/components/function-view/function-view.component";

@Component({
	selector: "app-user",
	templateUrl: "./user.component.html",
	styles: []
})
export class UserComponent extends STComponentBase implements OnInit {
	constructor(injector: Injector) {
		super(injector);
		this.moduleName = "user";
	}

	ngOnInit() {
		super.InitBase();
	}

	protected GetSTColumns(): STColumn[] {
		return [
			{
				title: "操作",
				fixed: "left",
				width: 65,
				buttons: [
					{
						text: "操作",
						children: [
							{ text: "编辑", icon: "edit", acl: "Root.Admin.Identity.User.Update", iif: (row) => row.Updatable, click: (row) => this.edit(row) },
							{ text: "角色", icon: "team", acl: "Root.Admin.Identity.User.SetRoles", click: (row) => this.roles(row) },
							{ text: "权限", icon: "safety", acl: "Root.Admin.Identity.User.SetModules", click: (row) => this.module(row) },
							{ text: "删除", icon: "delete", type: "del", acl: "Root.Admin.Identity.User.Delete", iif: (row) => row.Deletable, click: (row) => this.delete(row) },
							{ text: "查看功能", icon: "security-scan", acl: "Root.Admin.Auth.UserFunction", click: (row) => this.viewFunction(row) }
						]
					}
				]
			},
			{ title: "编号", index: "Id", sort: true, readOnly: true, editable: true, type: "number", filterable: true },
			{ title: "用户名", index: "UserName", sort: true, editable: true, ftype: "string", filterable: true, className: "max-200" },
			{ title: "昵称", index: "NickName", sort: true, editable: true, ftype: "string", filterable: true },
			{ title: "邮箱", index: "Email", sort: true, editable: true, ftype: "string", filterable: true },
			{ title: "邮箱确认", index: "EmailConfirmed", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "手机号", index: "PhoneNumber", sort: true, editable: true, ftype: "string", filterable: true },
			{ title: "手机确认", index: "PhoneNumberConfirmed", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "角色", index: "Roles", format: (d) => this.osharp.expandAndToString(d.Roles) },
			{ title: "是否锁定", index: "IsLocked", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "登录锁", index: "LockoutEnabled", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "登录错误", index: "AccessFailedCount", sort: true, editable: true, ftype: "number", filterable: true },
			{ title: "锁时间", index: "LockoutEnd", sort: true, editable: true, type: "date", filterable: true },
			{ title: "注册时间", index: "CreatedTime", sort: true, type: "date", filterable: true }
		];
	}

	protected GetSFUISchema(): SFUISchema {
		let ui: SFUISchema = {
			"*": { spanLabelFixed: 100, grid: { span: 12 } },
			$UserName: { grid: { span: 24 } },
			$LockoutEnd: { grid: { span: 24 } }
		};
		return ui;
	}

	adSearch(group: FilterGroup) {
		this.request.FilterGroup = group;
		this.st.reload();
	}

	// #region 角色设置

	roleTitle: string;
	roleTreeDataUrl: string;
	@ViewChild("roleModal", { static: false })
	roleModal: ModalTreeComponent;

	private roles(row: STData) {
		this.editRow = row;
		this.roleTitle = `设置用户角色 - ${row.UserName}`;
		this.roleTreeDataUrl = `api/admin/role/ReadUserRoles?userId=${row.Id}`;
		this.roleModal.open();
	}

	setRoles(value: NzTreeNode[]) {
		let ids = this.alain.GetNzTreeCheckedIds(value);
		let body = { userId: this.editRow.Id, roleIds: ids };
		this.http.post("api/admin/user/setRoles", body).subscribe((result) => {
			this.osharp.ajaxResult(result, () => {
				this.st.reload();
				this.roleModal.close();
			});
		});
	}

	// #endregion

	// #region 权限设置

	moduleTitle: string;
	moduleTreeDataUrl: string;
	@ViewChild("moduleModal", { static: false })
	moduleModal: ModalTreeComponent;

	private module(row: STData) {
		this.editRow = row;
		this.moduleTitle = `设置用户权限 - ${row.UserName}`;
		this.moduleTreeDataUrl = `api/admin/module/ReadUserModules?userId=${row.Id}`;
		this.moduleModal.open();
	}

	setModules(value: NzTreeNode[]) {
		let ids = this.alain.GetNzTreeCheckedIds(value);
		let body = { userId: this.editRow.Id, moduleIds: ids };
		this.http.post("api/admin/user/setModules", body).subscribe((result) => {
			this.osharp.ajaxResult(result, () => {
				this.st.reload();
				this.moduleModal.close();
			});
		});
	}

	// #endregion

	// #region 查看功能

	functionTitle: string;
	functionVisible = false;
	functionReadUrl: string;
	@ViewChild("function", { static: false })
	function: FunctionViewComponent;

	private viewFunction(row: STData) {
		this.functionTitle = `查看用户功能 - ${row.Id}. ${row.UserName}`;
		this.functionVisible = true;

		this.functionReadUrl = `api/admin/userfunction/readfunctions?userId=${row.Id}`;
		let filter: FilterGroup = new FilterGroup();
		setTimeout(() => {
			this.function.reload(filter);
		}, 100);
	}

	closeFunction() {
		this.functionVisible = false;
	}
	// #endregion
}
