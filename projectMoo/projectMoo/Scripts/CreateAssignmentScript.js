

    $('#btnNewRow').click(function () {

    var rows = $('#rows').children();
    var k = 10;
    var max = 20;
    $(rows).each(function (i, obj) {
        if ($(obj).hasClass('visible')) {
           k++;
        }
    });

    if (k <= max) {
        $("#" + (k)).removeClass("hidden").addClass("visible");
    }

    });

    $('#btnNewMilestone').click(function () {

        var milestones = $('#milestones').children();
        var k = 0;
        var max = 10;

        $(milestones).each(function (i, obj) {
            if ($(obj).hasClass('visible')) {
                k++;
            }
        });
       
        //alert(k);

        if (k <= max)
        {
            $("#" + (k)).removeClass("hidden").addClass("visible");

        }
        
        /*
        $.get(url, function(response) {

          milestones.append(response);

        });
        */

        /*
        $.ajax({
            url: url,
            cache: false,
            success: function (html) {
                milestones.append(html);
            }

        });
        return false;
        */
    });

    $(document).on('click', '.deleteRow', function () {
       // $(this).parent().removeClass('visible').addClass('hidden');
    });

    $(function () {
        $("#datepicker").datepicker();
    });