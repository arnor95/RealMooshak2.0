
    

    $('#btnNewMilestone').click(function () {

        var milestones = $('#milestones').children();
        var k = 0;
        $(milestones).each(function (i, obj) {
            if ($(obj).hasClass('visible')) {
                k++;
            }
        });
       
        //alert(k);

        if (k <= 10)
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