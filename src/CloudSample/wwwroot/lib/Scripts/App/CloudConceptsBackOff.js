$(function () {
    $("#loadsite").click(function () {
        $("#waitCell").show();

        $.get(Url.SiteContent + '?url=' + $("#url").val())
            .success(function (result) {
                $("#waitCell").hide();
                $("#contentDiv").html(result);
                $("#contentDiv").css("border", "1px solid gray");
            })
            .fail(function () {
                alert("Unable to connect to service. \n Please try later.");
                $("#waitCell").hide();
            });
    });
});