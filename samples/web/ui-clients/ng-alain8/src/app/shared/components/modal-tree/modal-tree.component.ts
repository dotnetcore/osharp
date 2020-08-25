import { Component, Input, Output, EventEmitter, ViewChild, OnInit } from '@angular/core';
import { NzTreeNode, NzTreeNodeOptions, NzModalComponent, NzTreeComponent } from 'ng-zorro-antd';
import { _HttpClient } from '@delon/theme';
import { AlainService } from '@shared/osharp/services/alain.service';

@Component({
  selector: 'app-osharp-modal-tree',
  template: `
  <nz-modal #modal [(nzVisible)]="visible" [nzTitle]="title" [nzClosable]="false" [nzFooter]="permissFooter"
    [nzBodyStyle]="{'max-height':'600px', 'overflow-y': 'auto'}">
    <nz-alert *ngIf="loading" nzType="info" nzMessage="正在加载树数据，请稍候……"></nz-alert>
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
  @ViewChild("modal", { static: false }) modal: NzModalComponent;

  constructor(public http: _HttpClient, private alain: AlainService) { }

  // TODO: 不知原因，nzAfterOpen事件会重复调用，暂时使用loading来阻隔
  loading = false;
  private loadTreeData() {
    if (this.loading) {
      return;
    }
    this.loading = true;
    let url = this.treeDataUrl;
    if (!url) {
      return;
    }
    this.http.get(url).map((res: any[]) => {
      return this.alain.ToNzTreeData(res);
    }).subscribe(res => {
      this.treeData = res;
      this.loading = false;
    });
  }

  open() {
    this.modal.open();
    setTimeout(() => {
      this.loadTreeData();
    }, 100);
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
