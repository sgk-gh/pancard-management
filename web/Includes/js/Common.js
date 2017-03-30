$(function () {
    $("body").click(function () {
        setTimeout(function () { ClearMessage(); }, 2000);
    });
});

function ClearMessage() {
    if ($("#ctl00_body_divMessage").html() == "") return;
    $("#ctl00_body_divMessage").html("");
}

function ClearValidation() {
    jsvalidator.clear();
}