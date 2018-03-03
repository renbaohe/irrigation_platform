//打开隐藏的win窗口
function openWin(Sys_RoleName) {
   
    openTopWindow1('/User/SelectIndex?Sys_RoleName=' + Sys_RoleName,
        {
            title: '选择人员',
            callback: function (res) {
                if (res == 'ok') {

                }
            }
        });
}

function openWinNew(Sys_RoleName) {
  
    openTopWindow1('/User/SelectIndex?moreSelect=1&Sys_RoleName=' + Sys_RoleName,
        {
            title: '选择人员',
            callback: function (res) {
                if (res == 'ok') {

                }
            }
        });
}
//打开隐藏的win窗口
function openWinShop() {

    openTopWindow1('/Shop/SelectIndex',
        {
            title: '选择商铺',
            callback: function (res) {
                if (res == 'ok') {

                }
            }
        });
}

//打开隐藏的win窗口
function openWinShopNew(gourmet_palace_id) {
    openTopWindow1('/Shop/SelectIndex?gourmet_palace_id=' + gourmet_palace_id,
        {
            title: '选择商铺',
            callback: function (res) {
                if (res == 'ok') {

                }
            }
        });
}



function openTopWindow1(url, options) {

    var default_options = {
        title: '标题',
        width: 600,
        height: 400,
        callback: function (res) {

            console.log(res);
        }
    };
    var op = $.extend(default_options, options);

    var randomname = 'topwin_1';
    if (url != undefined) {
        var content = '<iframe name="{name}" id="iparent" scrolling="auto" frameborder="0" src="{src}" style="width:100%;height:100%;"></iframe>'.replace('{src}', url).replace('{name}', randomname);
        var $wrap = window.top.$('<div/>').css('overflow', 'hidden').attr('rname', randomname).appendTo(window.top.document.body);
        var win = $wrap.window({
            title: op.title,
            width: op.width,
            height: op.height,
            content: content,
            modal: true,
            animate: true,
            minimizable: false,
            onClose: function (e) {
                if (op.callback) {
              
                    var data = $wrap.data('dialogresult');
                    if (data != undefined) {
                        loadFrom_data(data);
                    }
                    op.callback(data);
                }
                $wrap.window('destroy');//释放资源
            }
        });
    }
}

