///
function openTopWindow(url, title, width, height) {

    title = title == undefined ? '' : title;

    width = width == undefined ? 800 : width;

    height = height == undefined ? 300 : height;

    if (url != undefined) {

        var content = '<iframe name="eui-open-page" scrolling="auto" frameborder="0" src="{src}" style="width:100%;height:100%;"></iframe>'.replace('{src}',url);

        window.top.$('#openWindow').window({

            title: title,

            width: width,

            height: height,

            content: content,

            modal: true,

            animate: true,

            minimizable: false

        });

    }

}




//返回的json结果中包含<pre>标签，去掉,并返回对象
function GetData_RegDelPre(data)
{
    var reg = /<pre.+?>(.+)<\/pre>/g;
    var result = data.match(reg);
    data = RegExp.$1;
    data = eval("(" + data + ")");
    return data;
}

function UpImgs(url, fileElementId, imgfile, pimg, img) {

    $.ajaxFileUpload({
        type: "post",
        url: url,
        secureuri: false,
        fileElementId: fileElementId,
        dataType: 'text',
        async: false,
        success: function (data) {
           
           
            var reg = /<pre.+?>(.+)<\/pre>/g;
            var result = data.match(reg);
            data = RegExp.$1;
            data = eval("(" + data + ")");
            result = data;

            if (data.Success) {
                $("#" + pimg).attr("src", data.Data);
                $("#" + img).val(data.Data);
            }
            else {
                $.Message.alert(data.Message);
            }

        },
        error: function () {
            $.Message.alert("图片上传失败...");
        }
    });

}

