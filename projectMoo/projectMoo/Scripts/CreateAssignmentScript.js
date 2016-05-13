
    $('#btnNewMilestone').click(function () {

        var milestones = $('#milestones').children();
        var k = 0;
        var max = 10;

        $(milestones).each(function (i, obj) {
            if ($(obj).hasClass('visible')) {
                k++;
            }
        });
       
        if (k <= max)
        {
            $("#" + (k)).removeClass("hidden").addClass("visible");

        }
       
    });

    $(function () {
        $("#datepicker").datepicker({
            dateFormat: 'dd-mm-yyyy'
        });
    });