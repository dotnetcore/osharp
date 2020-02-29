import { Component, OnInit, Injector, ViewChild } from "@angular/core";
import { STComponentBase } from "@shared/osharp/components/st-component-base";
import { STData } from "@delon/abc";
import { OsharpSTColumn } from "@shared/osharp/services/alain.types";
import { SFUISchema } from "@delon/form";
import { NzTreeNode } from "ng-zorro-antd";
import { ModalTreeComponent } from "@shared/components/modal-tree/modal-tree.component";
import { FunctionViewComponent } from "@shared/components/function-view/function-view.component";
import { FilterGroup } from "@shared/osharp/osharp.model";

@Component({
	selector: "app-identity-role",
	templateUrl: "./role.component.html"
})
export class RoleComponent extends STComponentBase implements OnInit {
	constructor(injector: Injector) {
		super(injector);
		this.moduleName = "role";
	}

	ngOnInit() {
		super.InitBase();
	}

	protected GetSTColumns(): OsharpSTColumn[] {
		return [
			{
				title: "操作",
				fixed: "left",
				width: 65,
				buttons: [
					{
						text: "操作",
						children: [
							{ text: "编辑", icon: "edit", acl: "Root.Admin.Identity.Role.Update", iif: (row) => row.Updatable, click: (row) => this.edit(row) },
							{ text: "权限", icon: "safety", acl: "Root.Admin.Identity.Role.SetModules", click: (row) => this.module(row) },
							{ text: "删除", icon: "delete", type: "del", acl: "Root.Admin.Identity.Role.Delete", iif: (row) => row.Deletable, click: (row) => this.delete(row) },
							{ text: "查看功能", icon: "security-scan", acl: "Root.Admin.Auth.UserFunction", click: (row) => this.viewFunction(row) }
						]
					}
				]
			},
			{ title: "编号", index: "Id", sort: true, readOnly: true, editable: true, filterable: true, ftype: "number" },
			{ title: "名称", index: "Name", sort: true, editable: true, filterable: true, ftype: "string" },
			{ title: "备注", index: "Remark", sort: true, editable: true, filterable: true, ftype: "string" },
			{ title: "管理角色", index: "IsAdmin", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "默认", index: "IsDefault", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "锁定", index: "IsLocked", sort: true, type: "yn", editable: true, filterable: true },
			{ title: "创建时间", index: "CreatedTime", sort: true, type: "date", filterable: true }
		];
	}

	protected GetSFUISchema(): SFUISchema {
		let ui: SFUISchema = {
			"*": { spanLabelFixed: 100, grid: { span: 12 } },
			$Name: { grid: { span: 24 } },
			$Remark: { widget: "textarea", grid: { span: 24 } }
		};
		return ui;
	}

	// #region 权限设置

	moduleTitle: string;
	moduleTreeDataUrl: string;
	@ViewChild("moduleModal", { static: false })
	moduleModal: ModalTreeComponent;

	private module(row: STData) {
		this.editRow = row;
		this.moduleTitle = `修改角色权限 - ${row.Name}`;
		this.moduleTreeDataUrl = `api/admin/module/ReadRoleModules?roleId=${row.Id}`;
		this.moduleModal.open();
	}

	setModules(value: NzTreeNode[]) {
		let ids = this.alain.GetNzTreeCheckedIds(value);
		let body = { roleId: this.editRow.Id, moduleIds: ids };
		this.http.post("api/admin/role/setModules", body).subscribe((result) => {
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
		this.functionTitle = `查看角色功能 - ${row.Id}. ${row.Name}`;
		this.functionVisible = true;

		this.functionReadUrl = `api/admin/rolefunction/readfunctions?roleId=${row.Id}`;
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
