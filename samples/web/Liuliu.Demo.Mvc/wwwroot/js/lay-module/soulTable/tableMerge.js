/**
 *
 * @name:  子表格扩展
 * @author: yelog
 * @link: https://github.com/yelog/layui-soul-table
 * @license: MIT
 * @version: v1.5.16
 */
layui.define(['table'], function (exports) {

  var $ = layui.jquery;

  // 封装方法
  var mod = {
    /**
     * 渲染入口
     * @param myTable
     */
    render: function (myTable) {
      var tableBox = $(myTable.elem).next().children('.layui-table-box'),
        $main = $(tableBox.children('.layui-table-body').children('table').children('tbody').children('tr').toArray().reverse()),
        $fixLeft = $(tableBox.children('.layui-table-fixed-l').children('.layui-table-body').children('table').children('tbody').children('tr').toArray().reverse()),
        $fixRight = $(tableBox.children('.layui-table-fixed-r').children('.layui-table-body').children('table').children('tbody').children('tr').toArray().reverse()),
        mergeRecord = {};

      layui.each(myTable.cols, function (i1, item1) {
        layui.each(item1, function (i2, item2) {
          if (item2.merge && item2.field) {
            var mergeField = [item2.field];
            if (item2.merge !== true) {
              if (typeof item2.merge === 'string') {
                mergeField = [item2.merge]
              } else {
                mergeField = item2.merge
              }
            }
            mergeRecord[myTable.index + '-' + i1 + '-' + i2] = {mergeField: mergeField, rowspan: 1}
          }
        })
      })

      $main.each(function (i) {

        for (var item in mergeRecord) {
          if (i === $main.length - 1 || isMaster(i, item)) {
            var tdHeight = $(this).children('[data-key="' + item + '"]').outerHeight(), patchHeight = 0; // 获取td高度
            if ($main.eq(i).data('index') === 0) {
              patchHeight = 1
            }
            $(this).children('[data-key="' + item + '"]').attr('rowspan', mergeRecord[item].rowspan).css({
              'position': 'static',
              'height': tdHeight * mergeRecord[item].rowspan + patchHeight
            }).children().css({
              height: 'auto',
              'white-space': 'normal',
              'max-height': tdHeight * mergeRecord[item].rowspan + patchHeight - 10
            });
            $fixLeft.eq(i).children('[data-key="' + item + '"]').attr('rowspan', mergeRecord[item].rowspan).css({
              'position': 'static',
              'height': tdHeight * mergeRecord[item].rowspan + patchHeight
            }).children().css({
              height: 'auto',
              'white-space': 'normal',
              'max-height': tdHeight * mergeRecord[item].rowspan + patchHeight - 10
            });
            $fixRight.eq(i).children('[data-key="' + item + '"]').attr('rowspan', mergeRecord[item].rowspan).css({
              'position': 'static',
              'height': tdHeight * mergeRecord[item].rowspan + patchHeight
            }).children().css({
              height: 'auto',
              'white-space': 'normal',
              'max-height': tdHeight * mergeRecord[item].rowspan + patchHeight - 10
            });
            mergeRecord[item].rowspan = 1;
          } else {
            $(this).children('[data-key="' + item + '"]').remove();
            $fixLeft.eq(i).children('[data-key="' + item + '"]').remove();
            $fixRight.eq(i).children('[data-key="' + item + '"]').remove();
            mergeRecord[item].rowspan += 1;
          }
        }
      })

      function isMaster(index, item) {
        var mergeField = mergeRecord[item].mergeField;
        var dataLength = layui.table.cache[myTable.id].length;
        for (var i = 0; i < mergeField.length; i++) {

          if (layui.table.cache[myTable.id][dataLength - 2 - index][mergeField[i]]
            !== layui.table.cache[myTable.id][dataLength - 1 - index][mergeField[i]]) {
            return true;
          }
        }
        return false;
      }

    }
  };

  // 输出
  exports('tableMerge', mod);
});

