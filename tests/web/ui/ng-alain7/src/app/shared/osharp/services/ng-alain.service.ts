import { PageRequest, PageCondition, SortCondition, ListSortDirection, AjaxResult } from '../osharp.model';
import { STColumn, STReq, STRes, STComponent, STChange, STPage, STRequestOptions, STData, STError, STColumnBadge, STColumnTag } from '@delon/abc';
import { ViewChild, Injector } from '@angular/core';
import { OsharpService } from './osharp.service';
import { SFSchema, SFUISchema, SFSchemaEnumType } from '@delon/form';
import { _HttpClient } from '@delon/theme';
import { NzModalComponent, NzTreeNodeOptions, NzTreeNode } from 'ng-zorro-antd';
import { OsharpSTColumn } from './ng-alain.types';

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
  selecteds: STData[] = [];

  public get http(): _HttpClient {
    return this.osharp.http;
  }

  constructor(injector: Injector) {
    this.osharp = injector.get(OsharpService);
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
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body: request, allInBody: true, process: opt => this.RequestProcess(opt)
    };
    return req;
  }

  protected GetSTRes(): STRes {
    let res: STRes = { reName: { list: 'Rows', total: 'Total' }, process: data => this.ResponseDataProcess(data) };
    return res;
  }

  protected GetSTPage(): STPage {
    let page: STPage = { showSize: true, showQuickJumper: true, toTop: true, toTopOffset: 0 };
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
          sort.ListSortDirection = item.substr(num + 1) === 'ascend' ? ListSortDirection.Ascending : ListSortDirection.Descending;
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

  protected ColumnsToSchemas(columns: OsharpSTColumn[]): { [key: string]: SFSchema } {
    let properties: { [key: string]: SFSchema } = {};
    for (const column of columns) {
      if (!column.index || !column.editable) {
        continue;
      }
      let schemaProps = ['enum', 'minimum', 'exclusiveMinimum', 'maximum', 'exclusiveMaximum', 'multipleOf', 'maxLength', 'minLength', 'pattern',
        'items', 'minItems', 'maxItems', 'uniqueItems', 'additionalItems', 'maxProperties', 'minProperties', 'required', 'properties', 'if', 'then',
        'else', 'allOf', 'anyOf', 'oneOf', 'description', 'readOnly', 'definitions', '$ref', '$comment', 'ui'];
      let specialProps = ['ftype', 'fformat', 'fdefault', 'ftitle'];
      let schema: SFSchema = {};
      let keys = Object.getOwnPropertyNames(column);
      for (const key of keys) {
        if (schemaProps.indexOf(key) > -1) {
          schema[key] = column[key];
        } else if (specialProps.indexOf(key) > -1) {
          // fkey的情况，直接使用fkey的值
          let ekey = key.substr(1);
          schema[ekey] = column[key];
        } else if (specialProps.indexOf(`f${key}`) > -1) {
          // key的情况，要取key值来转换才能用
          switch (key) {
            case 'default':
            case 'title':
              schema[key] = column[key];
              break;
            case 'type':
              {
                let val = column[key];
                if (['link', 'img'].indexOf(val) > -1) {
                  schema[key] = 'string';
                } else if (['number', 'currency'].indexOf(val) > -1) {
                  schema[key] = 'number';
                } else if (val === 'yn') {
                  schema[key] = 'boolean';
                } else if (val === 'date') {
                  schema[key] = 'string';
                  schema.format = 'date-time';
                } else {
                  schema[key] = 'string';
                }
              }
              break;
          }
        }
      }
      if (keys.indexOf('type') === -1) {
        schema.type = 'string';
      }

      properties[column.index as string] = schema;
    }
    return properties;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {};
    return ui;
  }

  protected toEnum(items: { id: number, text: string }[]): SFSchemaEnumType[] {
    return items.map(item => {
      let e: SFSchemaEnumType = { value: item.id, label: item.text };
      return e;
    });
  }

  create() {
    if (!this.editModal) return;
    this.editRow = {};
    this.editTitle = "新增";
    this.editModal.open();
  }

  edit(row: STData) {
    if (!row || !this.editModal) {
      return;
    }
    this.editRow = row;
    this.editTitle = '编辑';
    this.editModal.open();
  }

  close() {
    if (!this.editModal) return;
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

import { Injectable } from '@angular/core';
import { List } from 'linqts';
import { Observable } from 'rxjs';
import { stringify } from '@angular/core/src/util';


@Injectable({
  providedIn: 'root'
})
export class AlainService {

  constructor(private osharp: OsharpService) { }

  public get http(): _HttpClient {
    return this.osharp.http;
  }

  /**
   * 将原始树数据节点转换为 `NzTreeNodeOptions`节点
   * @param nodes 原始树数据节点
   */
  ToNzTreeData(nodes: any[]): NzTreeNodeOptions[] {
    if (!nodes.length) {
      return [];
    }
    let result: NzTreeNodeOptions[] = [];
    for (const node of nodes) {
      let item: NzTreeNodeOptions = { title: node.Name, key: node.Id, checked: node.IsChecked, isLeaf: !node.HasChildren };
      if (node.Children && node.Children.length > 0) {
        item.children = this.ToNzTreeData(node.Children);
      }
      result.push(item);
    }
    return result;
  }

  GetNzTreeCheckedIds(nodes: NzTreeNode[]) {
    let ids = [];
    let stack: NzTreeNode[] = [...nodes];
    while (stack.length > 0) {
      let node = stack.pop();
      if (node.isChecked) {
        ids.push(node.key);
      }
      if (node.children && node.children.length > 0) {
        stack.push(...node.children);
      }
    }
    ids = new List(ids).Distinct().ToArray();
    return ids;
  }

  ReadNode(url: string, labelName: string, valueName: string): Observable<SFSchemaEnumType[]> {
    return this.http.get(url).map((nodes: any[]) => {
      let items: SFSchemaEnumType[] = [];
      for (const node of nodes) {
        let item: SFSchemaEnumType = { label: node[labelName], value: node[valueName] };
        items.push(item);
      }
      return items;
    });
  }

  AjaxResultTypeTags: STColumnTag = {
    200: { text: '成功', color: 'green' },
    203: { text: '消息', color: '' },
    401: { text: '未登录', color: 'orange' },
    403: { text: '无权操作', color: 'orange' },
    404: { text: '不存在', color: 'orange' },
    423: { text: '锁定', color: 'orange' },
    500: { text: '错误', color: 'red' }
  };
  AccessTypeTags: STColumnTag = {
    0: { text: '匿名访问', color: 'green' },
    1: { text: '登录访问', color: 'blue' },
    2: { text: '角色访问', color: 'orange' },
  };
  DataAuthOperationTags: STColumnTag = {
    0: { text: '读取', color: 'green' },
    1: { text: '更新', color: 'blue' },
    2: { text: '删除', color: 'orange' },
  };
  OperateTypeTags: STColumnTag = {
    0: { text: '读取', color: '' },
    1: { text: '新增', color: 'green' },
    2: { text: '更新', color: 'blue' },
    3: { text: '删除', color: 'orange' },
  };
}
