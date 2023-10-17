$('#keyword').keyup(function () {
    var searchField = $("#keyword").val();
    var expression = RegExp(searchField, "i");

    $.ajax({
        type: "GET",
        url: "Home/Search",
        success: function (response) {
            var data = JSON.parse(response);
            console.log(data);
        }

    })
})