
$(document).ready(function() {
    load();
    numVerify();
    noNumVerify();
    dataPresence();
    datePick();
});
function EndRequestHandler(sender, args) {
    if (args.get_error() == undefined) {
        numVerify();
        noNumVerify();
        dataPresence();
        datePick();
    }
    else {
        alert("Error occured");
    }
}
function load() {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
}
function numVerify() {
    $("input[data-type=number]").each(function() {
        $(this).keyup(function() {
            if (isNaN($(this).val())) {
                alert($(this).val() + " is Not Numeric and not allowed here");
                $(this).val("");
            }
            else {
                
            }
        });
    });
}
function noNumVerify() {
    $("input[data-type=nonumber]").each(function() {
        $(this).keyup(function() {
            if (isNaN($(this).val())) {
                
            }
            else {
                alert($(this).val() + " is Numeric and not allowed here");
                $(this).val("");
            }
        });
    });
}
function dataPresence() {
    $("input[type=submit]").click(function() {
        var isEmpty = false;
        $("input[data-presence=must]").each(function() {
            if (!($(this).val())) {
                //alert("Field Cannot Remain empty");
                $(this).attr("placeholder", "Can not stay empty");
                $(this).css("border", "2px dotted red");
                $(this).focus();
                isEmpty = true;
            }
            else {

            }
        });
        if (isEmpty) {
            event.preventDefault();
        }
    });

}
function datePick() {
    $("input[data-type=date]").each(function() {
        try {
            if ($(this).attr("data-formate") == null) {
                $(this).datepicker({ dateFormat: 'dd/M/yy' });
                $(this).attr("readonly", "readonly");
            }
            else {
                $(this).datepicker({ dateFormat: $(this).attr("data-formate") });
            }
        }
        catch (err) {
            try {
                $(this).datepicker({ dateFormat: 'dd/M/yy' });
            }
            catch (err1) {
                alert("jquery ui is not present in the page or make sure they are in the correct order");
            }
        }
    });
}