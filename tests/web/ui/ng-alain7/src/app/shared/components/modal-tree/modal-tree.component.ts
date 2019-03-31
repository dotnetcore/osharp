import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { NzTreeNodeOptions, NzModalComponent, NzTreeComponent, NzTreeNode, NzFormatEmitEvent } from 'ng-zorro-antd';
import { _HttpClient } from '@delon/theme';
import { AlainService } from '@shared/osharp/services/ng-alain.service';

@Component({
  selector: 'app-identity-modal-tree',
  template: `
  <nz-modal #modal [(nzVisible)]="visible" [nzTitle]="title" [nzClosable]="false" [nzFooter]="permissFooter" (nzAfterOpen)="loadTreeData()"
    [nzBodyStyle]="{'max-height':'600px', 'overflow-y': 'auto'}">
    <nz-tree #tree [nzData]="treeData" nzCheckable nzMultiple [nzExpandAll]="true"></nz-tree>
    <ng-template #permissFooter>
      <button nz-button type="button" (click)="close()">关闭</button>
      <button nz-button type="submit" [nzType]="'primary'" (click)="submit(tree)" [nzLoading]="http.loading" [acl]="submitACL">保存</button>
    </ng-template>
  </nz-modal>
  `,
  styles: []
})
export class ModalTreeComponent {

  @Input() title: string;
  @Input() treeDataUrl: string;
  @Input() submitACL: string;
  @Output() submited: EventEmitter<NzTreeNode[]> = new EventEmitter<NzTreeNode[]>();

  visible: boolean;
  treeData: NzTreeNodeOptions[] = [];
  @ViewChild("modal") modal: NzModalComponent;

  constructor(public http: _HttpClient, private alain: AlainService) { }

  loadTreeData() {
    let url = this.treeDataUrl;
    if (!url) {
      return;
    }
    this.http.get(url).map((res: any[]) => {
      return this.alain.ToNzTreeData(res);
    }).subscribe(res => {
      this.treeData = res;
    });
  }

  open() {
    this.modal.open();
  }

  close() {
    this.treeData = [];
    this.modal.destroy();
  }

  submit(tree: NzTreeComponent) {
    let nodes = tree.nzTreeService.getCheckedNodeList();
    this.submited.emit(nodes);
  }
}
