function pageaccess() {
    getaccess();
}
function getaccess() {
    var page=getpagename();
    $.ajax({
                type: "POST",
                url: "../pageaccessEngin.aspx/pageaccess",
                data: "{pagename:'" + page + "'}",
                contentType: "application/json; charset=utf-8",
                success: function(msg) {
                    if (msg.d != null) {
                        var messages = msg.d;
                        var ms = $.parseJSON(messages);
                        $.each(ms, function(index, message) {
                            var Cedit = message.SYS_ACCP_EDIT;
                            var Cdelete = message.SYS_ACCP_DELETE;
                            var Cprint = message.SYS_ACCP_PRINT;
                            if(Cedit=="N")
                            {
                                editaccess();
                            }
                            if(Cdelete=="N")
                            {
                                deleteaccess();
                            }
                            if(Cprint=="N")
                            {
                                 printaccess();
                            }
                        });
                    }
                    else {
                    alert("nothing");
                    }
                },
                failure: function(msg) {
                    alert(msg);
                }
            });
}
function getpagename() {
    var path = window.location.pathname;
    var filename = path.match(/.*\/([^/]+)\.([^?]+)/i)[1];
    return filename+".aspx";
}
function readCookie(name) {
    var cookiename = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(cookiename) == 0) return c.substring(cookiename.length, c.length);
    }
    return null;
}
function editaccess() {
    $("a:contains('Edit')").css("visibility", "hidden");
}
function printaccess() {
    $("a:contains('Print')").css("visibility", "hidden");
    $("input:contains('Print')").css("visibility", "hidden");
    $("button:contains('Print')").css("visibility", "hidden");
}
function deleteaccess() {
    $("a:contains('Delete')").css("visibility", "hidden");
}