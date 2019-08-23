import { Injectable } from '@angular/core';
import { OsharpService } from './osharp.service';
import { _HttpClient } from '@delon/theme';
import { NzTreeNodeOptions, NzTreeNode } from 'ng-zorro-antd';
import { List } from 'linqts';
import { Observable } from 'rxjs';
import { SFSchemaEnumType, SFSchema } from '@delon/form';
import { STColumn, STColumnTag, STReq, STRes, STPage, STRequestOptions, STData } from '@delon/abc';
import { FilterRule, FilterOperate, PageRequest, PageCondition, SortCondition, ListSortDirection } from '../osharp.model';
import { OsharpSTColumn } from './alain.types';

@Injectable({
  providedIn: 'root',
})
export class AlainService {

  constructor(private osharp: OsharpService) { }

  public get http(): _HttpClient {
    return this.osharp.http;
  }

  AjaxResultTypeTags: STColumnTag = {
    200: { text: '成功', color: 'green' },
    203: { text: '消息', color: '' },
    401: { text: '未登录', color: 'orange' },
    403: { text: '无权操作', color: 'orange' },
    404: { text: '不存在', color: 'orange' },
    423: { text: '锁定', color: 'orange' },
    500: { text: '错误', color: 'red' },
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
  PackLevelTags: STColumnTag = {
    1: { text: 'Core', color: 'red' },
    10: { text: 'Framework', color: 'orange' },
    20: { text: 'Application', color: 'blue' },
    30: { text: 'Business', color: 'green' },
  };

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
      let item: NzTreeNodeOptions = {
        title: node.Name,
        key: node.Id,
        checked: node.IsChecked,
        isLeaf: !node.HasChildren,
      };
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

  ReadNode(
    url: string,
    labelName: string,
    valueName: string,
  ): Observable<SFSchemaEnumType[]> {
    return this.http.get(url).map((nodes: any[]) => {
      let items: SFSchemaEnumType[] = [];
      for (const node of nodes) {
        let item: SFSchemaEnumType = {
          label: node[labelName],
          value: node[valueName],
        };
        items.push(item);
      }
      return items;
    });
  }

  ToSFSchema(column: OsharpSTColumn): SFSchema {
    let schemaProps = ['enum', 'minimum', 'exclusiveMinimum', 'maximum', 'exclusiveMaximum', 'multipleOf', 'maxLength', 'minLength', 'pattern', 'items', 'minItems',
      'maxItems', 'uniqueItems', 'additionalItems', 'maxProperties', 'minProperties', 'required', 'properties', 'if', 'then', 'else', 'allOf', 'anyOf', 'oneOf',
      'description', 'readOnly', 'definitions', '$ref', '$comment', 'ui'];
    let specialProps = ['ftype', 'fformat', 'fdefault', 'ftitle'];
    let schema: SFSchema = {};
    let keys = Object.getOwnPropertyNames(column);
    for (const key of keys) {
      if (schemaProps.includes(key)) {
        schema[key] = column[key];
      } else if (specialProps.includes(key)) {
        // fkey的情况，直接使用fkey的值
        let ekey = key.substr(1);
        schema[ekey] = column[key];
      } else if (specialProps.includes(`f${key}`)) {
        // key的情况，要取key值来转换才能用
        switch (key) {
          case 'default':
          case 'title':
            schema[key] = column[key];
            break;
          case 'type':
            {
              let val = column[key];
              if (['link', 'img'].includes(val)) {
                schema[key] = 'string';
              } else if (['number', 'currency'].includes(val)) {
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
    if (!keys.includes('type') && !keys.includes('ftype')) {
      schema.type = 'string';
    }
    return schema;
  }

  changeFilterRuleType(rule: FilterRule, column: STColumn) {
    let type = column.type || column.ftype || 'string';
    rule.control = 'string';
    switch (type) {
      case 'tag':
      case 'number':
        if (column.enum || column.tag) {
          rule.entries = this.osharp.getOperateEntries([
            FilterOperate.Equal,
            FilterOperate.NotEqual,
          ]);
          rule.Operate = rule.entries[0].Operate;
          rule.control = 'enum';
          rule.enums = column.enum || this.TagsToEnums(column.tag);
          rule.Value = rule.enums[0].value;
        } else {
          rule.entries = this.osharp.getOperateEntries([
            FilterOperate.Equal,
            FilterOperate.NotEqual,
            FilterOperate.Less,
            FilterOperate.LessOrEqual,
            FilterOperate.Greater,
            FilterOperate.GreaterOrEqual,
          ]);
          rule.Operate = rule.entries[0].Operate;
          rule.Value = 0;
          rule.control = 'number';
        }
        break;
      case 'date':
        rule.entries = this.osharp.getOperateEntries([
          FilterOperate.Equal,
          FilterOperate.NotEqual,
          FilterOperate.Less,
          FilterOperate.LessOrEqual,
          FilterOperate.Greater,
          FilterOperate.GreaterOrEqual,
        ]);
        rule.Operate = rule.entries[0].Operate;
        rule.control = 'date';
        rule.Value = null;
        break;
      case 'yn':
      case 'boolean':
        rule.entries = this.osharp.getOperateEntries([
          FilterOperate.Equal,
          FilterOperate.NotEqual,
        ]);
        rule.Operate = rule.entries[0].Operate;
        rule.control = 'boolean';
        rule.Value = false;
        break;
      default:
        rule.entries = this.osharp.getOperateEntries([
          FilterOperate.Equal,
          FilterOperate.NotEqual,
          FilterOperate.StartsWith,
          FilterOperate.EndsWith,
          FilterOperate.Contains,
          FilterOperate.NotContains,
        ]);
        rule.Operate = FilterOperate.Contains;
        rule.control = 'string';
        rule.Value = null;
        break;
    }
  }

  TagsToEnums(tag: STColumnTag): SFSchemaEnumType[] {
    let enums: SFSchemaEnumType[] = [];
    for (const key in tag) {
      if (tag.hasOwnProperty(key)) {
        const value = tag[key];
        enums.push({ value: key, label: value.text });
      }
    }
    return enums;
  }

  GetSTReq(request: PageRequest, process?: (requestOptions: STRequestOptions) => STRequestOptions): STReq {
    let req: STReq = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: request,
      allInBody: true,
      process: process ? process : null,
    };
    return req;
  }

  GetSTRes(process?: (data: STData[], rawData?: any) => STData[]): STRes {
    let res: STRes = {
      reName: { list: 'Rows', total: 'Total' },
      process: process ? process : null
    };
    return res;
  }

  GetSTPage(): STPage {
    let page: STPage = {
      showSize: true,
      showQuickJumper: true,
      toTop: true,
      toTopOffset: 0,
    };
    return page;
  }

  RequestProcess(opt: STRequestOptions, fieldReplaceFunc?: (f: string) => string): STRequestOptions {
    if (opt.body.PageCondition) {
      let page: PageCondition = opt.body.PageCondition;
      page.PageIndex = opt.body.pi;
      page.PageSize = opt.body.ps;
      if (opt.body.sort) {
        page.SortConditions = [];
        let sorts: string[] = opt.body.sort.split('-');
        for (const item of sorts) {
          let sort = new SortCondition();
          let num = item.lastIndexOf('.');
          let field = item.substr(0, num);
          if (fieldReplaceFunc) {
            field = fieldReplaceFunc(field);
          }
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
}
