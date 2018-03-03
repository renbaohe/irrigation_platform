///初始化combox 
function InitCombox(id, dataUrl, valueField, textField, isMultiple,firstSelected) {
    valueField = valueField == undefined ? "id" : valueField;
    textField = textField == undefined ? "text" : textField;

    $('#' + id).combobox({
        url: dataUrl,
        valueField: valueField,
        textField: textField,
        multiple: isMultiple,
        panelHeight: 'auto'
    });
    if (firstSelected) {
        $('#' + id).combobox({
            onLoadSuccess: function () {
                var data = $(this).combobox('getData');
                $(this).combobox("setValue", eval("data[0]."+valueField));
            }
        });
    }
}
function InitComboxData(id, data, valueField, textField, firstSelected) {
    valueField = valueField == undefined ? "id" : valueField;
    textField = textField == undefined ? "text" : textField;

    $('#' + id).combobox({
        data: data,
        valueField: valueField,
        textField: textField,
        panelHeight: 200
    });
    if (firstSelected) {
        $('#' + id).combobox({
            onLoadSuccess: function () {
                var data = $(this).combobox('getData');
                $(this).combobox("setValue", eval("data[0]." + valueField));
            }
        });
    }
}
///初始化常规的 combox 
function InitCommonCombox(id, dataUrl) {
    InitCombox(id, dataUrl, undefined, undefined, false);
}
//多选下拉框
function InitCommonMultipleCombox(id, dataUrl) {
    InitCombox(id, dataUrl, undefined, undefined, true);
}