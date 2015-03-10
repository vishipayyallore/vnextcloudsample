$(function () {
    $("#search").click(function () {
        $("#waitCell").show();

        $.get(Url.FriendSearch + '?firstName=' + $("#firstName").val() + '&lastName=' + $("#lastName").val())
            .success(function (result) {
                $("#resultTable").css("border", "1px solid gray");
                $('#resultTable').find("tr:gt(0)").remove();

                result.forEach(function (row) {
                    $('#resultTable').append('<tr><td>' + row.Firstname + '</td><td>' + row.Lastname + '</td><td>' + row.Phone + '</td><td>' + row.Email + '</td><td>' + row.Twitter + '</td></tr>');
                });

                $("#waitCell").hide();
            })
            .fail(function () {
                alert("Unable to connect to service. \n Please try later.");
                $("#waitCell").hide();
            });
    });
});