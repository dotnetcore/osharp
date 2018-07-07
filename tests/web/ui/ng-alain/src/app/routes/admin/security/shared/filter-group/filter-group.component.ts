import { Component, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Group, Rule, FilterOperate, AjaxResult, AjaxResultType, EntityProperty } from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';


@Component({
  selector: 'security-filter-group',
  templateUrl: './filter-group.component.html',
  styles: [`
  .group-box{margin:5px;padding:5px; border:dashed 2px #ddd;}
  `]
})
export class FilterGroupComponent implements OnChanges {

  @Input() group: Group;
  @Input() entity: string;
  @Output() remove: EventEmitter<Group> = new EventEmitter<Group>();

  entityProperties: EntityProperty[] = [];

  constructor(
    private osharp: OsharpService,
    private http: HttpClient
  ) {
    this.group = new Group();
    this.group.Operate = FilterOperate.Or;
  }

  ngOnChanges() {
    if (this.group && (!this.group.Level || this.group.Level == 1)) {
      Group.Init(this.group);
      this.http.get<AjaxResult>("api/admin/entityinfo/ReadProperties?typeName=" + this.entity).subscribe(res => {
        if (res.Type != AjaxResultType.Success) {
          this.osharp.error(res.Content);
          return;
        }
        this.entityProperties = res.Data;
        console.log(this.entityProperties);
      });
    }
  }
  addGroup() {
    if (!this.entity) {
      this.osharp.error("请选择左边的一行再进行操作");
      return;
    }
    let subGroup = new Group();
    subGroup.Level = this.group.Level + 1;
    this.group.Groups.push(subGroup);
  }
  addRule() {
    if (!this.entity) {
      this.osharp.error("请选择左边的一行再进行操作");
      return;
    }
    this.group.Rules.push(new Rule(null, null));
  }
  removeRule(rule: Rule) {
    if (rule) {
      let index = this.group.Rules.indexOf(rule);
      if (index >= 0) {
        this.group.Rules.splice(index, 1);
      }
    }
  }
  /** 删除分组按钮 */
  removeGroup() {
    this.remove.emit(this.group);
  }
  /**删除子分组的事件响应 */
  removeSubGroup(subGroup: Group) {
    if (subGroup) {
      let index = this.group.Groups.indexOf(subGroup);
      if (index >= 0) {
        this.group.Groups.splice(index, 1);
      }
    }
  }
}
