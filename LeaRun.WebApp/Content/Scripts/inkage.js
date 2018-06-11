//初始化绑定下拉框
function BindCombobox() {
    //所在省
    $("#ProvinceId").append("<option value=''>请选择</option>");
    $("#CityId").append("<option value=''>请选择</option>");
    $("#CountyId").append("<option value=''>请选择</option>");
    AjaxJson("/Utility/GetProvinceCityListJson", { ParentId: 0 }, function (DataJson) {
        $.each(DataJson, function (i) {
            $("#ProvinceId").append($("<option></option>").val(DataJson[i].Code).html(DataJson[i].FullName));
        });
    })
    //所在市
    $("#ProvinceId").change(function () {
        BindCityId();
        BindCountyId();
    });
    //所在县区
    $("#CityId").change(function () {
        BindCountyId();
    });
}
//所在市下拉框
function BindCityId() {
    $("#CityId").html("");
    $("#CityId").append("<option value=''>请选择</option>");
    AjaxJson("/Utility/GetProvinceCityListJson", { ParentId: $("#ProvinceId").val() }, function (DataJson) {
        $.each(DataJson, function (i) {
            $("#CityId").append($("<option></option>").val(DataJson[i].Code).html(DataJson[i].FullName));
        });
    })
}
//所在县区
function BindCountyId() {
    $("#CountyId").html("");
    $("#CountyId").append("<option value=''>请选择</option>");
    AjaxJson("/Utility/GetProvinceCityListJson", { ParentId: $("#CityId").val() }, function (DataJson) {
        $.each(DataJson, function (i) {
            $("#CountyId").append($("<option></option>").val(DataJson[i].Code).html(DataJson[i].FullName));
        });
    })
}