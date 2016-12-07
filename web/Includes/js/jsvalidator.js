if (typeof jsvalidator == "undefined") {
    var jsvalidator = {
        validate: function (parameters) {
            var elements = [];
            var validationResult = true;
            for (var argI = 0; argI < parameters.length; argI++) {
                elements[argI] = jQuery.extend({
                    id: '',
                    type: '',
                    isRequired: false,
                    reqErrorMessage: '',
                    compareWith: '',
                    compErrorMessage:''
                }, parameters[argI]);
                if (elements[argI].isRequired) {
                    if (elements[argI].type == "text") {
                        if ($("#" + elements[argI].id).val().trim().length == 0) {
                            validationResult = false;
                            $("#" + elements[argI].id).parent().addClass("has-error");
                            $("#" + elements[argI].id).next("span").remove()
                            $("#" + elements[argI].id).after("<span class='jsv-error' style='color: #dd4b39;display: block;line-height: 17px;margin: 0.5em 0 0;'>" + elements[argI].reqErrorMessage + "</span>");
                            $("#" + elements[argI].id).keypress(function () {
                                if ($(this).parent().hasClass("has-error"))
                                    $(this).parent().removeClass("has-error");
                                if ($(this).next("span").length) {
                                    $(this).next("span").remove();
                                }
                            });
                        }
                    }
                    else if (elements[argI].type == "file") {
                        if ($("#" + elements[argI].id).val().length == 0) {
                            validationResult = false;
                            $("#" + elements[argI].id).parent().addClass("has-error");
                            //$("#" + elements[argI].id).attr("style", "border-color: #a94442;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.075) inset;");
                            $("#" + elements[argI].id).next("span").remove()
                            $("#" + elements[argI].id).after("<span class='jsv-error' style='color: #dd4b39;display: block;line-height: 17px;margin: 0.5em 0 0;'>" + elements[argI].reqErrorMessage + "</span>");
                            $("#" + elements[argI].id).keypress(function () {
                                if ($(this).parent().hasClass("has-error")) {
                                    $(this).parent().removeClass("has-error");
                                    //$(this).removeAttr("style");
                                }
                                if ($(this).next("span").length) {
                                    $(this).next("span").remove();
                                }
                            });
                        }
                    }
                }
                if (elements[argI].compareWith != '') {
                    if ($("#" + elements[argI].id).val() != $("#" + elements[argI].compareWith).val()) {
                        validationResult = false;
                        $("#" + elements[argI].id).parent().addClass("has-error");
                        $("#" + elements[argI].id).next("span").remove()
                        $("#" + elements[argI].id).after("<span class='jsv-error' style='color: #dd4b39;display: block;line-height: 17px;margin: 0.5em 0 0;'>" + elements[argI].compErrorMessage + "</span>");
                        $("#" + elements[argI].id).keypress(function () {
                            if ($(this).parent().hasClass("has-error")) {
                                $(this).parent().removeClass("has-error");                                
                            }
                            if ($(this).next("span").length) {
                                $(this).next("span").remove();
                            }
                        });
                    }
                    
                }
            }
            return validationResult;
        },
        clear: function () {
            $(".jsv-error").parent().removeClass("has-error");
            $(".jsv-error").remove();
        }
    };
}