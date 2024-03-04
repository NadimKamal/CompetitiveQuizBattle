//<%--if the notifications are 0 the div is not shown--%>

function totalfunc() {
    $(document).ready(function() {
        var ids = new Array();
        
        $('.notifications a span span').each(function() {
            if ($(this).text() == 0) {
                var id = $(this).parent().parent().parent().attr("id");
                ids.push(id);
            }
            else {
                
            }
        });
        $.each(ids, function(key, value) {
            $("#" + value).hide();
        });
    });
    totalcount();
    $('body').show();
}
 //<%--opens the dialogue box--%>
function showfunc(hrefs, thisid) {
    //$('#' + thisid + ' a').text($('#' + thisid + ' a').text()-1);
    $('#' + thisid + ' a').text("");
    $('#dialogdiv').css("visibility","visible");
    //$('#dialogdiv').load(hrefs).dialog({ modal: true, title: 'NOTIFICATION' });
     $('#dialogdiv iframe').attr("src",hrefs);
    totalcount();
}

function totalcount() {
    var totalnot = 0;
    $('.notifications a span span').each(function() {
        if ($(this).text() > 0) {
            totalnot = parseInt($(this).text()) + totalnot;
        }
        
    });
    $('#pendings').text(totalnot);
}
