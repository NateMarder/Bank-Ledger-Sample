
document.addEventListener("DOMContentLoaded", function() {

    var securityToken = $('[name=__RequestVerificationToken]').val();
    
    $(document).ajaxSend(function (event, request, opt) {
        if (opt.hasContent && securityToken) {   // handle all verbs (put,post,get,etc..)
            var token = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
            opt.data = opt.data ? [opt.data, token].join("&") : token;
           
            if (opt.contentType !== false || event.contentType) {  // ensure we have a content-type header
                request.setRequestHeader( "Content-Type", opt.contentType);
            }
        }
    });

});