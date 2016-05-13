$('.assignment-li-a').click(function (event) {

    event.preventDefault();

    var type = $(this).attr('data-type');

    $("#" + type).removeClass("hidden").siblings("div").addClass("hidden");

    $(".Empty").addClass("hidden");
});

