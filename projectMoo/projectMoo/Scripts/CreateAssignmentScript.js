
    

    $('#btnNewMilestone').click(function () {

        var milestones = $('#milestones');
        var url = 'AddMilestone';

        $.get(url, function(response) {

          milestones.append(response);

        });
    });

    $(document).on('click', '.deleteRow', function () {
        $(this).parent().remove();
    });