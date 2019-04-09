import { PageRequest, PageCondition, SortCondition, ListSortDirection, AjaxResult, } from '../osharp.model';
import { STColumn, STReq, STRes, STComponent, STChange, STPage, STRequestOptions, STData, STError, } from '@delon/abc';
import { ViewChild, Injector } from '@angular/core';
import { SFSchema, SFUISchema, SFSchemaEnumType } from '@delon/form';
import { _HttpClient } from '@delon/theme';
import { NzModalComponent, NzTreeNodeOptions, NzTreeNode } from 'ng-zorro-antd';
import { OsharpService } from '../services/osharp.service';
import { AlainService } from '../services/ng-alain.service';
import { OsharpSTColumn } from '../services/ng-alain.types';

export abstract class STComponentBase {
  moduleName: string;

  // URL
  readUrl: string;
  createUrl: string;
  updateUrl: string;
  deleteUrl: string;

  // 表格属性
  columns: STColumn[];
  request: PageRequest;
  req: STReq;
  res: STRes;
  page: STPage;
  @ViewChild('st') st: STComponent;

  // 编辑属性

  schema: SFSchema;
  ui: SFUISchema;
  editRow: STData;
  editTitle = '编辑';
  @ViewChild('modal') editModal: NzModalComponent;

  osharp: OsharpService;
  alain: AlainService;
  selecteds: STData[] = [];

  public get http(): _HttpClient {
    return this.osharp.http;
  }

  constructor(injector: Injector) {
    this.osharp = injector.get(OsharpService);
    this.alain = injector.get(AlainService);
  }

  protected InitBase() {
    this.readUrl = `api/admin/${this.moduleName}/read`;
    this.createUrl = `api/admin/${this.moduleName}/create`;
    this.updateUrl = `api/admin/${this.moduleName}/update`;
    this.deleteUrl = `api/admin/${this.moduleName}/delete`;

    this.request = new PageRequest();
    this.columns = this.GetSTColumns();
    this.req = this.GetSTReq(this.request);
    this.res = this.GetSTRes();
    this.page = this.GetSTPage();

    this.schema = this.GetSFSchema();
    this.ui = this.GetSFUISchema();
  }

  // #region 表格

  /**
   * 重写以获取表格的列设置Columns
   */
  protected abstract GetSTColumns(): OsharpSTColumn[];

  protected GetSTReq(request: PageRequest): STReq {
    let req: STReq = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: request,
      allInBody: true,
      process: opt => this.RequestProcess(opt),
    };
    return req;
  }

  protected GetSTRes(): STRes {
    let res: STRes = {
      reName: { list: 'Rows', total: 'Total' },
      process: data => this.ResponseDataProcess(data),
    };
    return res;
  }

  protected GetSTPage(): STPage {
    let page: STPage = {
      showSize: true,
      showQuickJumper: true,
      toTop: true,
      toTopOffset: 0,
    };
    return page;
  }

  protected RequestProcess(opt: STRequestOptions): STRequestOptions {
    if (opt.body.PageCondition) {
      let page: PageCondition = opt.body.PageCondition;
      page.PageIndex = opt.body.pi;
      page.PageSize = opt.body.ps;
      if (opt.body.sort) {
        page.SortConditions = [];
        let sorts = opt.body.sort.split('-');
        for (const item of sorts) {
          let sort = new SortCondition();
          let num = item.lastIndexOf('.');
          let field = item.substr(0, num);
          field = this.ReplaceFieldName(field);
          sort.SortField = field;
          sort.ListSortDirection =
            item.substr(num + 1) === 'ascend'
              ? ListSortDirection.Ascending
              : ListSortDirection.Descending;
          page.SortConditions.push(sort);
        }
      } else {
        page.SortConditions = [];
      }
    }
    return opt;
  }

  protected ResponseDataProcess(data: STData[]): STData[] {
    return data;
  }

  protected ReplaceFieldName(field: string): string {
    return field;
  }

  search(request: PageRequest) {
    if (!request) {
      return;
    }
    this.req.body = request;
    this.st.reload();
  }

  change(value: STChange) {
    if (value.type === 'checkbox') {
      this.selecteds = value.checkbox;
    } else if (value.type === 'radio') {
      this.selecteds = [value.radio];
    }
  }

  error(value: STError) {
    console.log(value);
  }

  // #endregion

  // #region 编辑

  /**
   * 默认由列配置 `STColumn[]` 来生成SFSchema，不需要可以重写定义自己的SFSchema
   */
  protected GetSFSchema(): SFSchema {
    let schema: SFSchema = { properties: this.ColumnsToSchemas(this.columns) };
    return schema;
  }

  protected ColumnsToSchemas(
    columns: OsharpSTColumn[],
  ): { [key: string]: SFSchema } {
    let properties: { [key: string]: SFSchema } = {};
    for (const column of columns) {
      if (!column.index || !column.editable || column.buttons) {
        continue;
      }
      let schema: SFSchema = this.alain.ToSFSchema(column);
      properties[column.index as string] = schema;
    }
    return properties;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {};
    return ui;
  }

  protected toEnum(items: { id: number; text: string }[]): SFSchemaEnumType[] {
    return items.map(item => {
      let e: SFSchemaEnumType = { value: item.id, label: item.text };
      return e;
    });
  }

  create() {
    if (!this.editModal) return;
    this.schema = this.GetSFSchema();
    this.ui = this.GetSFUISchema();
    this.editRow = {};
    this.editTitle = '新增';
    this.editModal.open();
  }

  edit(row: STData) {
    if (!row || !this.editModal) {
      return;
    }
    this.schema = this.GetSFSchema();
    this.ui = this.GetSFUISchema();
    this.editRow = row;
    this.editTitle = '编辑';
    this.editModal.open();
  }

  close() {
    if (!this.editModal) return;
    console.log(this.editModal);
    this.editModal.destroy();
  }

  save(value: STData) {
    let url = value.Id ? this.updateUrl : this.createUrl;
    this.http.post<AjaxResult>(url, [value]).subscribe(result => {
      this.osharp.ajaxResult(result, () => {
        this.st.reload();
        this.editModal.destroy();
      });
    });
  }

  delete(value: STData) {
    if (!value) {
      return;
    }
    this.http.post<AjaxResult>(this.deleteUrl, [value.Id]).subscribe(result => {
      this.osharp.ajaxResult(result, () => {
        this.st.reload();
      });
    });
  }

  // #endregion
}
