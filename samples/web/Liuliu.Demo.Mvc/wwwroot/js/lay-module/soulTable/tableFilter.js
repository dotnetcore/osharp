/**
 *
 * @name:  表格筛选扩展
 * @author: yelog
 * @link: https://github.com/yelog/layui-soul-table
 * @license: MIT
 * @version: v1.5.16
 */
layui.define(['table', 'form', 'laydate', 'util', 'excel', 'laytpl'], function (exports) {

  var $ = layui.jquery,
    table = layui.table,
    form = layui.form,
    laydate = layui.laydate,
    laytpl = layui.laytpl,
    util = layui.util,
    excel = layui.excel,
    columnsTimeOut,
    dorpListTimeOut,
    conditionTimeOut,
    bfColumnTimeOut,
    bfCond1TimeOut,
    isFilterReload = {},
    SOUL_ROW_INDEX = 'SOUL_ROW_INDEX',
    cache = {},
    HIDE = 'layui-hide',
    maxId = 1,
    UNHANDLED_VALUES = [undefined, '', null],
    where_cache = {},
    isFilterCache = {},
    table_cache = {},
    conditionChangeItems = {
      'eq': '等于',
      'ne': '≠ 不等于',
      'gt': '> 大于',
      'ge': '≥ 大于等于',
      'lt': '< 小于',
      'le': '≤ 小于等于',
      'contain': '包含',
      'notContain': '不包含',
      'start': '以...开头',
      'end': '以...结尾',
      'null': '为空',
      'notNull': '不为空'
    },
    dateTimeItems = {
      'all': '全部',
      'yesterday': '昨天',
      'thisWeek': '本周',
      'lastWeek': '上周',
      'thisMonth': '本月',
      'thisYear': '今年'
    },
    defaultFilterItems = ['column', 'data', 'condition', 'editCondition', 'excel'],
    itemsMap = {
      'column': 'soul-column',
      'data': 'soul-dropList',
      'condition': 'soul-condition',
      'editCondition': 'soul-edit-condition',
      'excel': 'soul-export',
      'clearCache': 'soul-clear-cache',
    },
    modeMapItems = {
      'in': 'data',
      'condition': 'condition',
      'date': 'condition',
    },
    revertMode = {
      'data': {
        'mode': 'condition',
        'type': 'eq',
        'value': '',
      },
      'condition': {
        'mode': 'in',
        'values': [],
      },
    };

  // 封装方法
  var mod = {
    /**
     * 摧毁render数据
     * @param myTables
     */
    destroy: function (myTables) {
      if (myTables) {
        if (Array.isArray(myTables)) {
          for (var i = 0; i < myTables.length; i++) {
            deleteRender(myTables[i]);
          }
        } else {
          deleteRender(myTables);
        }
      }

      function deleteRender(myTable) {
        if (!myTable) {
          return;
        }
        var tableId = myTable.config.id;
        $('#soul-filter-list' + tableId).remove();
        $('#soulCondition' + tableId).remove();
        $('#soulDropList' + tableId).remove();

        delete isFilterReload[tableId];
        delete where_cache[tableId];
        delete table_cache[tableId];
      }
    },
    clearFilter: function (myTable) {
      if (typeof myTable === 'string') {
        myTable = table_cache[myTable];
      }
      if (!where_cache[myTable.id] || !where_cache[myTable.id].filterSos || where_cache[myTable.id].filterSos === "[]") {
        return;
      }
      where_cache[myTable.id].filterSos = "[]";
      this.soulReload(myTable, true);
      if (table_cache[myTable.id].where && table_cache[myTable.id].where.filterSos && table_cache[myTable.id].where.filterSos !== "[]") {
        table_cache[myTable.id].where.filterSos = "[]";
      }
    },
    render: function (myTable) {
      var _this = this,
        $table = $(myTable.elem),
        $tableMain = $table.next().children('.layui-table-box').children('.layui-table-main'),
        $tableHead = $table.next().children('.layui-table-box').children('.layui-table-header').children('table'),
        $fixedLeftTableHead = $table.next().children('.layui-table-box').children('.layui-table-fixed-l').children('.layui-table-header').children('table'),
        $fixedRigthTableHead = $table.next().children('.layui-table-box').children('.layui-table-fixed-r').children('.layui-table-header').children('table'),
        tableId = myTable.id,
        columns = _this.getCompleteCols(myTable.cols),
        filterItems = myTable.filter ? myTable.filter.items || defaultFilterItems : defaultFilterItems,
        needFilter = false, // 是否存在筛选列需要进行初始化
        initFilter = false, // 是否为第一次筛选
        mainExcel = typeof myTable.excel === 'undefined' || ((myTable.excel && (typeof myTable.excel.on === 'undefined' || myTable.excel.on)) ? myTable.excel : false),
        i, j;

      for (i = 0; i < columns.length; i++) {
        if (columns[i].field && columns[i].filter) {
          needFilter = true;
          if ($tableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.soul-table-filter').length === 0) {
            initFilter = true;
            if ($tableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').length > 0) {
              $tableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').hide();
              $tableHead.find('th[data-field="' + columns[i].field + '"]').children().append('<span class="layui-table-sort soul-table-filter layui-inline" data-items="' + (columns[i].filter.items ? columns[i].filter.items.join(',') : '') + '" data-column="' + columns[i].field + '" lay-sort="' + $tableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').attr('lay-sort') + '" ' + (typeof columns[i].filter.split === 'undefined' ? '' : 'data-split="' + columns[i].filter.split + '"') + '><i class="soul-icon soul-icon-filter"></i><i class="soul-icon soul-icon-filter-asc"></i><i class="soul-icon soul-icon-filter-desc"></i></span>');
            } else {
              $tableHead.find('th[data-field="' + columns[i].field + '"]').children().append('<span class="soul-table-filter layui-inline" data-items="' + (columns[i].filter.items ? columns[i].filter.items.join(',') : '') + '" data-column="' + columns[i].field + '" ' + (typeof columns[i].filter.split === 'undefined' ? '' : 'data-split="' + columns[i].filter.split + '"') + '><i class="soul-icon soul-icon-filter"></i><i class="soul-icon soul-icon-filter-asc"></i><i class="soul-icon soul-icon-filter-desc"></i></span>');
            }
            if ($fixedLeftTableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').length > 0) {
              $fixedLeftTableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').hide();
              $fixedLeftTableHead.find('th[data-field="' + columns[i].field + '"]').children().append('<span class="layui-table-sort soul-table-filter layui-inline" data-items="' + (columns[i].filter.items ? columns[i].filter.items.join(',') : '') + '" data-column="' + columns[i].field + '" lay-sort="' + $fixedLeftTableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').attr('lay-sort') + '" ' + (typeof columns[i].filter.split === 'undefined' ? '' : 'data-split="' + columns[i].filter.split + '"') + '><i class="soul-icon soul-icon-filter"></i><i class="soul-icon soul-icon-filter-asc"></i><i class="soul-icon soul-icon-filter-desc"></i></span>');
            } else {
              $fixedLeftTableHead.find('th[data-field="' + columns[i].field + '"]').children().append('<span class="soul-table-filter layui-inline" data-items="' + (columns[i].filter.items ? columns[i].filter.items.join(',') : '') + '" data-column="' + columns[i].field + '" ' + (typeof columns[i].filter.split === 'undefined' ? '' : 'data-split="' + columns[i].filter.split + '"') + '><i class="soul-icon soul-icon-filter"></i><i class="soul-icon soul-icon-filter-asc"></i><i class="soul-icon soul-icon-filter-desc"></i></span>');
            }
            if ($fixedRigthTableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').length > 0) {
              $fixedRigthTableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').hide();
              $fixedRigthTableHead.find('th[data-field="' + columns[i].field + '"]').children().append('<span class="layui-table-sort soul-table-filter layui-inline" data-items="' + (columns[i].filter.items ? columns[i].filter.items.join(',') : '') + '" data-column="' + columns[i].field + '" lay-sort="' + $fixedRigthTableHead.find('th[data-field="' + columns[i].field + '"]').children().children('.layui-table-sort').attr('lay-sort') + '" ' + (typeof columns[i].filter.split === 'undefined' ? '' : 'data-split="' + columns[i].filter.split + '"') + '><i class="soul-icon soul-icon-filter"></i><i class="soul-icon soul-icon-filter-asc"></i><i class="soul-icon soul-icon-filter-desc"></i></span>');
            } else {
              $fixedRigthTableHead.find('th[data-field="' + columns[i].field + '"]').children().append('<span class="soul-table-filter layui-inline" data-items="' + (columns[i].filter.items ? columns[i].filter.items.join(',') : '') + '" data-column="' + columns[i].field + '" ' + (typeof columns[i].filter.split === 'undefined' ? '' : 'data-split="' + columns[i].filter.split + '"') + '><i class="soul-icon soul-icon-filter"></i><i class="soul-icon soul-icon-filter-asc"></i><i class="soul-icon soul-icon-filter-desc"></i></span>');
            }
          }
        }
      }
      table_cache[myTable.id] = myTable; // 缓存table配置
      isFilterCache[myTable.id] = needFilter;
      if (!needFilter) {
        return;
      } //如果没筛选列，直接退出

      // 渲染底部筛选条件
      if (!(myTable.filter && typeof myTable.filter.bottom !== 'undefined' && !myTable.filter.bottom) && $table.next().children('.soul-bottom-contion').length === 0) {
        $table.next().children('.layui-table-box').after('<div class="soul-bottom-contion"><div class="condition-items"></div><div class="editCondtion"><a class="layui-btn layui-btn-primary">编辑筛选条件</a></div></div>');
        var changeHeight = $table.next().children('.layui-table-box').children('.layui-table-body').outerHeight() - $table.next().children('.soul-bottom-contion').outerHeight();
        if (myTable.page && $table.next().children('.layui-table-page').hasClass('layui-hide')) {
          changeHeight += $table.next().children('.layui-table-page').outerHeight();
        }
        $table.next().children('.layui-table-box').children('.layui-table-body').css('height', changeHeight);
        var fixHeight = changeHeight - _this.getScrollWidth($tableMain[0]),
          layMainTableHeight = $tableMain.children('table').height();
        $table.next().children('.layui-table-box').children('.layui-table-fixed').children('.layui-table-body').css('height', layMainTableHeight >= fixHeight ? fixHeight : 'auto');
        $table.next().children('.soul-bottom-contion').children('.condition-items').css('width', ($table.next().children('.soul-bottom-contion').width() - $table.next().children('.soul-bottom-contion').children('.editCondtion').width()) + 'px');
        $table.next().children('.soul-bottom-contion').children('.editCondtion').children('a').on('click', function () {
          _this.showConditionBoard(myTable);
        });
      }

      /**
       * 不重载表头数据，重新绑定事件后结束
       */
      if (!initFilter || isFilterReload[myTable.id] || myTable.isSoulFrontFilter) {
        isFilterReload[myTable.id] = false;
        myTable['isSoulFrontFilter'] = false;
        // 同步选中状态
        if (!myTable.url && myTable.page && myTable.data) {
          myTable.data.forEach(function (row) {
            cache[myTable.id][row[SOUL_ROW_INDEX]] = row;
          });
        }
        this.bindFilterClick(myTable);
        return;
      } else {
        if (!myTable.url && myTable.page && myTable.data && myTable.data.length > myTable.limit) {
          // 前端分页大于一页，修复 index （用于排序恢复时需要通过这个排序）
          layui.each(myTable.data, function (index, item) {
            item[myTable.indexName] = index;
          });
        }
        /**
         * 缓存所有数据
         */
        if (myTable.url && !myTable.page) {
          // 修复不分页时，前端筛选后，data不为空，造成所有数据丢失的问题
          cache[myTable.id] = layui.table.cache[myTable.id];
        } else {
          cache[myTable.id] = myTable.data || layui.table.cache[myTable.id];
        }
        // 给表格数据添加位置标志
        cache[myTable.id].forEach(function (item, index) {
          item[SOUL_ROW_INDEX] = index;
        });

        if (myTable.filter && myTable.filter.clearFilter) {
          if (myTable.where && myTable.where.filterSos && JSON.parse(myTable.where.filterSos).length > 0) {
            // 重新查询新数据
            myTable.where.filterSos = '[]';
            where_cache[myTable.id] = myTable.where || {};
            _this.soulReload(myTable, false);
            return;
          } else {
            where_cache[myTable.id] = myTable.where || {};
          }
        } else if ((typeof myTable.url !== 'undefined' && myTable.page ? typeof myTable.where.filterSos === 'undefined' : true) && where_cache[myTable.id] && JSON.parse(where_cache[myTable.id].filterSos || '[]').length > 0) {
          myTable.where['filterSos'] = where_cache[myTable.id].filterSos;
          where_cache[myTable.id] = myTable.where;
          _this.soulReload(myTable, false);
          return;
        } else {
          where_cache[myTable.id] = myTable.where || {};
        }
      }

      // 第一次渲染时，追加数据
      if ($('#soul-filter-list' + tableId).length === 0) {

        if (typeof myTable.soulSort === 'undefined' || myTable.soulSort) {
          if (typeof $table.attr('lay-filter') === 'undefined') {
            $table.attr('lay-filter', tableId);
          }
          table.on('sort(' + $table.attr('lay-filter') + ')', function (obj) {

            // 同步分页信息
            myTable.limit = table_cache[myTable.id].limit;

            if (myTable.url && myTable.page) {
              // 后台分页
              where_cache[myTable.id].field = obj.field;
              where_cache[myTable.id].order = obj.type;
              isFilterReload[myTable.id] = true;
              table.render($.extend(myTable, {
                initSort: obj
                , where: where_cache[myTable.id]
                , page: {
                  curr: 1 //重新从第 1 页开始
                }
              }));
            } else if (!myTable.url && myTable.page) {
              // 前台分页
              if (obj.type === 'asc') { //升序
                cache[myTable.id] = layui.sort(cache[myTable.id], obj.field);
              } else if (obj.type === 'desc') { //降序
                cache[myTable.id] = layui.sort(cache[myTable.id], obj.field, true);
              } else { //清除排序
                cache[myTable.id] = layui.sort(cache[myTable.id], myTable.indexName);
              }
              myTable.initSort = obj;
              myTable.page = {curr: 1};
              _this.soulReload(myTable, false);
            }
          });
        }

        var soulFilterList = [],
          filterItemsHtml = {
            column: '<li class="soul-column"><i class="layui-icon layui-icon-table"></i> 表格列 <i class="layui-icon layui-icon-right" style="float: right"></i></li>',
            data: '<li class="soul-dropList"><i class="soul-icon soul-icon-drop-list"></i> 筛选数据 <i class="layui-icon layui-icon-right" style="float: right"></i></li>',
            condition: '<li class="soul-condition"><i class="soul-icon soul-icon-query"></i> 筛选条件 <i class="layui-icon layui-icon-right" style="float: right"></i></li>',
            editCondition: '<li class="soul-edit-condition"><i class="layui-icon layui-icon-edit"></i> 编辑筛选条件 </li>',
            excel: '<li class="soul-export"><i class="soul-icon soul-icon-download"></i> 导出excel </li>',
            clearCache: '<li class="soul-clear-cache"><i class="layui-icon layui-icon-delete"></i> 清除缓存 </li>'
          };
        soulFilterList.push('<div id="soul-filter-list' + tableId + '"><form action="" class="layui-form" lay-filter="orm"><ul id="main-list' + tableId + '" style="display: none">');
        soulFilterList.push('<li class="soul-sort" data-value="asc" ><i class="soul-icon soul-icon-asc"></i> 升序排列 </li>');
        soulFilterList.push('<li class="soul-sort" data-value="desc"  style="border-bottom: 1px solid #e6e6e6"><i class="soul-icon soul-icon-desc"></i> 降序排列 </li>');
        for (i = 0; i < defaultFilterItems.length; i++) {
          if (defaultFilterItems[i] === 'excel' && !mainExcel) {
            continue;
          }
          soulFilterList.push(filterItemsHtml[defaultFilterItems[i]]);
        }
        soulFilterList.push('</ul><ul id="soul-columns' + tableId + '" style="display: none;">');

        var types = {}; //存储过滤数据的类型
        // 根据表格列显示
        for (i = 0; i < columns.length; i++) {
          if (columns[i].type === 'checkbox' || !columns[i].field) {
            soulFilterList.push('<li class="layui-hide"><input type="checkbox" title="' + columns[i].title + '" /></li>');
            continue;
          }
          soulFilterList.push('<li data-value="' + columns[i].field + '" data-key="' + i + '"><input type="checkbox" value="' + (myTable.index + '-' + columns[i].key) + '" title="' + columns[i].title + '" data-fixed="' + (columns[i].fixed || "") + '" lay-skin="primary" lay-filter="changeColumns' + tableId + '" ' + (columns[i].hide ? '' : 'checked') + '></li>');

          //存储过滤数据的类型
          if (columns[i].filter && columns[i].filter.type) {
            if (columns[i].filter.field) {
              types[columns[i].filter.field] = columns[i].filter.type;
            } else {
              types[columns[i].field] = columns[i].filter.type;
            }
          }
        }
        if (JSON.stringify(types).length !== 2) {
          myTable.where['tableFilterType'] = JSON.stringify(types);
        }

        soulFilterList.push('</ul><div id="soul-dropList' + tableId + '" style="display: none"><div class="filter-search"><input type="text" placeholder="关键字搜索" class="layui-input"></div><div class="check"><div class="multiOption" data-type="all"><i class="soul-icon">&#xe623;</i> 全选</div><div class="multiOption" data-type="none"><i class="soul-icon">&#xe63e;</i> 清空</div><div class="multiOption" data-type="reverse"><i class="soul-icon">&#xe614;</i>反选</div></div><ul></ul></div>');
        soulFilterList.push('<ul id="soul-condition' + tableId + '" style="display: none;"></ul></form></div>');
        $('body').append(soulFilterList.join(''));


        // 显示隐藏列
        var liClick = true;
        form.on('checkbox(changeColumns' + tableId + ')', function (data) {
          liClick = false;
          var columnkey = data.value;
          if (data.elem.checked) {
            $table.next().find('[data-key=' + columnkey + ']').removeClass(HIDE);
          } else {
            $table.next().find('[data-key=' + columnkey + ']').addClass(HIDE);
          }
          // 同步配置
          var tempColumns = [].concat.apply([], myTable.cols);
          for (i = 0; i < tempColumns.length; i++) {
            if (tempColumns[i].field && tempColumns[i].field === columnkey) {
              tempColumns[i]['hide'] = !data.elem.checked;
            }
          }
          if (layui.soulTable) {
            layui.soulTable.fixTableRemember(myTable);
          }
          $table.next().children('.layui-table-box').children('.layui-table-body').children('table').children('tbody').children('tr.childTr').children('td').attr('colspan', $table.next().children('.layui-table-box').children('.layui-table-header').find('thead>tr>th:visible').length);
          table.resize(tableId);
        });
        $('#soul-columns' + tableId + '>li[data-value]').on('click', function () {
          if (!$(this).find(':checkbox').is(':disabled')) { //disabled禁止点击
            if (liClick) {
              $(this).find('div.layui-form-checkbox').trigger('click');
            }
            liClick = true;
          }
        });

        // 全选-反选事件
        $('#soul-dropList' + tableId + ' .check [data-type]').on('click', function () {

          switch ($(this).data('type')) {
            case 'all':
              $(this).parents('#soul-dropList' + tableId).find('input[type=checkbox]:not(:checked)').prop('checked', true);
              break;
            case 'reverse':
              $(this).parents('#soul-dropList' + tableId).find('input[type=checkbox]').each(function () {
                $(this).prop('checked', !$(this).prop('checked'));
              });
              break;
            case 'none':
              $(this).parents('#soul-dropList' + tableId).find('input[type=checkbox]:checked').prop('checked', false);
              break;
          }
          form.render('checkbox', 'orm');
          _this.updateDropList(myTable, $('#main-list' + tableId).data('field'));
          return false;
        });

        // 关键字搜索
        $('#soul-dropList' + tableId + ' .filter-search input').on('input', function () {
          var key = $(this).val();
          if (key === '') {
            $('#soul-dropList' + tableId + '>ul>li').show();
          } else {
            $('#soul-dropList' + tableId + '>ul>li').hide();
            $('#soul-dropList' + tableId + '>ul>li[data-value*="' + key.toLowerCase() + '"]').show();
          }
        });

        // 显示表格列
        $('#main-list' + tableId + ' .soul-column').on('mouseover', function (e) {
          _this.hideDropList(myTable);
          _this.hideCondition(myTable);
          e.stopPropagation();
          if (columnsTimeOut) {
            clearTimeout(columnsTimeOut);
          }
          $('#soul-columns' + tableId).show();
          var left, animate;
          if ($(this).parent().offset().left + $(this).parent().width() + $('#soul-columns' + tableId).width() < document.body.clientWidth) {
            left = $(this).parent().offset().left + $(this).parent().width();
            animate = 'fadeInLeft';
          } else {
            left = $(this).parent().offset().left - $('#soul-columns' + tableId).width();
            animate = 'fadeInRight';
          }
          $('#soul-columns' + tableId).css({'top': $(this).offset().top, 'left': left})
            .removeClass().addClass(animate + ' animated');
        });
        // 显示数据下拉
        $('#main-list' + tableId + ' .soul-dropList').on('mouseover', function (e) {
          if ($('#soul-dropList' + tableId).is(':visible') && !$('#soul-dropList' + tableId).hasClass('fadeOutLeft')) {
            return false;
          }
          _this.hideColumns(myTable);
          _this.hideCondition(myTable);
          e.stopPropagation();
          if (dorpListTimeOut) {
            clearTimeout(dorpListTimeOut);
          }
          $('#soul-dropList' + tableId + '>.filter-search>input').val('');
          $('#soul-dropList' + tableId).show();
          var left, animate, field = $('#main-list' + tableId).data('field');
          if ($('#main-list' + tableId).offset().left + $('#soul-dropList' + tableId).width() + $('#soul-dropList' + tableId).width() < document.body.clientWidth) {
            left = $('#main-list' + tableId).offset().left + $('#main-list' + tableId).width();
            animate = 'fadeInLeft';
          } else {
            left = $('#main-list' + tableId).offset().left - $('#soul-dropList' + tableId).width();
            animate = 'fadeInRight';
          }

          $('#soulDropList' + tableId).find('.' + field + 'DropList li input[type=checkbox]:checked').prop('checked', false);
          var where = where_cache[myTable.id] || {},
            filterSos = JSON.parse(where.filterSos ? where.filterSos : null),
            id = '', prefix = '';
          if (filterSos) {
            for (i = 0; i < filterSos.length; i++) {
              if (filterSos[i].head && filterSos[i].mode === "in" && filterSos[i].field === field) {
                id = filterSos[i].id;
                prefix = filterSos[i].prefix;
                for (j = 0; j < filterSos[i].values.length; j++) {
                  $('#soulDropList' + tableId).find('.' + field + 'DropList li input[type=checkbox][value="' + filterSos[i].values[j] + '"]').prop('checked', true);
                }
                break;
              }
            }
          }
          $('#soul-dropList' + tableId + '>ul').data({
            head: true,
            'id': id,
            prefix: prefix,
            refresh: true,
            split: $('#main-list' + tableId).data('split')
          }).html($('#soulDropList' + tableId).find('.' + field + 'DropList li').clone());

          $('#soul-dropList' + tableId).css({'top': $(this).offset().top, 'left': left})
            .show().removeClass().addClass(animate + ' animated');
          setTimeout(function () {
            $('#soul-dropList' + tableId + '>.filter-search>input').focus(); // 聚焦搜索框
            form.render('checkbox', 'orm');
          }, 1);

          // 监听筛选数据
          var liClick = true;
          form.on('checkbox(soulDropList' + tableId + ')', function (data) {
            liClick = false;
            _this.updateDropList(myTable, field);
          });

          $('#soul-dropList' + tableId + '>ul>li[data-value]').on('click', function () {
            if (liClick) {
              $(this).find('div.layui-form-checkbox').trigger('click');
            }
            liClick = true;
          });
        });

        // 显示筛选条件
        $('#main-list' + tableId + ' .soul-condition').on('mouseover', function (e) {
          if ($('#soul-condition' + tableId).is(':visible') && !$('#soul-condition' + tableId).hasClass('fadeOutLeft')) {
            return false;
          }
          _this.hideColumns(myTable);
          _this.hideDropList(myTable);
          e.stopPropagation();
          if (conditionTimeOut) {
            clearTimeout(conditionTimeOut);
          }
          var documentWidth = document.body.clientWidth;
          $('#soul-condition' + tableId).show();
          var left, animate, field = $(this).parent().data('field');
          if ($(this).parent().offset().left + $(this).parent().width() + $('#soul-condition' + tableId).width() < documentWidth) {
            left = $(this).parent().offset().left + $(this).parent().width();
            animate = 'fadeInLeft';
          } else {
            left = $(this).parent().offset().left - $('#soul-condition' + tableId).width();
            animate = 'fadeInRight';
          }

          var filterSo, conditionHtml = [],
            where = where_cache[myTable.id] || {},
            filterSos = JSON.parse(where.filterSos ? where.filterSos : null);
          if (filterSos) {
            for (i = 0; i < filterSos.length; i++) {
              if (filterSos[i].head && filterSos[i].field === field && (filterSos[i].mode === "date" || filterSos[i].mode === 'group')) {
                filterSo = filterSos[i];
                break;
              }
            }
          }

          var filterType = $(this).parent().data('type');
          if (_this.startsWith(filterType, 'date')) {
            _this.showDate(myTable, field, filterSo, animate, $(this).offset().top, $(this).parent().offset().left + $(this).parent().width(), 'down', true);
          } else {
            /**
             * 筛选条件
             */
            var fieldMap = {};
            for (i = 0; i < columns.length; i++) {
              if (columns[i].field) {
                fieldMap[columns[i]['field']] = columns[i];
              }
            }
            // 查询条件
            var selectStr = "<select lay-filter='conditionChange'>";
            for (var key in conditionChangeItems) {
              selectStr += '<option value="' + key + '">' + conditionChangeItems[key] + '</option>';
            }
            selectStr += "</select>";
            conditionHtml.push('<table class="condition-table"><tbody>');
            if (filterSo && filterSo.children && filterSo.children.length > 0) {
              for (i = 0; i < filterSo.children.length; i++) {
                var id = filterSo.children[i].id,
                  prefix = filterSo.children[i].prefix,
                  type = filterSo.children[i].type,
                  value = filterSo.children[i].value;
                conditionHtml.push('<tr data-id="' + id + '">');
                if (i === 0) {
                  conditionHtml.push('<td class="soul-condition-title">' + fieldMap[field].title + '</td>');
                } else {
                  conditionHtml.push(
                    '<td>' +
                    '   <div>' +
                    '      <input type="checkbox" name="switch" lay-filter="soul-coondition-switch" lay-skin="switch" lay-text="与|或" ' + (!prefix || prefix === 'and' ? 'checked' : '') + '>' +
                    '    </div>' +
                    '</td>');
                }
                conditionHtml.push('<td style="width: 110px;"><div class="layui-block" ><select lay-filter="conditionChange">');
                for (var key in conditionChangeItems) {
                  conditionHtml.push('<option value="' + key + '" ' + (key === type ? 'selected' : '') + '>' + conditionChangeItems[key] + '</option>');
                }
                conditionHtml.push('</select></div></td>');
                conditionHtml.push('<td style="width: 110px;"><div class="layui-block" ><input class="layui-input value" value="' + (value || '') + '" placeholder="值" /></div></td>');
                conditionHtml.push('<td><i class="layui-icon layui-icon-delete del" style="font-size: 23px; color: #FF5722; cursor: pointer"></i></td>');
                conditionHtml.push('</tr>');
              }
            } else {
              conditionHtml.push('<tr data-id="" data-type="eq" data-value="">'
                + '<td class="soul-condition-title">' + fieldMap[field].title + '</td>'
                + '<td style="width: 110px;"><div class="layui-block" >' + selectStr
                + '</div></td>'
                + '<td style="width: 110px;"><div class="layui-block" ><input class="layui-input value" placeholder="值" /></div></td>'
                + '<td><i class="layui-icon layui-icon-delete del" style="font-size: 23px; color: #FF5722; cursor: pointer"></i></td>'
                + '</tr>');
            }
            conditionHtml.push('</tbody></table><div style="text-align: center; padding-top: 5px"><button class="layui-btn layui-btn-sm" data-type="add"><i class="layui-icon">&#xe654;</i>添加</button><span style="display: inline-block;width: 50px"></span><button class="layui-btn layui-btn-sm" data-type="search"><i class="layui-icon">&#xe615;</i>查询</button></div>');

            $('#soul-condition' + tableId).data({head: true, id: filterSo ? filterSo.id || '' : ''})
              .html(conditionHtml.join(''))
              .css({'top': $(this).offset().top, 'left': left})
              .show().removeClass().addClass(animate + ' animated');

            $('.condition-table').on('click', function () {
              return false;
            });

            // 新增与查询
            $('#soul-condition' + tableId + ' button[data-type]').on('click', function () {
              /**
               * 新增
               */
              if ($(this).data('type') === 'add') {
                var groupId = $('#soul-condition' + tableId).data('id'),
                  head = $('#soul-condition' + tableId).data('head'),
                  type = 'eq',
                  filterSo,
                  $tr1 = $('#soul-condition' + tableId).find('tr:eq(0)');

                if (groupId) {
                  filterSo = {
                    head: head,
                    prefix: 'and',
                    field: field,
                    mode: 'condition',
                    type: type,
                    value: '',
                    groupId: groupId
                  };
                } else {
                  filterSo = {
                    head: head,
                    prefix: head ? 'and' : 'and',
                    mode: 'group',
                    field: field,
                    children: [{
                      id: _this.getDifId(),
                      prefix: 'and',
                      field: field,
                      mode: 'condition',
                      type: $tr1.find('select').val(),
                      value: $tr1.find('.value').val()
                    }, {
                      id: _this.getDifId(),
                      prefix: 'and',
                      field: field,
                      mode: 'condition',
                      type: type,
                      value: ''
                    }]
                  };
                }

                _this.updateWhere(myTable, filterSo);
                if (!groupId) {
                  $('#soul-condition' + tableId).data('id', filterSo.id);
                  $tr1.data('id', filterSo.children[0].id);
                }
                // $tableHead.find('thead>tr>th[data-field="'+field+'"] .soul-table-filter').attr('soul-filter','true');
                var newId = groupId ? filterSo.id : filterSo.children[1].id;
                var newTr = '<tr data-id="' + newId + '"><td>' +
                  '   <div>' +
                  '      <input type="checkbox" name="switch" lay-filter="soul-coondition-switch" lay-skin="switch" lay-text="与|或" checked>' +
                  '   </div>' +
                  '</td>'
                  + '<td style="width: 110px;"><div class="layui-block">' + selectStr + '</div></td>'
                  + '<td style="width: 110px;"><div class="layui-block"><input class="layui-input value" placeholder="值" /></div></td>'
                  + '<td><i class="layui-icon layui-icon-delete del" style="font-size: 23px; color: #FF5722; cursor: pointer"></i></td></tr>';

                $('#soul-condition' + tableId + ">table>tbody").append(newTr);
                $('#soul-condition' + tableId).find('.del:last').on('click', function () { //删除
                  delCurrentTr(this);
                });

                // input同步筛选条件
                $('#soul-condition' + tableId + ' input.value:last').on('input', function () {
                  updateTrWhere($(this).parents('tr:eq(0)'));
                });
              } else if ($(this).data('type') === 'search') {
                /**
                 * 查询
                 */
                _this.soulReload(myTable);
              }
              form.render('select', 'orm');
              form.render('checkbox', 'orm');
              return false;
            });

            // input同步筛选条件
            $('#soul-condition' + tableId + ' input.value').on('input', function () {
              updateTrWhere($(this).parents('tr:eq(0)'));
            });

            // 当前行改动时，同步where条件
            function updateTrWhere($tr) {
              var id = $tr.data('id'),
                groupId = $('#soul-condition' + tableId).data('id'),
                prefix = $tr.find('input[lay-filter="soul-coondition-switch"]:checked').prop('checked') ? 'and' : 'or',
                type = $tr.find('select').val(),
                value = $tr.find('.value').val(),
                head = $('#soul-condition' + tableId).data('head');

              if (groupId) {
                filterSo = {
                  id: id,
                  prefix: prefix,
                  mode: 'condition',
                  field: field,
                  type: type,
                  value: value,
                  groupId: groupId
                };
              } else {
                filterSo = {
                  head: head,
                  prefix: head ? 'and' : 'and',
                  mode: 'group',
                  field: field,
                  children: [{
                    id: _this.getDifId(),
                    prefix: prefix,
                    mode: 'condition',
                    field: field,
                    type: type,
                    value: value,
                    groupId: groupId
                  }]
                };
              }
              _this.updateWhere(myTable, filterSo);
              if (!groupId) {
                $('#soul-condition' + tableId).data('id', filterSo.id);
                $tr.data('id', filterSo.children[0].id);
              } else if (!id) {
                $tr.data('id', filterSo.id);
              }
            }

            // select同步筛选条件
            form.on('select(conditionChange)', function (data) {
              if (data.value === 'null' || data.value === 'notNull') {
                $(this).parents('tr').find('input.value').hide();
              } else {
                $(this).parents('tr').find('input.value').show();
              }
              updateTrWhere($(data.elem).parents('tr:eq(0)'));
            });

            // radio同步筛选条件
            form.on('switch(soul-coondition-switch)', function (data) {
              updateTrWhere($(this).parents('tr:eq(0)'));
            });

            // 删除当前行
            $('#soul-condition' + tableId + ' .del').on('click', function () {
              delCurrentTr(this);
            });

            function delCurrentTr(obj) {

              var id;

              if ($(obj).parents('table:eq(0)').find('tr').length === 1) {
                id = $('#soul-condition' + tableId).data('id');
                $('#soul-condition' + tableId).data('id', '');
                $(obj).parents('tr:eq(0)').find('select').val('eq');
                $(obj).parents('tr:eq(0)').find('.value').val('').show();
                form.render('select', 'orm');
              } else {
                id = $(obj).parents('tr:eq(0)').data('id');
                if ($(obj).parents('tr:eq(0)').index() === 0) {
                  $(obj).parents('table:eq(0)').find('tr:eq(1)>td:eq(0)').html(fieldMap[field].title).addClass('soul-condition-title');
                }
                $(obj).parents('tr:eq(0)').remove();
              }
              if (id) {
                _this.updateWhere(myTable, {
                  id: id,
                  delete: true
                });
              }
            }
          }
          form.render('select', 'orm');
          form.render('checkbox', 'orm');

        });

        $('#soul-columns' + tableId + ', #soul-dropList' + tableId).on('mouseover', function (e) {
          e.stopPropagation();
        });
        $('#main-list' + tableId + ' .soul-edit-condition').on('mouseover', function (e) {
          _this.hideColumns(myTable);
          _this.hideDropList(myTable);
          _this.hideCondition(myTable);
          e.stopPropagation();
        }).on('click', function () {
          $('#main-list' + tableId).hide();
          _this.showConditionBoard(myTable);
        });
        $('#main-list' + tableId + ' .soul-export').on('mouseover', function (e) {
          _this.hideColumns(myTable);
          _this.hideDropList(myTable);
          _this.hideCondition(myTable);
          e.stopPropagation();
        }).on('click', function () {
          $('#main-list' + tableId).hide();
          _this.export(table_cache[myTable.id]);
        });

        $('#main-list' + tableId + ' .soul-clear-cache').on('mouseover', function (e) {
          _this.hideColumns(myTable);
          _this.hideDropList(myTable);
          _this.hideCondition(myTable);
          e.stopPropagation();
        }).on('click', function () {
          $('#main-list' + tableId).hide();
          if (layui.soulTable) {
            layui.soulTable.clearCache(myTable);
          }
          layer.msg('已还原！', {icon: 1, time: 1000});
        });

        $('#main-list' + tableId).on('mouseover', function (e) {
          var curX = e.pageX;
          var curY = e.pageY;
          var div = $(this);
          var y1 = div.offset().top;  //div上面两个的点的y值
          var y2 = y1 + div.height();//div下面两个点的y值
          var x1 = div.offset().left;  //div左边两个的点的x值
          var x2 = x1 + div.width();  //div右边两个点的x的值
          if (curX <= x1 || curX >= x2 || curY <= y1 || curY >= y2) {
          } else {
            _this.hideColumns(myTable);
            _this.hideDropList(myTable);
            _this.hideCondition(myTable);
          }
        });
      } else {

        types = {}; //存储过滤数据的类型
        // 根据表格列显示
        for (i = 0; i < columns.length; i++) {
          if (columns[i].type === 'checkbox' || !columns[i].field) {
            continue;
          }
          //存储过滤数据的类型
          if (columns[i].filter && columns[i].filter.type) {
            if (columns[i].filter.field) {
              types[columns[i].filter.field] = columns[i].filter.type;
            } else {
              types[columns[i].field] = columns[i].filter.type;
            }
          }
        }
        if (JSON.stringify(types).length !== 2) {
          myTable.where['tableFilterType'] = JSON.stringify(types);
        }

      }

      // 初始化下拉数据
      if ($('#soulDropList' + tableId).length === 0) {
        $('body').append('<div id="soulDropList' + tableId + '" style="display: none"></div>');
      }

      if ($tableHead.find('.soul-table-filter').length > 0) {
        var columnField = [], mainDataSwitch = filterItems.indexOf('data') !== -1;
        $tableHead.find('.soul-table-filter').each(function (index, elem) {
          if ($(this).data('column') && (mainDataSwitch ? (!$(this).data('items') || $(this).data('items').split(',').indexOf('data') !== -1) : $(this).data('items').split(',').indexOf('data') !== -1)) {
            columnField.push($(this).data('column'));
          }
        });
        if (columnField.length > 0) {
          if (typeof myTable.url !== 'undefined' && myTable.page) {
            var datas = JSON.parse(JSON.stringify(myTable.where)), url = myTable.url;
            datas['columns'] = JSON.stringify(columnField);

            if (typeof myTable.parseParams === 'function') {
              datas = myTable.parseParams(datas) || datas;
            }
            if (myTable.contentType && myTable.contentType.indexOf("application/json") === 0) { //提交 json 格式
              datas = JSON.stringify(datas);
            }

            $.ajax({
              url: url,
              data: datas,
              dataType: 'json',
              method: 'post',
              headers: myTable.headers || {},
              contentType: myTable.contentType,
              success: function (result) {

                var uls = [];
                for (var key in result) {
                  var list = result[key];
                  if (!((list.length === 1 && list[0] === '') || list.length === 0)) {
                    var ul = [];
                    ul.push("<ul class='" + key + "DropList' data-value='" + key + "'>");

                    var columnsConfigs = columns;
                    for (j = 0; j < columnsConfigs.length; j++) {
                      if (columnsConfigs[j].field === key) {
                        if (columnsConfigs[j].filter.split) {
                          var tempList = [];
                          for (i = 0; i < list.length; i++) {
                            var tempList2 = list[i].split(columnsConfigs[j].filter.split);
                            for (var k = 0; k < tempList2.length; k++) {
                              if (tempList.indexOf(tempList2[k]) === -1) {
                                tempList.push(tempList2[k]);
                              }
                            }
                          }
                          list = tempList;
                        }
                        list.sort(function (a, b) {
                          if (isNaN(a) || isNaN(b)) {
                            return String(a) >= String(b);
                          } else {
                            return Number(a) - Number(b);
                          }
                        });
                        for (i = 0; i < list.length; i++) {
                          if (UNHANDLED_VALUES.indexOf(list[i]) === -1) {
                            var line = {};
                            line[key] = list[i];
                            ul.push('<li data-value="' + String(list[i]).toLowerCase() + '"><input type="checkbox" value="' + list[i] + '" title="' + (_this.parseTempData(columnsConfigs[j], list[i], line, true)).replace(/\"|\'/g, '\'') + '" lay-skin="primary" lay-filter="soulDropList' + tableId + '"></li>');
                          }
                        }
                        break;
                      }
                    }

                    ul.push("</ul>");
                    uls.push(ul.join(''));
                  } else {
                    uls.push("<ul class='" + key + "DropList' data-value='" + key + "'><li style='color: gray;line-height: 25px;padding-left: 20px;'>(无数据)</li></ul>");
                  }
                }
                $('#soulDropList' + tableId).html(uls.join(''));
              },
              error: function () {
                // layer.msg('列筛选数据查询失败！', {icon: 2, anim: 6})
              }
            });
          } else {
            var tableDatas = cache[myTable.id];
            var dropDatas = {};
            for (i = 0; i < tableDatas.length; i++) {
              for (j = 0; j < columnField.length; j++) {
                var value = typeof tableDatas[i][columnField[j]] === 'undefined' ? '' : tableDatas[i][columnField[j]];
                if (dropDatas[columnField[j]]) {
                  if (dropDatas[columnField[j]].indexOf(value) === -1) {
                    dropDatas[columnField[j]].push(value);
                  }
                } else {
                  dropDatas[columnField[j]] = [value];
                }
              }
            }

            var columnsConfigs = columns;
            var uls = [];
            for (j = 0; j < columnsConfigs.length; j++) {
              var key = columnsConfigs[j].field;
              var list = dropDatas[key];
              if (list && !(list.length === 1 && list[0] === '')) {
                if (columnsConfigs[j].filter && columnsConfigs[j].filter.split) {
                  var tempList = [];
                  for (i = 0; i < list.length; i++) {
                    var tempList2 = String(list[i]).split(columnsConfigs[j].filter.split);
                    for (var k = 0; k < tempList2.length; k++) {
                      if (tempList.indexOf(tempList2[k]) === -1) {
                        tempList.push(tempList2[k]);
                      }
                    }
                  }
                  list = tempList;
                }
                list.sort(function (a, b) {
                  if (isNaN(a) || isNaN(b)) {
                    return String(a) >= String(b);
                  } else {
                    return Number(a) - Number(b);
                  }
                });
                var ul = [];
                ul.push("<ul class='" + key + "DropList' data-value='" + key + "'>");
                for (i = 0; i < list.length; i++) {
                  if (UNHANDLED_VALUES.indexOf(list[i]) === -1) {
                    var line = {};
                    line[key] = list[i];
                    ul.push('<li data-value="' + String(list[i]).toLowerCase() + '"><input type="checkbox" value="' + list[i] + '" title="' + (_this.parseTempData(columnsConfigs[j], list[i], line, true)).replace(/\"|\'/g, '\'') + '" lay-skin="primary" lay-filter="soulDropList' + tableId + '"></li>');
                  }
                }
                ul.push("</ul>");
                uls.push(ul.join(''));
              } else {
                uls.push("<ul class='" + key + "DropList' data-value='" + key + "'><li style='color: gray;line-height: 25px;padding-left: 20px;'>(无数据)</li></ul>");
              }
            }
            $('#soulDropList' + tableId).html(uls.join(''));
          }
        } else {
          _this.bindFilterClick(myTable);
        }
      }

      this.bindFilterClick(myTable);
    },
    showConditionBoard: function (myTable) {
      var _this = this,
        tableId = myTable.id,
        where = where_cache[myTable.id] || {},
        tableFilterTypes = where.tableFilterType ? JSON.parse(where.tableFilterType) : {},
        filterSos = where.filterSos ? JSON.parse(where.filterSos) : [],
        filterBoard = [], fieldMap = {}, firstColumn, curItems,
        filterItems = myTable.filter ? myTable.filter.items || defaultFilterItems : defaultFilterItems,
        columns = _this.getCompleteCols(myTable.cols),
        i;
      for (i = 0; i < columns.length; i++) {
        if (columns[i].field && columns[i].filter) {
          if (!firstColumn) {
            firstColumn = columns[i];
          }
          curItems = columns[i].filter.items || filterItems;
          fieldMap[columns[i]['field']] = {
            title: columns[i].title,
            items: curItems
          };
        }
      }
      filterBoard.push('<div class="soul-edit-out">');
      filterBoard.push('<div class="layui-form" lay-filter="soul-edit-out">');
      filterBoard.push('<div><a class="layui-btn layui-btn-sm" data-type="addOne"><i class="layui-icon layui-icon-add-1"></i> 添加条件</a><a class="layui-btn layui-btn-sm" data-type="addGroup"><i class="layui-icon layui-icon-add-circle" ></i> 添加分组</a><a class="layui-btn layui-btn-sm" data-type="search" style="float: right"><i class="layui-icon layui-icon-search"></i> 查询</a><span style="float: right"><input type="checkbox" lay-filter="out_auto" class="out_auto" title="实时更新"></span></div>');
      filterBoard.push('<hr>');
      filterBoard.push('<ul>');
      for (i = 0; i < filterSos.length; i++) {
        groupHtml(filterSos[i], filterBoard, fieldMap, i === 0, i === (filterSos.length - 1));
      }
      filterBoard.push('</ul>');
      filterBoard.push('</div>');
      filterBoard.push('</div>');
      layer.open({
        title: '编辑条件',
        type: 1,
        offset: 'auto',
        area: ['480px', '480px'],
        content: filterBoard.join('')
      });
      form.render(null, 'soul-edit-out');

      form.on('checkbox(out_auto)', function (data) {
        if (data.elem.checked) {
          _this.soulReload(myTable);
        }
      });

      function groupHtml(filterSo, filterBoard, fieldMap, isFirst, isLast) {
        var id = filterSo.id,
          field = filterSo.field,
          mode = filterSo.mode,
          type = filterSo.type,
          isOr = filterSo.prefix === 'or';
        filterBoard.push('<li data-id="' + id + '" data-field="' + field + '" ' + (isLast ? 'class="last"' : '') + ' data-mode="' + mode + '" data-type="' + type + '" data-value="' + (typeof filterSo.value === 'undefined' ? '' : filterSo.value) + '" >');
        filterBoard.push('<div><table><tbody><tr><td data-type="top"></td></tr><tr><td data-type="bottom"></td></tr></tbody></table></div>');
        // if (!isFirst) { //第一个隐藏 与或
        filterBoard.push('<div><input type="checkbox" name="switch" lay-filter="soul-edit-switch" lay-skin="switch" lay-text="与|或" ' + (isOr ? '' : 'checked') + '></div>');
        // }
        switch (mode) {
          case 'in':
            filterBoard.push('<div class="layui-firebrick item-field">' + (fieldMap[field].title) + '</div>');
            filterBoard.push('<div class="layui-deeppink item-type" >筛选数据</div>');
            filterBoard.push('<div class="layui-blueviolet item-value">共' + (filterSo.values ? filterSo.values.length : 0) + '条数据</div>');
            filterBoard.push('<div class="layui-red delete-item"><i class="layui-icon layui-icon-close-fill"></i></div>');
            break;
          case 'date':
            filterBoard.push('<div class="layui-firebrick item-field">' + (fieldMap[field].title) + '</div>');
            filterBoard.push('<div class="layui-deeppink item-type">选择日期</div>');
            filterBoard.push('<div class="layui-blueviolet item-value">' + (filterSo.type === 'specific' ? filterSo.value || '请选择' : dateTimeItems[filterSo.type]) + '</div>');
            filterBoard.push('<div class="layui-red delete-item"><i class="layui-icon layui-icon-close-fill"></i></div>');
            break;
          case 'condition':
            filterBoard.push('<div class="layui-firebrick item-field">' + (fieldMap[field].title) + '</div>');
            filterBoard.push('<div class="layui-deeppink item-type">' + conditionChangeItems[filterSo.type] + '</div>');
            if (type !== 'null' && type !== 'notNull') {
              filterBoard.push('<div class="layui-blueviolet item-value">' + (typeof filterSo.value === 'undefined' || filterSo.value === '' ? '请输入...' : filterSo.value) + '</div>');
            }
            filterBoard.push('<div class="layui-red delete-item"><i class="layui-icon layui-icon-close-fill"></i></div>');
            break;
          case 'group':
            filterBoard.push('<div class="layui-firebrick">分组</div>');
            filterBoard.push('<div><a class="layui-btn layui-btn-xs" data-type="addOne"><i class="layui-icon layui-icon-add-1"></i> 添加条件</a><a class="layui-btn layui-btn-xs" data-type="addGroup"><i class="layui-icon layui-icon-add-circle"></i> 添加分组</a></div>');
            filterBoard.push('<div class="layui-red delete-item"><i class="layui-icon layui-icon-close-fill"></i></div>');
            filterBoard.push('<ul class="group ' + (isLast ? '' : 'line') + '">');
            if (filterSo.children) {
              for (var i = 0; i < filterSo.children.length; i++) {
                groupHtml(filterSo.children[i], filterBoard, fieldMap, i === 0, i === (filterSo.children.length - 1));
              }
            }
            filterBoard.push('</ul>');
            break;
        }
        filterBoard.push('</li>');
      }

      // prefix
      form.on('switch(soul-edit-switch)', function (data) {
        changePrefix(data);
      });

      // column
      $('.soul-edit-out .item-field').on('click', function (e) {
        e.stopPropagation();
        showColums(this);
      });
      // type
      $('.soul-edit-out .item-type').on('click', function (e) {
        e.stopPropagation();
        showTypes(this);
      });
      // value
      $('.soul-edit-out .item-value').on('click', function (e) {
        e.stopPropagation();
        showValue(this);
      });
      // delete
      $('.soul-edit-out .delete-item').on('click', function () {
        var id = $(this).parent().data('id'),
          refresh = $('.soul-edit-out .out_auto').prop('checked');
        $(this).parent().remove();
        _this.updateWhere(myTable, {
          id: id,
          delete: true
        });
        if (refresh) {
          _this.soulReload(myTable);
        }
      });

      function changePrefix(data) {
        var prefix = data.elem.checked ? 'and' : 'or',
          id = $(data.elem).parents('li:eq(0)').data('id'),
          refresh = $('.soul-edit-out .out_auto').prop('checked');

        $(data.elem).parents('li:eq(0)').data('prefix', prefix);
        _this.updateWhere(myTable, {
          id: id,
          prefix: prefix
        });

        if (refresh) {
          _this.soulReload(myTable);
        }
      }

      function showColums(obj) {
        _this.hideDropList(myTable);
        _this.hideCondition(myTable);
        _this.hideColumns(myTable);
        _this.hideBfPrefix(myTable);
        _this.hideBfType(myTable);
        var top = $(obj).offset().top + $(obj).outerHeight(),
          left = $(obj).offset().left;

        $('#soul-bf-column' + tableId).find('li.soul-bf-selected').removeClass('soul-bf-selected');
        $('#soul-bf-column' + tableId)
          .data('field', $(obj).parent().data('field'))
          .data('id', $(obj).parent().data('id'))
          .data('mode', $(obj).parent().data('mode'))
          .data('group', $(obj).parents('li:eq(2)').data('id') || '')
          .data('refresh', $('.soul-edit-out .out_auto').prop('checked'))
          .show()
          .css({top: top, left: left})
          .removeClass().addClass('fadeInUp animated')
          .find('li[data-field="' + $(obj).parent().data('field') + '"]')
          .addClass('soul-bf-selected');
      }

      function showTypes(obj) {
        _this.hideDropList(myTable);
        _this.hideCondition(myTable);
        _this.hideColumns(myTable);
        _this.hideBfColumn(myTable);
        _this.hideBfPrefix(myTable);
        var top = $(obj).offset().top + $(obj).outerHeight(),
          left = $(obj).offset().left,
          field = $(obj).parent().data('field');

        $('#soul-bf-type' + tableId + ' li').hide();
        if (tableFilterTypes[field] && tableFilterTypes[field].indexOf('date') === 0) {
          $('#soul-bf-type' + tableId + ' li[data-mode=date]').show();
        }
        if (fieldMap[field].items.indexOf('data') !== -1) {
          $('#soul-bf-type' + tableId + ' li[data-mode=in]').show();
        }
        if (fieldMap[field].items.indexOf('condition') !== -1) {
          $('#soul-bf-type' + tableId + ' li[data-mode=condition]').show();
        }

        $('#soul-bf-type' + tableId).find('li.soul-bf-selected').removeClass('soul-bf-selected');
        switch ($(obj).parent().data('mode')) {
          case 'in':
            $('#soul-bf-type' + tableId).find('li[data-mode="in"]')
              .addClass('soul-bf-selected');
            break;
          case 'date':
            $('#soul-bf-type' + tableId).find('li[data-mode="date"]')
              .addClass('soul-bf-selected');
          case 'condition':
            $('#soul-bf-type' + tableId).find('li[data-value="' + $(obj).parent().data('type') + '"]')
              .addClass('soul-bf-selected');
        }

        $('#soul-bf-type' + tableId)
          .data('type', $(obj).parent().data('type'))
          .data('mode', $(obj).parent().data('mode'))
          .data('id', $(obj).parent().data('id'))
          .data('group', $(obj).parents('li:eq(2)').data('id') || '')
          .data('refresh', $('.soul-edit-out .out_auto').prop('checked'))
          .show()
          .css({top: top, left: left})
          .removeClass().addClass('fadeInUp animated');
      }

      function showValue(obj) {
        _this.hideColumns(myTable);
        _this.hideBfType(myTable);
        _this.hideBfPrefix(myTable);
        _this.hideBfColumn(myTable);

        var top,
          left = $(obj).offset().left,
          mode = $(obj).parent().data('mode'),
          field = $(obj).parent().data('field'),
          id = $(obj).parent().data('id'),
          head = $(obj).parent().data('head'),
          prefix = $(obj).parent().data('prefix'),
          value = $(obj).parent().data('value'),
          refresh = $('.soul-edit-out .out_auto').prop('checked'),
          where = where_cache[myTable.id] || {},
          filterSos = where.filterSos ? JSON.parse(where.filterSos) : [];

        switch (mode) {
          case 'in':
            _this.hideCondition(myTable);
            if (dorpListTimeOut) {
              clearTimeout(dorpListTimeOut);
            }
            $('#soul-dropList' + tableId + '>.filter-search>input').val('');
            $('#soul-dropList' + tableId).show();
            $('#soulDropList' + tableId).find('.' + field + 'DropList li input[type=checkbox]:checked').prop('checked', false);
            var filterSo = _this.getFilterSoById(filterSos, id);
            if (filterSo.values) {
              for (i = 0; i < filterSo.values.length; i++) {
                $('#soulDropList' + tableId).find('.' + field + 'DropList li input[type=checkbox][value="' + filterSo.values[i] + '"]').prop('checked', true);
              }
            }

            $('#soul-dropList' + tableId + '>ul').data('id', id).data('head', head).data('refresh', refresh).data('prefix', prefix).html($('#soulDropList' + tableId).find('.' + field + 'DropList li').clone());
            form.render('checkbox', 'orm');
            top = $(obj).offset().top + $(obj).outerHeight();
            $('#soul-dropList' + tableId).css({'top': top, 'left': left})
              .show().removeClass().addClass('fadeInUp animated');
            setTimeout(function () {
              $('#soul-dropList' + tableId + '>.filter-search>input').focus(); // 聚焦搜索框
            }, 1);

            // 监听筛选数据
            var liClick = true;
            form.on('checkbox(soulDropList' + tableId + ')', function (data) {
              liClick = false;
              _this.updateDropList(myTable, field);
            });

            $('#soul-dropList' + tableId + '>ul>li[data-value]').on('click', function () {
              if (liClick) {
                $(this).find('div.layui-form-checkbox').trigger('click');
              }
              liClick = true;
            });
            break;
          case 'date':
            _this.hideDropList(myTable);
            if (conditionTimeOut) {
              clearTimeout(conditionTimeOut);
            }
            var filterSo = _this.getFilterSoById(filterSos, id),
              top = $(obj).offset().top + $(obj).height();

            _this.showDate(myTable, field, filterSo, "fadeInUp", top, left, "down", refresh);
            break;
          case 'condition':
            $(obj).hide();
            $(obj).after('<div><input class="layui-input tempValue" value="" /></div>');
            $(obj).next().children().val(value).select().on('keydown', function (e) {
              if (e.keyCode === 13) {
                $(this).blur();
              }
            }).on('blur', function () {
              var newValue = $(this).val();
              $(obj).html(typeof newValue === 'undefined' || newValue === '' ? '请输入...' : newValue);
              $(obj).show();
              $(this).parent().remove();
              if (newValue !== value) {
                $(obj).parent().data('value', newValue);
                _this.updateWhere(myTable, {
                  id: id,
                  value: newValue
                });
                if (refresh) {
                  _this.soulReload(myTable);
                }
              }
            });
            break;
        }

      }

      $('.soul-edit-out a[data-type]').on('click', function () {
        if ($(this).data('type') === 'search') {
          _this.soulReload(myTable);
        } else {
          addLine(this);
        }
      });

      function addLine(obj) {
        var refresh = $('.soul-edit-out .out_auto').prop('checked');
        filterBoard = [];
        switch ($(obj).data('type')) {
          case 'addOne':
            var filterSo = {
              prefix: 'and',
              field: firstColumn.field,
              mode: 'condition',
              type: 'eq',
              value: ''
            };
            if ($(obj).parent().parent().data('id')) {
              $.extend(filterSo, {
                groupId: $(obj).parent().parent().data('id')
              });
            }

            _this.updateWhere(myTable, filterSo);

            filterBoard.push('<li data-id="' + filterSo.id + '" data-field="' + filterSo.field + '" data-mode="' + filterSo.mode + '" data-type="' + filterSo.type + '" data-value="' + filterSo.value + '" data-prefix="' + filterSo.prefix + '" class="last">');
            filterBoard.push('<div><table><tbody><tr><td data-type="top"></td></tr><tr><td data-type="bottom"></td></tr></tbody></table></div>');
            filterBoard.push('<div><input type="checkbox" name="switch" lay-filter="soul-edit-switch" lay-skin="switch" lay-text="与|或" checked></div>');
            filterBoard.push('<div class="layui-firebrick item-field">' + fieldMap[filterSo.field].title + '</div>');
            filterBoard.push('<div class="layui-deeppink item-type">等于</div>');
            filterBoard.push('<div class="layui-blueviolet item-value">请输入...</div>');
            filterBoard.push('<div class="layui-red delete-item"><i class="layui-icon layui-icon-close-fill"></i></div>');
            filterBoard.push('</li>');
            break;
          case 'addGroup':
            var filterSo = {
              prefix: 'and',
              mode: 'group',
              children: []
            };
            if ($(obj).parent().parent().data('id')) {
              $.extend(filterSo, {
                groupId: $(obj).parent().parent().data('id')
              });
            }
            _this.updateWhere(myTable, filterSo);

            filterBoard.push('<li data-id="' + filterSo.id + '" class="last">');
            filterBoard.push('<div><table><tbody><tr><td data-type="top"></td></tr><tr><td data-type="bottom"></td></tr></tbody></table></div>');
            filterBoard.push('<div><input type="checkbox" name="switch" lay-filter="soul-edit-switch" lay-skin="switch" lay-text="与|或" checked></div>');
            filterBoard.push('<div class="layui-firebrick">分组</div>');
            filterBoard.push('<div><a class="layui-btn layui-btn-xs" data-type="addOne"><i class="layui-icon layui-icon-add-1"></i> 添加条件</a><a class="layui-btn layui-btn-xs" data-type="addGroup"><i class="layui-icon layui-icon-add-circle"></i> 添加分组</a></div>');
            filterBoard.push('<div class="layui-red delete-item"><i class="layui-icon layui-icon-close-fill"></i></div>');
            filterBoard.push('<ul class="group">');
            filterBoard.push('</ul>');
            filterBoard.push('</li>');
            break;
        }
        if (refresh) {
          _this.soulReload(myTable);
        }
        if ($(obj).parent().parent().children('ul').children('li').length > 0) {
          $(obj).parent().parent().children('ul').children('li:last').removeClass('last');
          if ($(obj).parent().parent().children('ul').children('li:last').children('ul.group').length > 0) {
            $(obj).parent().parent().children('ul').children('li:last').children('ul.group').addClass('line');
          }
        }
        $(obj).parent().parent().children('ul').append(filterBoard.join(''));
        form.render('checkbox', 'soul-edit-out');
        if ($(obj).data('type') === 'addGroup') {
          $(obj).parent().parent().children('ul').children("li:last").find('a[data-type]').on('click', function () {
            addLine(this);
          });
        } else {
          $(obj).parent().parent().children('ul').children("li:last").find('.item-field').on('click', function (e) {
            e.stopPropagation();
            showColums(this);
          });
          $(obj).parent().parent().children('ul').children("li:last").find('.item-type').on('click', function (e) {
            e.stopPropagation();
            showTypes(this);
          });
          $(obj).parent().parent().children('ul').children("li:last").find('.item-value').on('click', function (e) {
            e.stopPropagation();
            showValue(this);
          });
        }
        $(obj).parent().parent().children('ul').children("li:last").children('.delete-item').on('click', function () {
          var id = $(this).parent().data('id'),
            refresh = $('.soul-edit-out .out_auto').prop('checked');
          $(this).parent().remove();
          _this.updateWhere(myTable, {
            id: id,
            delete: true
          });
          if (refresh) {
            _this.soulReload(myTable);
          }
        });
      }
    }
    , hideColumns: function (myTable, animate) {
      var tableId = myTable.id;

      $('#soul-columns' + tableId).removeClass().addClass('fadeOutLeft animated');
      if (columnsTimeOut) {
        clearTimeout(columnsTimeOut);
      }
      if (typeof animate === 'undefined' || animate) {
        columnsTimeOut = setTimeout(function (e) {
          $('#soul-columns' + tableId).hide();
        }, 500);
      } else {
        $('[id^=soul-columns]').hide();
      }

    }
    , hideDropList: function (myTable, animate) {
      var tableId = myTable.id;
      $('#soul-dropList' + tableId).removeClass().addClass('fadeOutLeft animated');
      if (dorpListTimeOut) {
        clearTimeout(dorpListTimeOut);
      }
      if (typeof animate === 'undefined' || animate) {
        dorpListTimeOut = setTimeout(function (e) {
          $('#soul-dropList' + tableId).hide();
        }, 500);
      } else {
        $('[id^=soul-dropList]').hide();
      }

    }
    , hideCondition: function (myTable, animate) {
      var tableId = myTable.id;
      $('#soul-condition' + tableId).removeClass().addClass('fadeOutLeft animated');
      if (conditionTimeOut) {
        clearTimeout(conditionTimeOut);
      }
      if (typeof animate === 'undefined' || animate) {
        conditionTimeOut = setTimeout(function (e) {
          $('#soul-condition' + tableId).hide();
        }, 500);
      } else {
        $('[id^=soul-condition]').hide();
      }
    }
    , hideBfPrefix: function (myTable, animate) {
      var tableId = myTable.id;
      $('#soul-bf-prefix' + tableId).removeClass().addClass('fadeOutDown animated');
      if (bfColumnTimeOut) {
        clearTimeout(bfColumnTimeOut);
      }
      if (typeof animate === 'undefined' || animate) {
        bfColumnTimeOut = setTimeout(function () {
          $('#soul-bf-prefix' + tableId).hide();
        }, 500);
      } else {
        $('[id=soul-bf-prefix' + tableId + ']').hide();
      }
    }
    , hideBfColumn: function (myTable, animate) {
      var tableId = myTable.id;
      $('#soul-bf-column' + tableId).removeClass().addClass('fadeOutDown animated');
      if (bfColumnTimeOut) {
        clearTimeout(bfColumnTimeOut);
      }
      if (typeof animate === 'undefined' || animate) {
        bfColumnTimeOut = setTimeout(function () {
          $('#soul-bf-column' + tableId).hide();
        }, 500);
      } else {
        $('[id=soul-bf-column' + tableId + ']').hide();
      }
    }
    , hideBfType: function (myTable, animate) {
      var tableId = myTable.id;
      $('#soul-bf-type' + tableId).removeClass().addClass('fadeOutDown animated');
      if (bfCond1TimeOut) {
        clearTimeout(bfCond1TimeOut);
      }
      if (typeof animate === 'undefined' || animate) {
        bfCond1TimeOut = setTimeout(function () {
          $('#soul-bf-type' + tableId).hide();
        }, 500);
      } else {
        $('[id=soul-bf-type' + tableId + ']').hide();
      }
    }
    , bindFilterClick: function (myTable) {
      var _this = this,
        $table = $(myTable.elem),
        $tableHead = $table.next().children('.layui-table-box').children('.layui-table-header').children('table'),
        $fixedLeftTableHead = $table.next().children('.layui-table-box').children('.layui-table-fixed-l').children('.layui-table-header').children('table'),
        $fixedRigthTableHead = $table.next().children('.layui-table-box').children('.layui-table-fixed-r').children('.layui-table-header').children('table'),
        tableId = myTable.id,
        filterItems = myTable.filter ? myTable.filter.items || defaultFilterItems : defaultFilterItems,
        mainListTimeOut;

      // 显示筛选框
      $tableHead.find('.soul-table-filter').off('click').on('click', function (e) {
        e.stopPropagation();
        showFilter($(this));
      });
      $fixedLeftTableHead.find('.soul-table-filter').off('click').on('click', function (e) {
        e.stopPropagation();
        showFilter($(this));
      });
      $fixedRigthTableHead.find('.soul-table-filter').off('click').on('click', function (e) {
        e.stopPropagation();
        showFilter($(this));
      });

      function showFilter($that) {
        var curItems = $that.data('items') ? $that.data('items').split(',') : filterItems;
        _this.hideColumns(myTable, false);
        _this.hideDropList(myTable, false);
        _this.hideCondition(myTable, false);
        $('[id^=main-list]').hide();

        $('#main-list' + tableId).data({'field': $that.data('column'), 'split': $that.data('split')});

        $('#soul-columns' + tableId + ' [type=checkbox]').attr('disabled', false);
        // if (myTable.cols[0][0].type=='checkbox') {
        //     $('#soul-columns'+tableId+' [type=checkbox]:eq('+($that.parents('th').data('key').split('-')[2]-1)+')').attr('disabled', true);
        // } else {
        $('#soul-columns' + tableId + ' li[data-key=' + $that.parents('th').data('key').split('-')[2] + '] [type=checkbox]').attr('disabled', true);
        // }

        $('#main-list' + tableId + ' > li').hide();
        // 是否显示排序框
        if ($that.hasClass('layui-table-sort')) {
          $('#main-list' + tableId + ' .soul-sort').show();
        }
        for (var i = 0; i < curItems.length; i++) {
          $('#main-list' + tableId + ' .' + itemsMap[curItems[i]]).show();
          if ($('#main-list' + tableId + ' .' + itemsMap[curItems[i]]).index() !== (i + 2)) {
            $('#main-list' + tableId + '>li:eq("' + (i + 2) + '")').before($('#main-list' + tableId + ' .' + itemsMap[curItems[i]]));

          }
        }
        if (mainListTimeOut) {
          clearTimeout(mainListTimeOut);
        }
        var left, animate;
        if ($that.offset().left + $('#main-list' + tableId).outerWidth() < document.body.clientWidth) {
          left = $that.offset().left + 10;
          animate = 'fadeInLeft';
        } else {
          left = $that.offset().left - $('#main-list' + tableId).outerWidth();
          animate = 'fadeInRight';
        }
        $('#main-list' + tableId).data('type', myTable.where.tableFilterType ? JSON.parse(myTable.where.tableFilterType)[$that.data('column')] || '' : '').hide().css({
          'top': $that.offset().top + 10,
          'left': left
        }).show().removeClass().addClass(animate + ' animated');

        // 排序
        $('#main-list' + tableId + ' .soul-sort').on('click', function (e) {
          $that.siblings('.layui-table-sort').find('.layui-table-sort-' + $(this).data('value')).trigger('click');
          $('#main-list' + tableId).hide();
        });
        form.render('checkbox', 'orm');
      }

      $(document).on('click', function (e) {
        $('#main-list' + tableId).hide();
        _this.hideColumns(myTable, false);
        _this.hideDropList(myTable, false);
        _this.hideCondition(myTable, false);
        _this.hideBfPrefix(myTable, false);
        _this.hideBfColumn(myTable, false);
        _this.hideBfType(myTable, false);
      });
      $('#main-list' + tableId + ',#soul-columns' + tableId + ',#soul-dropList' + tableId + ',#soul-condition' + tableId).on('click', function (e) {
        $(this).find('.layui-form-selected').removeClass('layui-form-selected');
        e.stopPropagation();
      });

      //渲染底部筛选条件
      _this.renderBottomCondition(myTable);

      // 表头样式
      var where = where_cache[myTable.id] || {},
        filterSos = JSON.parse(where.filterSos ? where.filterSos : '[]');

      for (var i = 0; i < filterSos.length; i++) {
        if (filterSos[i].head) {
          var hasFilter = false;
          switch (filterSos[i].mode) {
            case 'in':
              if (filterSos[i].values && filterSos[i].values.length > 0) {
                hasFilter = true;
              }
              break;
            case 'date':
              if (filterSos[i].type !== 'all' && typeof filterSos[i].value !== 'undefined' && filterSos[i].value !== '') {
                hasFilter = true;
              }
              break;
            case 'group':
              if (filterSos[i].children && filterSos[i].children.length > 0) {
                hasFilter = true;
              }
            default:
              break;
          }
          $tableHead.find('thead>tr>th[data-field="' + filterSos[i].field + '"] .soul-table-filter').attr('soul-filter', '' + hasFilter);
          $fixedLeftTableHead.find('thead>tr>th[data-field="' + filterSos[i].field + '"] .soul-table-filter').attr('soul-filter', '' + hasFilter);
          $fixedRigthTableHead.find('thead>tr>th[data-field="' + filterSos[i].field + '"] .soul-table-filter').attr('soul-filter', '' + hasFilter);
        }
      }
    }
    , resize: function (myTable) {
      var _this = this,
        $table = $(myTable.elem),
        $tableBox = $table.next().children('.layui-table-box'),
        $tableMain = $tableBox.children('.layui-table-main');
      // 减去底部筛选的高度
      if ($table.next().children('.soul-bottom-contion').length > 0) {
        $table.next().children('.soul-bottom-contion').children('.condition-items').css('width', $table.next().children('.soul-bottom-contion').width() - $table.next().children('.soul-bottom-contion').children('.editCondtion').outerWidth());

        var bodyHeight = $table.next().height() - $table.next().children('.soul-bottom-contion').outerHeight();
        if ($table.next().children('.layui-table-tool').length > 0) {
          bodyHeight = bodyHeight - $table.next().children('.layui-table-tool').outerHeight();
        }
        if ($table.next().children('.layui-table-total').length > 0) {
          bodyHeight = bodyHeight - $table.next().children('.layui-table-total').outerHeight();
        }
        if ($table.next().children('.layui-table-page').length > 0) {
          bodyHeight = bodyHeight - $table.next().children('.layui-table-page').outerHeight();
        }

        bodyHeight = bodyHeight - $table.next().children('.layui-table-box').children('.layui-table-header').outerHeight();

        $table.next().children('.layui-table-box').children('.layui-table-body').height(bodyHeight);
        var fixHeight = bodyHeight - _this.getScrollWidth($tableMain[0]),
          layMainTableHeight = $tableMain.children('table').height();
        $table.next().children('.layui-table-box').children('.layui-table-fixed').children('.layui-table-body').height(layMainTableHeight >= fixHeight ? fixHeight : 'auto');

        var scollWidth = $tableMain.width() - $tableMain.prop('clientWidth'); //纵向滚动条宽度;
        $tableBox.children('.layui-table-fixed-r').css('right', scollWidth - 1);
      }
    }
    /**
     * 同步当前 droplist
     * @param myTable
     * @param field
     */
    , updateDropList: function (myTable, field) {
      var _this = this,
        $table = $(myTable.elem),
        tableId = myTable.id,
        id = $('#soul-dropList' + tableId + '>ul').data('id'),
        $checkedDom = $('#soul-dropList' + tableId + '>ul input[type=checkbox]:checked'),
        values = [],
        head = $('#soul-dropList' + tableId + '>ul').data('head'),
        prefix = $('#soul-dropList' + tableId + '>ul').data('prefix'),
        refresh = $('#soul-dropList' + tableId + '>ul').data('refresh'),
        split = $('#soul-dropList' + tableId + '>ul').data('split');
      if ($checkedDom.length > 0) {
        $checkedDom.each(function () {
          values.push($(this).val());
        });
      }
      var filterSo = {
        id: id,
        head: head,
        prefix: prefix || 'and',
        mode: 'in',
        field: field,
        split: split,
        values: values
      };
      _this.updateWhere(myTable, filterSo);
      if (!id) {
        $('#soul-dropList' + tableId + '>ul').data('id', filterSo.id);
      }

      if ($('.soul-edit-out').length > 0) {
        $('.soul-edit-out li[data-id="' + filterSo.id + '"]>.item-value').html('共' + (filterSo.values ? filterSo.values.length : 0) + '条数据');
      }

      if (refresh) {
        _this.soulReload(myTable);
      }
    }
    , getFilterSoById: function (filterSos, id) {
      for (var i = 0; i < filterSos.length; i++) {
        if (filterSos[i].id === id) {
          return filterSos[i];
        } else if (filterSos[i].mode === 'group') {
          for (var j = 0; j < filterSos[i].children.length; j++) {
            var filterSo = this.getFilterSoById(filterSos[i].children, id);
            if (filterSo) return filterSo;
          }
        }
      }
      return null;
    }
    /**
     * 更新 filter 条件
     * @param myTable
     * @param filterSo
     */
    , updateWhere: function (myTable, filterSo) {
      var _this = this,
        where = where_cache[myTable.id] || {},
        filterSos = JSON.parse(where.filterSos ? where.filterSos : '[]');

      if (filterSo.id || filterSo.groupId) {
        for (var i = 0; i < filterSos.length; i++) {
          if (filterSo.delete && filterSos[i].id === filterSo.id) {
            filterSos.splice(i, 1);
            break;
          }
          if (updateFilterSo(filterSos[i], filterSo)) {
            break;
          }
        }
      } else if (!(filterSo.mode === 'in' && !(filterSo.values && filterSo.values.length > 0))) {
        filterSos.push($.extend(filterSo, {
          id: _this.getDifId()
        }));
      }
      where['filterSos'] = JSON.stringify(filterSos);
      myTable.where = where;
      where_cache[myTable.id] = where;

      function updateFilterSo(filterSo, newFilterSo) {
        var isMatch = false;

        if (filterSo.id === newFilterSo.id) {
          $.extend(filterSo, newFilterSo);
          isMatch = true;
        }

        // 在分组中新增
        if (!newFilterSo.id && filterSo.id === newFilterSo.groupId) {
          filterSo.children.push($.extend(newFilterSo, {
            id: _this.getDifId()
          }));
        } else if (filterSo.mode === 'group') {
          for (var i = 0; i < filterSo.children.length; i++) {
            if (newFilterSo.delete && filterSo.children[i].id === newFilterSo.id) {
              filterSo.children.splice(i, 1);
              return true;
            }
            if (updateFilterSo(filterSo.children[i], newFilterSo)) {
              return true;
            }
          }

        }

        return isMatch;
      }
    }
    /**
     * 根据当前条件重载表格
     * @param myTable 需要重载的表格对象
     * @param isr 是否为筛选重载，为 true 时，不进行筛选的初始化动作（包括渲染dom、请求表头数据等）
     */
    , soulReload: function (myTable, isr) {
      var _this = this,
        $table = $(myTable.elem),
        scrollLeft = $table.next().children('.layui-table-box').children('.layui-table-main').scrollLeft();

      isFilterReload[myTable.id] = typeof isr === 'undefined' ? true : isr;
      if (typeof myTable.url !== 'undefined' && myTable.page) {
        $table.data('scrollLeft', scrollLeft);
        /**
         * 后台筛选
         */
        table.reload(myTable.id, {
          where: where_cache[myTable.id] || {},
          page: {
            curr: 1 //重新从第 1 页开始
          }
        });
      } else {
        /**
         * 前端筛选
         */
        var where = where_cache[myTable.id] || {},
          filterSos = JSON.parse(where.filterSos ? where.filterSos : '[]'),
          tableFilterTypes = where.tableFilterType ? JSON.parse(where.tableFilterType) : {},
          loading = layer.load(2);
        if (!myTable.page) {
          // 修复前端不分页时，layui table bug 导致的只显示10条数据的问题
          myTable.limit = 100000000;
        }
        if (filterSos.length > 0) {
          var newData = [];
          layui.each(cache[myTable.id], function (index, item) {
            var show = true;

            for (var i = 0; i < filterSos.length; i++) {
              show = _this.handleFilterSo(filterSos[i], item, tableFilterTypes, show, i === 0);
            }

            if (show) {
              newData.push(item);
            }
          });
          if (myTable.page) {
            table.reload(myTable.id, {
              data: newData
              , initSort: myTable.initSort
              , isSoulFrontFilter: true
              , page: {
                curr: 1 //重新从第 1 页开始
              }
            });
          } else {
            var url = myTable.url;
            $table.next().off('click');
            var inst = table.reload(myTable.id, {
              url: ''
              , initSort: myTable.initSort
              , isSoulFrontFilter: true
              , data: newData
            });
            inst.config.url = url;
          }
          myTable.data = newData;

        } else {
          if (myTable.page) {
            table.reload(myTable.id, {
              data: cache[myTable.id]
              , initSort: myTable.initSort
              , isSoulFrontFilter: true
              , page: {
                curr: 1 //重新从第 1 页开始
              }
            });
          } else {
            table.reload(myTable.id, {
              data: cache[myTable.id]
              , initSort: myTable.initSort
              , isSoulFrontFilter: true
            });
          }
          myTable.data = cache[myTable.id];
        }
        $table.next().children('.layui-table-box').children('.layui-table-main').scrollLeft(scrollLeft);
        layer.close(loading);
      }
    }
    , handleFilterSo: function (filterSo, item, tableFilterTypes, show, first) {
      var isOr = first ? false : filterSo.prefix === 'or',
        field = filterSo.field,
        value = filterSo.value,
        status = true;

      // 如果有子元素
      if (filterSo.children && filterSo.children.length > 0) {
        for (var i = 0; i < filterSo.children.length; i++) {
          status = this.handleFilterSo(filterSo.children[i], item, tableFilterTypes, status, i === 0);
        }
        return isOr ? show || status : show && status;
      }

      switch (filterSo.mode) {
        case "in":
          if (filterSo.values && filterSo.values.length > 0) {
            if (filterSo.split) {
              var tempList = (item[field] + '').split(filterSo.split);
              var tempStatus = false;
              for (var i = 0; i < tempList.length; i++) {
                if (filterSo.values.indexOf(tempList[i]) !== -1) {
                  tempStatus = true;
                }
              }
              status = tempStatus;
            } else {
              status = filterSo.values.indexOf(item[field] + '') !== -1;
            }
          } else {
            return show;
          }
          break;
        case "condition":
          if (filterSo.type !== 'null' && filterSo.type !== 'notNull' && (typeof value === 'undefined' || value === '')) {
            return show;
          }
          switch (filterSo.type) {
            case "eq":
              status = isNaN(item[field]) || isNaN(value) ? item[field] === value : Number(item[field]) === Number(value);
              break;
            case "ne":
              status = isNaN(item[field]) || isNaN(value) ? item[field] !== value : Number(item[field]) !== Number(value);
              break;
            case "gt":
              status = isNaN(item[field]) || isNaN(value) ? item[field] > value : Number(item[field]) > Number(value);
              break;
            case "ge":
              status = isNaN(item[field]) || isNaN(value) ? item[field] >= value : Number(item[field]) >= Number(value);
              break;
            case "lt":
              status = isNaN(item[field]) || isNaN(value) ? item[field] < value : Number(item[field]) < Number(value);
              break;
            case "le":
              status = isNaN(item[field]) || isNaN(value) ? item[field] <= value : Number(item[field]) <= Number(value);
              break;
            case "contain":
              status = (item[field] + '').indexOf(value) !== -1;
              break;
            case "notContain":
              status = (item[field] + '').indexOf(value) === -1;
              break;
            case "start":
              status = (item[field] + '').indexOf(value) === 0;
              break;
            case "end":
              var d = (item[field] + '').length - (value + '').length;
              status = d >= 0 && (item[field] + '').lastIndexOf(value) === d;
              break;
            case "null":
              status = typeof item[field] === 'undefined' || item[field] === '' || item[field] === null;
              break;
            case "notNull":
              status = typeof item[field] !== 'undefined' && item[field] !== '' && item[field] !== null;
              break;
          }
          break;
        case "date":
          var dateVal = new Date(Date.parse(item[field].replace(/-/g, "/")));
          switch (filterSo.type) {
            case 'all':
              status = true;
              break;
            case 'yesterday':
              status = item[field] && isBetween(dateVal, getToday() - 86400, getToday() - 1);
              break;
            case 'thisWeek':
              status = item[field] && isBetween(dateVal, getFirstDayOfWeek(), getFirstDayOfWeek() + 86400 * 7 - 1);
              break;
            case 'lastWeek':
              status = item[field] && isBetween(dateVal, getFirstDayOfWeek() - 86400 * 7, getFirstDayOfWeek() - 1);
              break;
            case 'thisMonth':
              status = item[field] && isBetween(dateVal, getFirstDayOfMonth(), getCurrentMonthLast());
              break;
            case 'thisYear':
              status = item[field] && isBetween(dateVal, new Date(new Date().getFullYear(), 1, 1) / 1000, new Date(new Date().getFullYear() + 1, 1, 1) / 1000 - 1);
              break;
            case 'specific':
              var dateFormat = dateVal.getFullYear();
              dateFormat += '-' + (timeAdd0(dateVal.getMonth() + 1));
              dateFormat += '-' + timeAdd0(dateVal.getDate());
              status = item[field] && dateFormat === value;
              break;
          }
          break;
      }

      // 今天凌晨
      function getToday() {
        return new Date().setHours(0, 0, 0, 0) / 1000;
      }

      // 本周第一天
      function getFirstDayOfWeek() {
        var now = new Date();
        var weekday = now.getDay() || 7; //获取星期几,getDay()返回值是 0（周日） 到 6（周六） 之间的一个整数。0||7为7，即weekday的值为1-7
        return new Date(now.setDate(now.getDate() - weekday + 1)).setHours(0, 0, 0, 0) / 1000;//往前算（weekday-1）天，年份、月份会自动变化
      }

      //获取当月第一天
      function getFirstDayOfMonth() {
        return new Date(new Date().setDate(1)).setHours(0, 0, 0, 0) / 1000;
      }

      //获取当月最后一天最后一秒
      function getCurrentMonthLast() {
        var date = new Date();
        var currentMonth = date.getMonth();
        var nextMonth = ++currentMonth;
        var nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
        return nextMonthFirstDay / 1000 - 1;
      }

      function isBetween(v, a, b) {
        return (v.getTime() / 1000) >= a && (v.getTime() / 1000) <= b;
      }

      function timeAdd0(str) {
        str += "";
        if (str.length <= 1) {
          str = '0' + str;
        }
        return str;
      }

      return isOr ? show || status : show && status;
    }
    , getDifId: function () {
      return maxId++;
    }
    , showDate: function (myTable, field, filterSo, animate, top, left, type, refresh) {
      var _this = this,
        tableId = myTable.id,
        conditionHtml = [],
        documentWidth = document.body.clientWidth,
        animate;
      conditionHtml.push('<div class="' + field + 'Condition" data-value="' + field + '" style="padding: 5px;" >');
      conditionHtml.push('<div class="layui-row">');
      for (var key in dateTimeItems) {
        conditionHtml.push('<div class="layui-col-sm4"><input type="radio" name="datetime' + tableId + field + '" lay-filter="datetime' + tableId + '" value="' + key + '" title="' + dateTimeItems[key] + '"></div>');
      }
      conditionHtml.push('</div>');
      conditionHtml.push('<div><input type="radio" name="datetime' + tableId + field + '" lay-filter="datetime' + tableId + '"  value="specific" title="过滤具体日期"> <input type="hidden" class="specific_value"><div class="staticDate"></div></div></div>');
      $('#soul-condition' + tableId).html(conditionHtml.join(''));
      var filterDate = util.toDateString(new Date(), 'yyyy-MM-dd');
      if (filterSo) {
        $('#soul-condition' + tableId).data({'id': filterSo.id, 'head': true});
        $('#soul-condition' + tableId + '>.' + field + 'Condition' + ' [name^=datetime][value="' + filterSo.type + '"]').prop('checked', true);
        if (filterSo.type === 'specific') {
          filterDate = filterSo.value;
        }
      } else {
        $('#soul-condition' + tableId).data({'id': '', 'head': true});
        $('#soul-condition' + tableId + '>.' + field + 'Condition' + ' [name^=datetime][value="all"]').prop('checked', true);
      }

      $('#soul-condition' + tableId + ' .specific_value').val(filterDate);
      laydate.render({
        elem: '#soul-condition' + tableId + ' .staticDate'
        , position: 'static'
        , calendar: true
        , btns: ['now']
        , value: filterDate
        , done: function (value) {
          var id = $('#soul-condition' + tableId).data('id'),
            head = $('#soul-condition' + tableId).data('head');
          $('#soul-condition' + tableId + ' .specific_value').val(value);
          $('#soul-condition' + tableId + ' [name^=datetime]:checked').prop('checked', false);
          $('#soul-condition' + tableId + ' [name^=datetime][value=specific]').prop('checked', true);
          var filterSo = {
            id: id,
            head: head,
            prefix: head ? 'and' : 'and',
            mode: 'date',
            field: field,
            type: 'specific',
            value: value
          };
          _this.updateWhere(myTable, filterSo);
          if (!id) {
            $('#soul-condition' + tableId).data('id', filterSo.id);
          }
          if ($('.soul-edit-out').length > 0) {
            $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-value').html(filterSo.value);
          }
          if (refresh) {
            _this.soulReload(myTable);
          }
          form.render('radio', 'orm');
        }
      });
      form.on('radio(datetime' + tableId + ')', function (data) {
        var id = $('#soul-condition' + tableId).data('id'),
          head = $('#soul-condition' + tableId).data('head');
        var filterSo = {
          id: id,
          head: head,
          prefix: head ? 'and' : 'and',
          mode: 'date',
          field: field,
          type: data.value,
          value: $('#soul-condition' + tableId + ' .specific_value').val()
        };
        _this.updateWhere(myTable, filterSo);
        if (!id) {
          $('#soul-condition' + tableId).data('id', filterSo.id);
        }
        if ($('.soul-edit-out').length > 0) {
          $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-value').html(dateTimeItems[filterSo.type] || filterSo.value);
        }
        if (refresh) {
          _this.soulReload(myTable);
        }
      });
      form.render('radio', 'orm');
      if (type === 'down') {
        if (left + $('#soul-condition' + tableId).width() < documentWidth) {
          animate = 'fadeInLeft';
        } else {
          left = left - $('#main-list' + tableId).width() - $('#soul-condition' + tableId).width();
          animate = 'fadeInRight';
        }
      } else {
        top = top - $('#soul-condition' + tableId).outerHeight() - 10;
      }
      $('#soul-condition' + tableId).css({'top': top, 'left': left})
        .show().removeClass().addClass(animate + ' animated');

    }
    , bottomConditionHtml: function (bcHtml, filterSo, fieldMap, first) {
      var _this = this,
        isOr = filterSo.prefix === 'or',
        field = filterSo.field;

      if (filterSo.mode === 'group') {
        if (filterSo.children && filterSo.children.length > 0) {
          bcHtml.push('<div class="condition-item" data-id="' + filterSo.id + '" data-prefix="' + (filterSo.prefix || 'and') + '">');
          if (!first) {
            bcHtml.push('<div class="item-prefix layui-red">' + (isOr ? '或' : '与') + '</div> ');
          }

          for (var i = 0; i < filterSo.children.length; i++) {
            _this.bottomConditionHtml(bcHtml, filterSo.children[i], fieldMap, i === 0);
          }
          bcHtml.push('<i class="condition-item-close soul-icon soul-icon-unfold" ></i>');
          bcHtml.push('</div>');
        }
        return;
      }
      bcHtml.push('<div class="condition-item" data-field="' + field + '" data-id="' + filterSo.id + '" data-mode="' + filterSo.mode + '" data-type="' + filterSo.type + '" data-value="' + (typeof filterSo.value === 'undefined' ? '' : filterSo.value) + '" data-prefix="' + (filterSo.prefix || 'and') + '">');
      if (!first) {
        bcHtml.push('<div class="item-prefix layui-red">' + (isOr ? '或' : '与') + '</div> ');
      }
      bcHtml.push('<div class="item-field layui-firebrick">' + fieldMap[field].title + '</div> ');
      bcHtml.push('<div class="item-type layui-deeppink">');
      switch (filterSo.mode) {
        case 'in':
          bcHtml.push('筛选数据');
          break;
        case 'condition':
          bcHtml.push(conditionChangeItems[filterSo.type]);
          break;
        case 'date':
          bcHtml.push('选择日期');
          break;
        default:
          bcHtml.push('未知');
          break;
      }
      bcHtml.push('</div> ');
      if (filterSo.type !== 'null' && filterSo.type !== 'notNull') {
        bcHtml.push('<div class="item-value layui-blueviolet ' + (filterSo.mode === 'date' && filterSo.type !== 'specific') + '">');
        switch (filterSo.mode) {
          case 'in':
            bcHtml.push('共' + (filterSo.values ? filterSo.values.length : 0) + '条数据');
            break;
          case 'date':
            bcHtml.push(filterSo.type === 'specific' ? filterSo.value || '请选择' : dateTimeItems[filterSo.type]);
            break;
          case 'condition':
          default:
            bcHtml.push(typeof filterSo.value === 'undefined' || filterSo.value === '' ? '请输入...' : filterSo.value);
            break;
        }

        bcHtml.push('</div>');
      }
      bcHtml.push('<i class="condition-item-close soul-icon soul-icon-unfold" ></i>');
      bcHtml.push('</div>');
    }
    , renderBottomCondition: function (myTable) {
      var _this = this,
        where = where_cache[myTable.id] || {},
        filterSos = where.filterSos ? JSON.parse(where.filterSos) : [],
        tableFilterTypes = where.tableFilterType ? JSON.parse(where.tableFilterType) : {},
        $table = $(myTable.elem),
        tableId = myTable.id,
        $bottomCondition = $table.next().children('.soul-bottom-contion'),
        fieldMap = {}, bcHtml = [], curItems,
        filterItems = myTable.filter ? myTable.filter.items || defaultFilterItems : defaultFilterItems,
        columns = _this.getCompleteCols(myTable.cols);
      for (var i = 0; i < columns.length; i++) {
        if (columns[i].field && columns[i].filter) {
          curItems = columns[i].filter.items || filterItems;
          if (curItems.indexOf('data') !== -1 || curItems.indexOf('condition') !== -1) {
            fieldMap[columns[i]['field']] = {
              title: columns[i].title,
              items: curItems
            };
          }
        }
      }

      /**
       * 一、拼装底部内容
       */
      for (var i = 0; i < filterSos.length; i++) {
        _this.bottomConditionHtml(bcHtml, filterSos[i], fieldMap, i === 0);
      }
      $bottomCondition.children('.condition-items').html(bcHtml.join(''));

      /**
       * 二、组装底部弹窗条件
       */
      bcHtml = [];
      // 1. prefix
      if ($('#soul-bf-prefix' + tableId).length === 0) {
        bcHtml.push('<div id="soul-bf-prefix' + tableId + '" style="display: none;"><ul>');
        bcHtml.push('<li data-value="and">与</li>');
        bcHtml.push('<li data-value="or">或</li>');
        bcHtml.push('</ul></div>');
      }
      // 2. 列选择
      if ($('#soul-bf-column' + tableId).length === 0) {
        bcHtml.push('<div id="soul-bf-column' + tableId + '" style="display: none;"><ul>');
        for (var field in fieldMap) {
          bcHtml.push('<li data-field="' + field + '">' + fieldMap[field].title + '</li>');
        }
        bcHtml.push('</ul></div>');
      }

      // 3. 条件选择
      if ($('#soul-bf-type' + tableId).length === 0) {
        bcHtml.push('<div id="soul-bf-type' + tableId + '" style="display: none;"><ul>');
        bcHtml.push('<li data-value="in" data-mode="in">筛选数据</li>');
        bcHtml.push('<li data-value="all" data-mode="date">选择日期</li>');
        for (var key in conditionChangeItems) {
          bcHtml.push('<li data-value="' + key + '" data-mode="condition">' + conditionChangeItems[key] + '</li>');
        }
        bcHtml.push('</ul></div>');
      }

      // 4. 值选择
      if ($('#soul-bf-cond2-dropList' + tableId).length === 0) {
        bcHtml.push('<div id="soul-bf-cond2-dropList' + tableId + '" style="display: none;"><div class="filter-search"><input type="text" placeholder="关键字搜索" class="layui-input"></div><div class="check"><div class="multiOption" data-type="all"><i class="soul-icon">&#xe623;</i> 全选</div><div class="multiOption" data-type="none"><i class="soul-icon">&#xe63e;</i> 清空</div><div class="multiOption" data-type="reverse"><i class="soul-icon">&#xe614;</i>反选</div></div><ul></ul></div>');
      }


      $('body').append(bcHtml.join(''));

      /**
       * 三、底部弹窗事件
       */
      // 1. prefix弹出事件
      $bottomCondition.find('.item-prefix').off('click').on('click', function (e) {
        e.stopPropagation();
        $('#main-list' + tableId).hide();
        _this.hideDropList(myTable);
        _this.hideCondition(myTable);
        _this.hideColumns(myTable);
        _this.hideBfColumn(myTable);
        _this.hideBfType(myTable);
        var top = $(this).offset().top - $('#soul-bf-prefix' + tableId).outerHeight() - 10,
          left = $(this).offset().left;

        $('#soul-bf-prefix' + tableId).find('li.soul-bf-selected').removeClass('soul-bf-selected');
        $('#soul-bf-prefix' + tableId)
          .data('id', $(this).parent().data('id'))
          .data('prefix', $(this).parent().data('prefix'))
          .data('refresh', true)
          .show()
          .css({top: top, left: left})
          .removeClass().addClass('fadeInUp animated')
          .find('li[data-value="' + $(this).parent().data('prefix') + '"]')
          .addClass('soul-bf-selected');

      });
      // 2. 弹出列选择
      $bottomCondition.find('.item-field').off('click').on('click', function (e) {
        e.stopPropagation();
        $('#main-list' + tableId).hide();
        _this.hideDropList(myTable);
        _this.hideCondition(myTable);
        _this.hideColumns(myTable);
        _this.hideBfPrefix(myTable);
        _this.hideBfType(myTable);
        var top = $(this).offset().top - $('#soul-bf-column' + tableId).outerHeight() - 10,
          left = $(this).offset().left;

        $('#soul-bf-column' + tableId).find('li.soul-bf-selected').removeClass('soul-bf-selected');
        $('#soul-bf-column' + tableId)
          .data('field', $(this).parent().data('field'))
          .data('id', $(this).parent().data('id'))
          .data('mode', $(this).parent().data('mode'))
          .data('group', $(this).parent().parent().data('id') || '')
          .data('refresh', true)
          .show()
          .css({top: top, left: left})
          .removeClass().addClass('fadeInUp animated')
          .find('li[data-field="' + $(this).parent().data('field') + '"]')
          .addClass('soul-bf-selected');
      });

      // 3. 弹出方式选择
      $bottomCondition.find('.item-type').on('click', function (e) {
        e.stopPropagation();
        $('#main-list' + tableId).hide();
        _this.hideDropList(myTable);
        _this.hideCondition(myTable);
        _this.hideColumns(myTable);
        _this.hideBfColumn(myTable);
        _this.hideBfPrefix(myTable);
        var field = $(this).parent().data('field');
        $('#soul-bf-type' + tableId + ' li').hide();
        if (tableFilterTypes[field] && tableFilterTypes[field].indexOf('date') === 0) {
          $('#soul-bf-type' + tableId + ' li[data-mode=date]').show();
        }
        if (fieldMap[field].items.indexOf('data') !== -1) {
          $('#soul-bf-type' + tableId + ' li[data-mode=in]').show();
        }
        if (fieldMap[field].items.indexOf('condition') !== -1) {
          $('#soul-bf-type' + tableId + ' li[data-mode=condition]').show();
        }

        var top = $(this).offset().top - $('#soul-bf-type' + tableId).outerHeight() - 10,
          left = $(this).offset().left;
        $('#soul-bf-type' + tableId).find('li.soul-bf-selected').removeClass('soul-bf-selected');
        switch ($(this).parent().data('mode')) {
          case 'in':
            $('#soul-bf-type' + tableId).find('li[data-mode="in"]')
              .addClass('soul-bf-selected');
            break;
          case 'date':
            $('#soul-bf-type' + tableId).find('li[data-mode="date"]')
              .addClass('soul-bf-selected');
          case 'condition':
            $('#soul-bf-type' + tableId).find('li[data-value="' + $(this).parent().data('type') + '"]')
              .addClass('soul-bf-selected');
        }

        $('#soul-bf-type' + tableId)
          .data('type', $(this).parent().data('type'))
          .data('mode', $(this).parent().data('mode'))
          .data('id', $(this).parent().data('id'))
          .data('group', $(this).parent().parent().data('id') || '')
          .data('refresh', true)
          .show()
          .css({top: top, left: left})
          .removeClass().addClass('fadeInUp animated');
      });

      // 4. 弹出值选择
      $bottomCondition.find('.item-value').on('click', function (e) {
        e.stopPropagation();
        $('#main-list' + tableId).hide();
        _this.hideColumns(myTable);
        _this.hideBfType(myTable);
        _this.hideBfPrefix(myTable);
        _this.hideBfColumn(myTable);
        var top,
          left = $(this).offset().left,
          mode = $(this).parent().data('mode'),
          field = $(this).parent().data('field'),
          id = $(this).parent().data('id'),
          head = $(this).parent().data('head'),
          prefix = $(this).parent().data('prefix');

        switch (mode) {
          case 'in':
            _this.hideCondition(myTable);
            if (dorpListTimeOut) {
              clearTimeout(dorpListTimeOut);
            }
            $('#soul-dropList' + tableId + '>.filter-search>input').val('');
            $('#soul-dropList' + tableId).show();
            $('#soulDropList' + tableId).find('.' + field + 'DropList li input[type=checkbox]:checked').prop('checked', false);
            var filterSo = _this.getFilterSoById(filterSos, id);
            for (var i = 0; i < filterSo.values.length; i++) {
              $('#soulDropList' + tableId).find('.' + field + 'DropList li input[type=checkbox][value="' + filterSo.values[i] + '"]').prop('checked', true);
            }

            $('#soul-dropList' + tableId + '>ul').data('id', id).data('head', head).data('refresh', true).data('prefix', prefix).html($('#soulDropList' + tableId).find('.' + field + 'DropList li').clone());
            form.render('checkbox', 'orm');
            top = $(this).offset().top - $('#soul-dropList' + tableId).outerHeight() - 10;
            $('#soul-dropList' + tableId).css({'top': top, 'left': left})
              .show().removeClass().addClass('fadeInUp animated');
            setTimeout(function () {
              $('#soul-dropList' + tableId + '>.filter-search>input').focus(); // 聚焦搜索框
            }, 1);


            // 监听筛选数据
            var liClick = true;
            form.on('checkbox(soulDropList' + tableId + ')', function (data) {
              liClick = false;
              _this.updateDropList(myTable, field);
            });

            $('#soul-dropList' + tableId + '>ul>li[data-value]').on('click', function () {
              if (liClick) {
                $(this).find('div.layui-form-checkbox').trigger('click');
              }
              liClick = true;
            });
            break;
          case 'date':
            _this.hideDropList(myTable);
            if (conditionTimeOut) {
              clearTimeout(conditionTimeOut);
            }
            var filterSo = _this.getFilterSoById(filterSos, id),
              top = $(this).offset().top - 10;

            _this.showDate(myTable, field, filterSo, "fadeInUp", top, left, "up", true);
            break;
          default:
            _this.hideDropList(myTable);
            if (conditionTimeOut) {
              clearTimeout(conditionTimeOut);
            }
            var obj = this,
              value = $(this).parents('.condition-item:eq(0)').data('value');
            $(obj).hide();
            $(obj).after('<div><input style="height: 25px;" class="layui-input tempValue" value="" /></div>');
            $(obj).next().children().val(value).select().on('keydown', function (e) {
              if (e.keyCode === 13) {
                $(this).blur();
              }
            }).on('blur', function () {
              var newValue = $(this).val();
              $(obj).html(typeof newValue === 'undefined' || newValue === '' ? '请输入...' : newValue);
              $(obj).show();
              $(this).parent().remove();
              if (newValue !== value) {
                _this.updateWhere(myTable, {
                  id: id,
                  value: newValue
                });
                _this.soulReload(myTable);
              }
            });

            break;
        }
      });

      /**
       * 三、选择事件
       */
      // 1. 选择prefix
      $('#soul-bf-prefix' + tableId + '>ul>li').off('click').on('click', function () {
        var id = $(this).parent().parent().data('id'),
          newPrefix = $(this).data('value'),
          oldPrefix = $(this).parent().parent().data('prefix'),
          refresh = $(this).parent().parent().data('refresh');

        if (oldPrefix !== newPrefix) {
          _this.updateWhere(myTable, {
            id: id,
            prefix: newPrefix
          });
          if (refresh === true) {
            _this.soulReload(myTable);
          }
        }
      });
      // 1. 选择列
      $('#soul-bf-column' + tableId + '>ul>li').off('click').on('click', function () {
        var oldField = $(this).parent().parent().data('field'),
          newField = $(this).data('field'),
          mode = $(this).parent().parent().data('mode'),
          group = $(this).parent().parent().data('group'),
          refresh = $(this).parent().parent().data('refresh');

        if (oldField !== newField) {
          var filterSo = {
            id: $(this).parent().parent().data('id'),
            field: newField
          };
          if (fieldMap[newField].items.indexOf(modeMapItems[mode]) === -1) {
            $.extend(filterSo, $.extend({}, revertMode[modeMapItems[mode]],
              revertMode[modeMapItems[mode]].mode === 'condition' && _this.startsWith(tableFilterTypes[newField], 'date')
                ? {
                  mode: 'date',
                  type: 'all'
                } : {}));
          } else {
            // 重置values值
            if (mode === 'in') {
              $.extend(filterSo, {
                values: []
              });
            } else if (mode === 'date' && !(_this.startsWith(tableFilterTypes[newField], 'date'))) {
              $.extend(filterSo, {
                mode: 'condition',
                type: 'eq',
                value: ''
              });
            } else if (mode !== 'date' && _this.startsWith(tableFilterTypes[newField], 'date')) {
              $.extend(filterSo, {
                mode: 'date',
                type: 'all'
              });
            }
          }
          // 如果是头部条件，选择列是清除
          if (group) {
            _this.updateWhere(myTable, {
              id: group,
              head: false
            });
          }
          _this.updateWhere(myTable, filterSo);

          if ($('.soul-edit-out').length > 0) {
            $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-field').html(fieldMap[newField].title);
            if (filterSo.mode === 'in') {
              $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-type').html('筛选数据');
              $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-value').html('共0条数据');
            } else if (mode !== filterSo.mode) {
              if (filterSo.mode === 'date') {
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-type').html('选择日期');
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-value').html(dateTimeItems[filterSo.type]);
              } else if (filterSo.mode === 'condition') {
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-type').html(conditionChangeItems[filterSo.type]);
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').children('.item-value').html(filterSo.value === '' ? '请输入...' : filterSo.value);
              }
            }
          }

          if (refresh === true) {
            _this.soulReload(myTable);
          }
        }
      });

      // 2. 选择类型
      $('#soul-bf-type' + tableId + '>ul>li').off('click').on('click', function () {
        var newType = $(this).data('value') + "" // 引号修复为空（null值）传递问题
          , newMode = $(this).data('mode')
          , type = $(this).parent().parent().data('type')
          , mode = $(this).parent().parent().data('mode')
          , group = $(this).parent().parent().data('group')
          , refresh = $(this).parent().parent().data('refresh');
        if (type !== newType) {

          var filterSo = {
            id: $(this).parent().parent().data('id'),
            type: newType,
            mode: newMode
          };
          if (mode !== newMode) {
            $.extend(filterSo, {
              value: '',
              values: []
            });
          }

          // 如果是头部条件，选择列是清除
          if (group && newMode === 'in') {
            _this.updateWhere(myTable, {
              id: group,
              head: false
            });
          }
          _this.updateWhere(myTable, filterSo);

          if ($('.soul-edit-out').length > 0) {
            $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-value').show();
            $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-type').html(conditionChangeItems[newType] || (newMode === 'in' ? '筛选数据' : '选择日期'));
            switch (newMode) {
              case 'in':
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-value').html('共0条数据');
                break;
              case 'date':
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-value').html(dateTimeItems[newType]);
                break;
              case 'condition':
                if (mode !== newMode) {
                  $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-value').html('请输入...');
                }
                $('.soul-edit-out li[data-id="' + filterSo.id + '"]').data(filterSo).children('.item-value')[newType === 'null' || newType === 'notNull' ? 'hide' : 'show']();

                break;
            }
          }

          // 是否立即更新
          if (refresh === true) {
            _this.soulReload(myTable);
          }
        }
      });

      /**
       * 五、底部筛选条件删除事件
       */
      $bottomCondition.find('.condition-items .condition-item .condition-item-close').on('click', function () {
        _this.updateWhere(myTable, {
          id: $(this).parents('.condition-item:eq(0)').data('id'),
          delete: true
        });
        _this.soulReload(myTable);
      });

    }
    /**
     * 导出 excel 文件
     * @param myTable
     * @param curExcel
     */
    , export: function (myTable, curExcel) {
      if (typeof myTable === 'string') {
        myTable = table_cache[myTable]; // tableId 转 myTable
      }
      var loading = layer.msg('文件下载中', {
        icon: 16
        , time: -1
        , anim: -1
        , fixed: false
      });
      var cols = this.deepClone(myTable.cols)
        , style = myTable.elem.next().find('style')[0]
        , sheet = style.sheet || style.styleSheet || {}
        , rules = sheet.cssRules || sheet.rules;

      layui.each(rules, function (i, item) {
        if (item.style.width) {
          var keys = item.selectorText.split('-');
          cols[keys[3]][keys[4]]['width'] = parseInt(item.style.width);
        }
      });

      var data = JSON.parse(JSON.stringify(myTable.data || layui.table.cache[myTable.id])),
        showField = {},
        widths = {},
        mergeArrays = [], // 合并配置
        heightConfig = {},
        $table = $(myTable.elem),
        $tableBody = $table.next().children('.layui-table-box').children('.layui-table-body').children('table'),
        mainExcel = typeof myTable.excel === 'undefined' || ((myTable.excel && (typeof myTable.excel.on === 'undefined' || myTable.excel.on)) ? myTable.excel : false);

      mainExcel = mainExcel === true ? {} : mainExcel || {};
      curExcel = curExcel || {};

      var filename = curExcel.filename ? (typeof curExcel.filename === 'function' ? curExcel.filename.call(this) : curExcel.filename)
        : mainExcel.filename ? (typeof mainExcel.filename === 'function' ? mainExcel.filename.call(this) : mainExcel.filename)
          : '表格数据.xlsx',
        checked = curExcel.checked === true ? true : mainExcel.checked === true,
        curPage = curExcel.curPage === true ? true : mainExcel.curPage === true,
        customColumns = typeof curExcel.columns === 'undefined' ? mainExcel.columns : curExcel.columns,
        totalRow = typeof curExcel.totalRow === 'undefined' ? mainExcel.totalRow : curExcel.totalRow,
        type = filename.substring(filename.lastIndexOf('.') + 1, filename.length),
        tableStartIndex = mainExcel.add && mainExcel.add.top && Array.isArray(mainExcel.add.top.data) ? mainExcel.add.top.data.length + 1 : 1,  //表格内容从哪一行开始
        bottomLength = mainExcel.add && mainExcel.add.bottom && Array.isArray(mainExcel.add.bottom.data) ? mainExcel.add.bottom.data.length : 0;// 底部自定义行数

      if (checked) { // 获取选中行数据
        data = table.checkStatus(myTable.id).data;
      } else if (curPage) {
        data = layui.table.cache[myTable.id];
      } else if (myTable.url && myTable.page) {
        var ajaxStatus = true;
        var searchParam = isFilterCache[myTable.id] ? where_cache[myTable.id] : table_cache[myTable.id].where;
        if (myTable.contentType && myTable.contentType.indexOf("application/json") === 0) { //提交 json 格式
          searchParam = JSON.stringify(searchParam);
        }
        $.ajax({
          url: myTable.url,
          data: searchParam,
          dataType: 'json',
          method: myTable.method || 'post',
          async: false,
          cache: false,
          headers: myTable.headers || {},
          contentType: myTable.contentType,
          success: function (res) {
            if (typeof myTable.parseData === 'function') {
              res = myTable.parseData(res) || res;
            }
            //检查数据格式是否符合规范
            if (res[myTable.response.statusName] != myTable.response.statusCode) {
              layer.msg('返回的数据不符合规范，正确的成功状态码应为："' + myTable.response.statusName + '": ' + myTable.response.statusCode, {
                icon: 2,
                anim: 6
              });
            } else {
              data = res[myTable.response.dataName];
            }
          },
          error: function (res) {
            layer.msg('请求异常！', {icon: 2, anim: 6});
            ajaxStatus = false;
          }
        });
        if (!ajaxStatus) {
          return;
        }
      } else {
        var $sortDoom = $table.next().children('.layui-table-box').children('.layui-table-header').find('.layui-table-sort[lay-sort$="sc"]:eq(0)');
        if ($sortDoom.length > 0) {
          var sortField = $sortDoom.parent().parent().data('field');
          var sortOrder = $sortDoom.attr('lay-sort');
          switch (sortOrder) {
            case 'asc':
              data = layui.sort(data, sortField);
              break;
            case 'desc':
              data = layui.sort(data, sortField, true);
              break;
            default:
              break;
          }
        }
      }

      // 制定显示列和顺序
      var i, j, k, tempArray, cloneCol, columnsMap = [], curRowUnShowCount;
      for (i = 0; i < cols.length; i++) {
        curRowUnShowCount = 0;
        for (j = 0; j < cols[i].length; j++) {
          if (!cols[i][j].exportHandled) {
            if (cols[i][j].rowspan > 1) {
              if ((cols[i][j].field || cols[i][j].type === 'numbers') && !cols[i][j].hide) {
                mergeArrays.push([numberToLetter(j + 1 - curRowUnShowCount) + (i + tableStartIndex), numberToLetter(j + 1 - curRowUnShowCount) + (i + parseInt(cols[i][j].rowspan) + tableStartIndex - 1)]);
              } else {
                curRowUnShowCount++;
              }
              cloneCol = this.deepClone(cols[i][j]);
              cloneCol.exportHandled = true;
              k = i + 1;
              while (k < cols.length) {
                cols[k].splice(j, 0, cloneCol);
                k++;
              }
            }
            if (cols[i][j].colspan > 1) {
              mergeArrays.push([numberToLetter(j + 1 - curRowUnShowCount) + (i + tableStartIndex), numberToLetter(j + parseInt(cols[i][j].colspan) - curRowUnShowCount) + (i + tableStartIndex)]);
              cloneCol = this.deepClone(cols[i][j]);
              cloneCol.exportHandled = true;
              for (k = 1; k < cols[i][j].colspan; k++) {
                cols[i].splice(j, 0, cloneCol);
              }
              j = j + parseInt(cols[i][j].colspan) - 1;

            }
          } else if (!((cols[i][j].field || cols[i][j].type === 'numbers') && !cols[i][j].hide)) {
            curRowUnShowCount++;
          }
        }
      }
      var columns = cols[cols.length - 1]; // 获取真实列

      // 处理数据
      for (i = 0; i < data.length; i++) {
        for (j = 0; j < columns.length; j++) {
          if ((columns[j].field || columns[j].type === 'numbers') && (customColumns && Array.isArray(customColumns) || !columns[j].hide)) {
            data[i][columns[j].key] = data[i][columns[j].field || columns[j]['LAY_TABLE_INDEX']];
          }
        }
      }

      // 处理合计行
      if (totalRow !== false && myTable.totalRow) {
        var obj = {}, totalRows = {};
        for (i = 0; i < columns.length; i++) {
          if (columns[i].totalRowText) {
            obj[columns[i].key] = columns[i].totalRowText;
          } else if (columns[i].totalRow) {
            totalRows[columns[i].key] = 0;
          }
        }
        if (JSON.stringify(totalRows) !== '{}') {
          for (i = 0; i < data.length; i++) {
            for (var key in totalRows) {
              totalRows[key] = (parseFloat(totalRows[key]) + (parseFloat(data[i][key]) || 0)).toFixed(2);
            }
          }
        }
        data.push(Object.assign(obj, totalRows));
      }

      if (customColumns && Array.isArray(customColumns)) {
        var tempCustomColumns = [];
        tempArray = {};
        mergeArrays = []; // 重置表头合并列
        columnsMap[0] = {};
        for (i = 0; i < customColumns.length; i++) {
          for (j = 0; j < columns.length; j++) {
            if (columns[j].field === customColumns[i]) {
              columns[j].hide = false;
              tempCustomColumns.push(columns[j]);
              columnsMap[0][columns[j].key] = columns[j];
              tempArray[columns[j].key] = columns[j].title;
              break;
            }
          }
        }
        columns = tempCustomColumns;
        data.splice(0, 0, tempArray);
      } else {
        // 拼接表头数据
        for (i = 0; i < cols.length; i++) {
          columnsMap[i] = {};
          tempArray = {};
          for (j = 0; j < cols[i].length; j++) {
            columnsMap[i][cols[cols.length - 1][j].key] = cols[i][j];
            tempArray[cols[cols.length - 1][j].key] = cols[i][j].title;
          }
          data.splice(i, 0, tempArray);
        }
      }

      //添加自定义内容
      if (mainExcel.add) {
        var addTop = mainExcel.add.top,
          addBottom = mainExcel.add.bottom,
          startPos, endPos, jumpColsNum;

        if (addTop && Array.isArray(addTop.data) && addTop.data.length > 0) {

          for (i = 0; i < addTop.data.length; i++) {
            tempArray = {}, jumpColsNum = 0;
            for (j = 0; j < (addTop.data[i].length > columns.length ? addTop.data[i].length : columns.length); j++) {
              if ((columns[j].field || columns[j].type === 'numbers') && !columns[j].hide) {
                tempArray[columns[j] ? columns[j].key : j + ''] = addTop.data[i][j - jumpColsNum] || '';
              } else {
                jumpColsNum++;
              }
            }
            data.splice(i, 0, tempArray);
          }

          if (Array.isArray(addTop.heights) && addTop.heights.length > 0) {
            for (i = 0; i < addTop.heights.length; i++) {
              heightConfig[i] = addTop.heights[i];
            }
          }

          if (Array.isArray(addTop.merge) && addTop.merge.length > 0) {
            for (i = 0; i < addTop.merge.length; i++) {
              if (addTop.merge[i].length === 2) {
                startPos = addTop.merge[i][0].split(',');
                endPos = addTop.merge[i][1].split(',');
                mergeArrays.push([numberToLetter(startPos[1]) + startPos[0], numberToLetter(endPos[1]) + endPos[0]]);
              }

            }
          }
        }
        if (addBottom && Array.isArray(addBottom.data) && addBottom.data.length > 0) {
          for (i = 0; i < addBottom.data.length; i++) {
            tempArray = {}, jumpColsNum = 0;
            for (j = 0; j < (addBottom.data[i].length > columns.length ? addBottom.data[i].length : columns.length); j++) {
              if ((columns[j].field || columns[j].type === 'numbers') && !columns[j].hide) {
                tempArray[columns[j] ? columns[j].key : j + ''] = addBottom.data[i][j - jumpColsNum] || '';
              } else {
                jumpColsNum++;
              }
            }
            data.push(tempArray);
          }

          if (Array.isArray(addBottom.heights) && addBottom.heights.length > 0) {
            for (i = 0; i < addBottom.heights.length; i++) {
              heightConfig[data.length - addBottom.data.length + i] = addBottom.heights[i];
            }
          }

          if (Array.isArray(addBottom.merge) && addBottom.merge.length > 0) {
            for (i = 0; i < addBottom.merge.length; i++) {
              if (addBottom.merge[i].length === 2) {
                startPos = addBottom.merge[i][0].split(',');
                endPos = addBottom.merge[i][1].split(',');
                mergeArrays.push([numberToLetter(startPos[1]) + (data.length - addBottom.data.length + parseInt(startPos[0])), numberToLetter(endPos[1]) + (data.length - addBottom.data.length + parseInt(endPos[0]))]);
              }
            }
          }
        }
      }

      var index = 0, alignTrans = {'left': 'left', 'center': 'center', 'right': 'right'},
        borderTypes = ['top', 'bottom', 'left', 'right'];
      for (i = 0; i < columns.length; i++) {
        if ((columns[i].field || columns[i].type === 'numbers') && !columns[i].hide) {
          if (columns[i].width) {
            widths[String.fromCharCode(64 + parseInt(++index))] = columns[i].width;
          }
          showField[columns[i].key] = function (field, line, data, curIndex) {
            var bgColor = 'ffffff', color = '000000', family = 'Calibri', size = 12, cellType = 's',
              bodyIndex = curIndex - (customColumns ? 1 : cols.length) - tableStartIndex + 1,
              border = {
                top: {
                  style: 'thin',
                  color: {indexed: 64}
                },
                bottom: {
                  style: 'thin',
                  color: {indexed: 64}
                },
                left: {
                  style: 'thin',
                  color: {indexed: 64}
                },
                right: {
                  style: 'thin',
                  color: {indexed: 64}
                }
              };
            if (mainExcel.border) {
              for (j = 0; j < borderTypes.length; j++) {
                if (mainExcel.border[borderTypes[j]]) {
                  border[borderTypes[j]].style = mainExcel.border[borderTypes[j]].style || border[borderTypes[j]].style;
                  border[borderTypes[j]].color = handleRgb(mainExcel.border[borderTypes[j]].color) || border[borderTypes[j]].color;
                } else if (mainExcel.border['color'] || mainExcel.border['style']) {
                  border[borderTypes[j]].style = mainExcel.border['style'] || border[borderTypes[j]].style;
                  border[borderTypes[j]].color = handleRgb(mainExcel.border['color']) || border[borderTypes[j]].color;
                }
              }
            }
            if (curIndex < tableStartIndex - 1 || curIndex >= data.length - bottomLength) {
              return {
                v: line[field] || '',
                s: {// s 代表样式
                  alignment: {
                    horizontal: 'center',
                    vertical: 'center'
                  },
                  font: {name: family, sz: size, color: {rgb: color}},
                  fill: {
                    fgColor: {rgb: bgColor, bgColor: {indexed: 64}}
                  },
                  border: border
                },
                t: cellType
              };
            } else if (bodyIndex < 0) {
              bgColor = 'C7C7C7';
              if (mainExcel.head) {
                bgColor = mainExcel.head.bgColor || bgColor;
                color = mainExcel.head.color || color;
                family = mainExcel.head.family || family;
                size = mainExcel.head.size || size;
              }
              if (curExcel.head) {
                bgColor = curExcel.head.bgColor || bgColor;
                color = curExcel.head.color || color;
                family = curExcel.head.family || family;
                size = curExcel.head.size || size;
              }
            } else {
              if (mainExcel.font) {
                bgColor = mainExcel.font.bgColor || bgColor;
                color = mainExcel.font.color || color;
                family = mainExcel.font.family || family;
                size = mainExcel.font.size || size;
              }
              if (curExcel.font) {
                bgColor = curExcel.font.bgColor || bgColor;
                color = curExcel.font.color || color;
                family = curExcel.font.family || family;
                size = curExcel.head.size || size;
              }
              if (curExcel.border) {
                for (j = 0; j < borderTypes.length; j++) {
                  if (curExcel.border[borderTypes[j]]) {
                    border[borderTypes[j]].style = curExcel.border[borderTypes[j]].style || border[borderTypes[j]].style;
                    border[borderTypes[j]].color = handleRgb(curExcel.border[borderTypes[j]].color) || border[borderTypes[j]].color;
                  } else if (curExcel.border['color'] || curExcel.border['style']) {
                    border[borderTypes[j]].style = curExcel.border['style'] || border[borderTypes[j]].style;
                    border[borderTypes[j]].color = handleRgb(curExcel.border['color']) || border[borderTypes[j]].color;
                  }
                }
              }
              if (columnsMap[columnsMap.length - 1][field].excel) {
                var colExcel = typeof columnsMap[columnsMap.length - 1][field].excel === 'function' ? columnsMap[columnsMap.length - 1][field].excel.call(this, line, bodyIndex, data.length - cols.length - tableStartIndex + 1 - bottomLength) : columnsMap[columnsMap.length - 1][field].excel;
                if (colExcel) {
                  bgColor = colExcel.bgColor || bgColor;
                  color = colExcel.color || color;
                  family = colExcel.family || family;
                  size = colExcel.size || size;
                  cellType = colExcel.cellType || cellType;

                  if (colExcel.border) {
                    for (j = 0; j < borderTypes.length; j++) {
                      if (colExcel.border[borderTypes[j]]) {
                        border[borderTypes[j]].style = colExcel.border[borderTypes[j]].style || border[borderTypes[j]].style;
                        border[borderTypes[j]].color = handleRgb(colExcel.border[borderTypes[j]].color) || border[borderTypes[j]].color;
                      } else if (colExcel.border['color'] || colExcel.border['style']) {
                        border[borderTypes[j]].style = colExcel.border['style'] || border[borderTypes[j]].style;
                        border[borderTypes[j]].color = handleRgb(colExcel.border['color']) || border[borderTypes[j]].color;
                      }
                    }
                  }
                }
              }
            }

            function handleNull(val) {
              if (typeof val === 'undefined' || val === null) {
                return "";
              }
              return val;
            }

            var value = bodyIndex >= 0 && columnsMap[columnsMap.length - 1][field].templet ?
              typeof columnsMap[columnsMap.length - 1][field].templet === 'function' ?
                $('<div>' + columnsMap[columnsMap.length - 1][field].templet(line) + '</div>').find(':input').length === 0 ? $('<div>' + columnsMap[columnsMap.length - 1][field].templet(line) + '</div>').text() : $tableBody.children('tbody').children('tr[data-index=' + bodyIndex + ']').children('td[data-field="' + field + '"]').find(':input').val() || handleNull(line[field])
                : $('<div>' + laytpl($(columnsMap[columnsMap.length - 1][field].templet).html() || String(columnsMap[columnsMap.length - 1][field].templet)).render(line) + '</div>').find(':input').length === 0 ? $('<div>' + laytpl($(columnsMap[columnsMap.length - 1][field].templet).html() || String(columnsMap[columnsMap.length - 1][field].templet)).render(line) + '</div>').text() : $tableBody.children('tbody').children('tr[data-index=' + bodyIndex + ']').children('td[data-field="' + field + '"]').find(':input').val() || handleNull(line[field])
              : bodyIndex >= 0 && columnsMap[columnsMap.length - 1][field].type === 'numbers' ? bodyIndex + 1 : handleNull(line[field]);
            return {
              v: value,// v 代表单元格的值
              s: {// s 代表样式
                alignment: {
                  horizontal: columnsMap[bodyIndex < -1 ? curIndex - tableStartIndex + 1 : columnsMap.length - 1][field].align ? alignTrans[columnsMap[bodyIndex < -1 ? curIndex - tableStartIndex + 1 : columnsMap.length - 1][field].align] : 'top',
                  vertical: 'center'
                },
                font: {name: family, sz: size, color: {rgb: color}},
                fill: {
                  fgColor: {rgb: bgColor, bgColor: {indexed: 64}}
                },
                border: border
              },
              t: UNHANDLED_VALUES.indexOf(value) === -1 ? cellType : 's'
            };
          };
        }
      }

      excel.exportExcel({
        sheet1: excel.filterExportData(data, showField)
      }, filename, type, {
        extend: {
          '!cols': excel.makeColConfig(widths, 80),
          '!merges': excel.makeMergeConfig(mergeArrays),
          '!rows': excel.makeRowConfig(heightConfig, 16)
        }
      });
      layer.close(loading);

      // 合成 excel.js 识别的 rgb
      function handleRgb(rgb) {
        return rgb ? {rgb: rgb} : rgb;
      }

      function numberToLetter(num) {
        var result = [];
        while (num) {
          var t = num % 26;
          if (!t) {
            t = 26;
            --num;
          }
          // Polyfill 兼容旧浏览器
          if (!String.fromCodePoint) (function (stringFromCharCode) {
            var fromCodePoint = function (_) {
              var codeUnits = [], codeLen = 0, result = "";
              for (var index = 0, len = arguments.length; index !== len; ++index) {
                var codePoint = +arguments[index];
                // correctly handles all cases including `NaN`, `-Infinity`, `+Infinity`
                // The surrounding `!(...)` is required to correctly handle `NaN` cases
                // The (codePoint>>>0) === codePoint clause handles decimals and negatives
                if (!(codePoint < 0x10FFFF && (codePoint >>> 0) === codePoint))
                  throw RangeError("Invalid code point: " + codePoint);
                if (codePoint <= 0xFFFF) { // BMP code point
                  codeLen = codeUnits.push(codePoint);
                } else { // Astral code point; split in surrogate halves
                  // https://mathiasbynens.be/notes/javascript-encoding#surrogate-formulae
                  codePoint -= 0x10000;
                  codeLen = codeUnits.push(
                    (codePoint >> 10) + 0xD800,  // highSurrogate
                    (codePoint % 0x400) + 0xDC00 // lowSurrogate
                  );
                }
                if (codeLen >= 0x3fff) {
                  result += stringFromCharCode.apply(null, codeUnits);
                  codeUnits.length = 0;
                }
              }
              return result + stringFromCharCode.apply(null, codeUnits);
            };
            try { // IE 8 only supports `Object.defineProperty` on DOM elements
              Object.defineProperty(String, "fromCodePoint", {
                "value": fromCodePoint, "configurable": true, "writable": true
              });
            } catch (e) {
              String.fromCodePoint = fromCodePoint;
            }
          }(String.fromCharCode));
          result.push(String.fromCodePoint(t + 64));
          if (!String.fromCodePoint) (function (stringFromCharCode) {
            var fromCodePoint = function (_) {
              var codeUnits = [], codeLen = 0, result = "";
              for (var index = 0, len = arguments.length; index !== len; ++index) {
                var codePoint = +arguments[index];
                // correctly handles all cases including `NaN`, `-Infinity`, `+Infinity`
                // The surrounding `!(...)` is required to correctly handle `NaN` cases
                // The (codePoint>>>0) === codePoint clause handles decimals and negatives
                if (!(codePoint < 0x10FFFF && (codePoint >>> 0) === codePoint))
                  throw RangeError("Invalid code point: " + codePoint);
                if (codePoint <= 0xFFFF) { // BMP code point
                  codeLen = codeUnits.push(codePoint);
                } else { // Astral code point; split in surrogate halves
                  // https://mathiasbynens.be/notes/javascript-encoding#surrogate-formulae
                  codePoint -= 0x10000;
                  codeLen = codeUnits.push(
                    (codePoint >> 10) + 0xD800,  // highSurrogate
                    (codePoint % 0x400) + 0xDC00 // lowSurrogate
                  );
                }
                if (codeLen >= 0x3fff) {
                  result += stringFromCharCode.apply(null, codeUnits);
                  codeUnits.length = 0;
                }
              }
              return result + stringFromCharCode.apply(null, codeUnits);
            };
            try { // IE 8 only supports `Object.defineProperty` on DOM elements
              Object.defineProperty(String, "fromCodePoint", {
                "value": fromCodePoint, "configurable": true, "writable": true
              });
            } catch (e) {
              String.fromCodePoint = fromCodePoint;
            }
          }(String.fromCharCode));
          num = ~~(num / 26);
        }
        return result.reverse().join('');
      }
    },
    startsWith: function (content, str) {
      var reg = new RegExp("^" + str);
      return content && reg.test(content);
    },
    // 深度克隆-不丢失方法
    deepClone: function (obj) {
      var newObj = Array.isArray(obj) ? [] : {};
      if (obj && typeof obj === "object") {
        for (var key in obj) {
          if (obj.hasOwnProperty(key)) {
            newObj[key] = (obj && typeof obj[key] === 'object') ? this.deepClone(obj[key]) : obj[key];
          }
        }
      }
      return newObj;
    },
    deepStringify: function (obj) {
      var JSON_SERIALIZE_FIX = {
        PREFIX: "[[JSON_FUN_PREFIX_",
        SUFFIX: "_JSON_FUN_SUFFIX]]"
      };
      return JSON.stringify(obj, function (key, value) {
        if (typeof value === 'function') {
          return JSON_SERIALIZE_FIX.PREFIX + value.toString() + JSON_SERIALIZE_FIX.SUFFIX;
        }
        return value;
      });
    },
    /* layui table 中原生的方法 */
    getScrollWidth: function (elem) {
      var width = 0;
      if (elem) {
        width = elem.offsetWidth - elem.clientWidth;
      } else {
        elem = document.createElement('div');
        elem.style.width = '100px';
        elem.style.height = '100px';
        elem.style.overflowY = 'scroll';

        document.body.appendChild(elem);
        width = elem.offsetWidth - elem.clientWidth;
        document.body.removeChild(elem);
      }
      return width;
    }
    , getCompleteCols: function (origin) {
      var cols = this.deepClone(origin);
      var i, j, k, cloneCol;
      for (i = 0; i < cols.length; i++) {
        for (j = 0; j < cols[i].length; j++) {
          if (!cols[i][j].exportHandled) {
            if (cols[i][j].rowspan > 1) {
              cloneCol = this.deepClone(cols[i][j]);
              cloneCol.exportHandled = true;
              k = i + 1;
              while (k < cols.length) {
                cols[k].splice(j, 0, cloneCol);
                k++;
              }
            }
            if (cols[i][j].colspan > 1) {
              cloneCol = this.deepClone(cols[i][j]);
              cloneCol.exportHandled = true;
              for (k = 1; k < cols[i][j].colspan; k++) {
                cols[i].splice(j, 0, cloneCol);
              }
              j = j + parseInt(cols[i][j].colspan) - 1;
            }
          }
        }
      }
      return cols[cols.length - 1];
    }
    , parseTempData: function (item3, content, tplData, text) { //表头数据、原始内容、表体数据、是否只返回文本
      var str = item3.templet ? function () {
        return typeof item3.templet === 'function'
          ? item3.templet(tplData)
          : laytpl($(item3.templet).html() || String(content)).render(tplData);
      }() : content;
      return text ? $('<div>' + str + '</div>').text() : str;
    }
    , cache: cache
  };

  // 输出
  exports('tableFilter', mod);
});
