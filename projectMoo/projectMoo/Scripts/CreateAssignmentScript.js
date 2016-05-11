
    

    $('#btnNewMilestone').click(function () {

        var milestones = $('#milestones');
        var url = 'AddMilestone';

        /*
        $.get(url, function(response) {

          milestones.append(response);

        });
        */
        $.ajax({
            url:url,
            cache: false,
            success: function (html) {
                milestones.append(html);
            }

        });
        return false;

    });

    $(document).on('click', '.deleteRow', function () {
        $(this).parent().remove();
    });