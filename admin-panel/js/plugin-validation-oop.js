$(document).ready(function() {
    //    numVerify();
    //    noNumVerify();
    //    dataPresence();
    //    datePick();
    var mainObj = new mainClass();
    mainObj.start();
});
function mainClass() {
    this.start = function() {
        var valObj = new validationClass();
        valObj.numverify();
        valObj.noNumVerify();
        valObj.datepick();
        valObj.dataPresence();
    }
}
function validationClass() {
    var thisClass = this;
    this.numverify = function() {
        $("input[data-type=number]").each(function() {
            $(this).keyup(function() {
                if (isNaN($(this).val())) {
                    // alert($(this).val() + " is Not Numeric and not allowed here");
                    //                    $(this).attr("placeholder", "is not Numeric and not allowed here");
                    //                    $(this).css("border", "2px dotted red");
                    //                    $(this).focus();
                    //                    $(this).val("");
                    //                    $(this).val("");
                    thisClass.ErrorWork($(this), "is Not Numeric and not allowed here");
                }
                else {
                    $(this).css("border", "");
                }
            });
        });
    };
    this.noNumVerify = function() {
        $("input[data-type=nonumber]").each(function() {
            $(this).keyup(function() {
                if (isNaN($(this).val())) {
                    // $(this).css("border", "");
                    thisClass.undoError($(this));
                }
                else {
                    //alert($(this).val() + " is Numeric and not allowed here");
                    //                    $(this).attr("placeholder", "is Numeric and not allowed here");
                    //                    $(this).css("border", "2px dotted red");
                    //                    $(this).focus();
                    //                    $(this).val("");
                    thisClass.ErrorWork($(this), "is Numeric and not allowed here");
                }
            });
        });
    };
    this.dataPresence = function () {
        $("input[type=submit]").click(function(event) {
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

    };
    this.datepick = function() {
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
    };
    this.ErrorWork = function(that, message) {
        that.attr("placeholder", message);
        that.css("border", "2px dotted red");
        that.focus();
        that.val("");
    };
    this.undoError = function(that) {
        that.css("border", "");
    }
}
//function numVerify() {
//    $("input[data-type=number]").each(function() {
//        $(this).keyup(function() {
//            if (isNaN($(this).val())) {
//                alert($(this).val() + " is Not Numeric and not allowed here");
//                $(this).val("");
//            }
//            else {
//                
//            }
//        });
//    });
//}
//function noNumVerify() {
//    $("input[data-type=nonumber]").each(function() {
//        $(this).keyup(function() {
//            if (isNaN($(this).val())) {
//                
//            }
//            else {
//                alert($(this).val() + " is Numeric and not allowed here");
//                $(this).val("");
//            }
//        });
//    });
//}
//function dataPresence() {
//    $("input[type=submit]").click(function() {
//        var isEmpty = false;
//        $("input[data-presence=must]").each(function() {
//            if (!($(this).val())) {
//                //alert("Field Cannot Remain empty");
//                $(this).attr("placeholder", "Can not stay empty");
//                $(this).css("border", "2px dotted red");
//                $(this).focus();
//                isEmpty = true;
//            }
//            else {

//            }
//        });
//        if (isEmpty) {
//            event.preventDefault();
//        }
//    });

//}
//function datePick() {
//    $("input[data-type=date]").each(function() {
//        try {
//            if ($(this).attr("data-formate") == null) {
//                $(this).datepicker({ dateFormat: 'dd/M/yy' });
//                $(this).attr("readonly", "readonly");
//            }
//            else {
//                $(this).datepicker({ dateFormat: $(this).attr("data-formate") });
//            }
//        }
//        catch (err) {
//            try {
//                $(this).datepicker({ dateFormat: 'dd/M/yy' });
//            }
//            catch (err1) {
//                alert("jquery ui is not present in the page or make sure they are in the correct order");
//            }
//        }
//    });
//}