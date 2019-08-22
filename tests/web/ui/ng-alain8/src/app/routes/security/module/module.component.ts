import { Component, OnInit, ViewChild } from '@angular/core';
import { AlainService } from '@shared/osharp/services/alain.service';
import { PageRequest, FilterRule, FilterOperate } from '@shared/osharp/osharp.model';
import { List } from 'linqts';
import { ArrayService } from '@delon/util';
import { STColumn, STReq, STPage, STRes, STComponent } from '@delon/abc';

export interface TreeNodeInterface {
  Id: number;
  Name: string;
  Remark: string;
  Code: string;
  OrderCode: number;
  level: number;
  expand: boolean;
  children?: TreeNodeInterface[];
}

@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styles: []
})
export class ModuleComponent implements OnInit {

  data = [];
  request: PageRequest = new PageRequest();
  mapOfExpandedData: { [key: string]: TreeNodeInterface[] } = {};

  constructor(private alain: AlainService, private arraySrv: ArrayService) { }

  ngOnInit() {
    this.loadData();
    this.functionInit();
  }

  private loadData() {
    let url = 'api/admin/module/read';
    this.alain.http.post(url, this.request).subscribe(res => {
      let tree = this.arraySrv.arrToTree(res, { idMapName: 'Id', parentIdMapName: 'ParentId' });
      this.data = tree;
      tree.forEach(item => this.mapOfExpandedData[item.Id] = this.convertTreeToList(item));
    });
  }

  collapse(array: TreeNodeInterface[], data: TreeNodeInterface, $event: boolean): void {
    if ($event === false) {
      if (data.children) {
        data.children.forEach(d => {
          const target = array.find(a => a.Id === d.Id)!;
          target.expand = false;
          this.collapse(array, target, false);
        });
      } else {
        return;
      }
    }
  }

  convertTreeToList(root: object): TreeNodeInterface[] {
    const stack: any[] = [];
    const array: any[] = [];
    const hashMap = {};
    stack.push({ ...root, level: 0, expand: true });

    while (stack.length !== 0) {
      const node = stack.pop();
      this.visitNode(node, hashMap, array);
      if (node.children) {
        for (let i = node.children.length - 1; i >= 0; i--) {
          stack.push({ ...node.children[i], level: node.level + 1, expand: true, parent: node });
        }
      }
    }

    return array;
  }

  visitNode(node: TreeNodeInterface, hashMap: { [key: number]: any }, array: TreeNodeInterface[]): void {
    if (!hashMap[node.Id]) {
      hashMap[node.Id] = true;
      array.push(node);
    }
  }

  // #region 抽屉

  drawerTitle = '';
  drawerVisible = false;
  functionUrl = 'api/admin/module/readfunctions';
  functionColumns: STColumn[];
  functionRequest: PageRequest;
  functionReq: STReq;
  functionRes: STRes;
  functionPage: STPage;
  @ViewChild('functionST', { static: true }) functionST: STComponent;

  functionInit() {
    this.functionColumns = [
      { title: '功能名称', index: 'Name', filterable: true },
      { title: '功能类型', index: 'AccessType', type: 'tag', tag: this.alain.AccessTypeTags, width: 100, filterable: true },
    ];
    this.functionRequest = new PageRequest();
    this.functionReq = this.alain.GetSTReq(this.functionRequest, this.alain.RequestProcess);
    this.functionPage = this.alain.GetSTPage();
    this.functionRes = this.alain.GetSTRes();
  }

  showFunction(item) {
    this.drawerTitle = `查看功能 - ${item.Remark || item.Name}`;
    this.drawerVisible = true;

    this.functionST.pi = 1;
    let filterGroup = this.functionRequest.FilterGroup;
    filterGroup.Rules = [];
    filterGroup.Rules.push(new FilterRule('TreePathString', `$${item.Id}$`, FilterOperate.Contains));

    this.functionReq.body = this.functionRequest;
    this.functionST.reload();
  }

  closeFunction() {
    this.drawerVisible = false;
  }

  // #endregion
}
