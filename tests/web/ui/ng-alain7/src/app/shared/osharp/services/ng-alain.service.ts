import { PageRequest, PageCondition, SortCondition, ListSortDirection, AjaxResult } from '../osharp.model';
import { STColumn, STReq, STRes, STComponent, STChange, STPage, STRequestOptions, STData, STError } from '@delon/abc';
import { ViewChild, Injector } from '@angular/core';
import { OsharpService } from './osharp.service';
import { SFSchema, SFUISchema, SFSchemaEnumType } from '@delon/form';
import { _HttpClient } from '@delon/theme';
import { NzModalComponent } from 'ng-zorro-antd';
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
    this.req = this.GetSTReq();
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

  protected GetSTReq(): STReq {
    let req: STReq = {
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body: this.request, allInBody: true, process: this.RequestProcess
    };
    return req;
  }

  protected GetSTRes(): STRes {
    let res: STRes = { reName: { list: 'Rows', total: 'Total' }, process: this.ResponseDataProcess };
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
        sorts.forEach((item: string) => {
          let sort = new SortCondition();
          let num = item.lastIndexOf('.');
          sort.SortField = item.substr(0, num);
          sort.ListSortDirection = item.substr(num + 1) === 'ascend' ? ListSortDirection.Ascending : ListSortDirection.Descending;
          page.SortConditions.push(sort);
        });
      } else {
        page.SortConditions = [];
      }
    }
    return opt;
  }

  protected ResponseDataProcess(data: STData[]): STData[] {
    return data;
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
      for (const key in column) {
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
                if (['link', 'img', 'date'].indexOf(val) > -1) {
                  schema[key] = 'string';
                } else if (['number', 'currency'].indexOf(val) > -1) {
                  schema[key] = 'number';
                } else if (val === 'yn') {
                  schema[key] = 'boolean';
                }
              }
              break;
          }
        }
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
    this.editRow = {};
    this.editTitle = "新增";
    this.editModal.open();
  }

  edit(row: STData) {
    if (!row) {
      return;
    }
    this.editRow = row;
    this.editTitle = '编辑';
    this.editModal.open();
  }

  close() {
    this.editModal.close();
  }

  save(value: STData) {
    let url = value.Id ? this.updateUrl : this.createUrl;
    this.http.post<AjaxResult>(url, [value]).subscribe(result => {
      this.osharp.ajaxResult(result, () => {
        this.st.reload();
        this.editModal.close();
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
