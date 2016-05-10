$(document).ready(function () {
    // $('.datepicker').datepicker(); //Initialise any date pickers

    var milestones = $('#milestones');
    var url = '@Url.Action("AddMilestone", "Assignments")'; // adjust to suit

    $('#addMilestone').click(function () {

        $.get(url, function(response) {

          milestones.append(response);

        });
    });
});


