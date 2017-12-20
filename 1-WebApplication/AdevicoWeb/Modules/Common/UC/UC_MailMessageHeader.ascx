<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailMessageHeader.ascx.vb" Inherits="Comunita_OnLine.UC_MailMessageHeader" %>

<script type="text/javascript" src="<%#ResolveUrl("~/Jscript/Modules/Plugins/tagit/tag-it.min.js")%>"></script>
<link href="<%#ResolveUrl("~/Graphics/Plugins/tagit/jquery.tagit.css")%>" rel="Stylesheet" />
<script type="text/javascript">
    $(function () {

        $(".mailaddress .icon.paste").click(function () {
            var text = prompt("Paste");
            var texts = text.split(";");

            var $input = $(this).parents(".mailaddress").first().find("input.tokeninputmail");

            var length = texts.length,
                    element = null;
            for (var i = 0; i < length; i++) {
                element = texts[i];
                $input.tagit("createTag", element);
                // Do something with element i.
            }

            return false;
        });

        $(".fieldinfo.showiferror .hideerrors").click(function () {
            $(this).parents(".fieldrow.error").first().removeClass("error");

            return false;
        });

        var tgit = $("input.tokeninputmail").tagit({
            removeConfirmation: true,
            beforeTagAdded: function (event, ui) {
                var st = "";
                st = ui.tagLabel;

                var errors = "";

                var reg = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

                if (st.indexOf(";") != -1) {
                    //list of addresses
                    var texts = st.split(";");

                    var $input = tgit; // $(this).parents(".mailaddress").first().find("input.tokeninputmail");

                    var length = texts.length,
                        element = null;
                    for (var i = 0; i < length; i++) {
                        element = texts[i];
                        //check regular expression before
                        if (element != "") {
                            var m = element.match(reg);
                            if (m != null) {
                                $input.tagit("createTag", element);
                            } else {
                                errors += "<span class='erroritem mail'>" + element + "</span>";
                            }
                        }

                    }
                    if (errors != "") {
                        if (tgit.parents(".fieldrow").size() > 0) {
                            tgit.parents(".fieldrow").first().addClass("error").find(".fieldinfo").children(".details").html(errors);
                        }
                    }
                    return false;
                } else {

                    var m = st.match(reg);
                    return m != null;

                    //return reg.test(st);
                }
            }
        });


        $(".btnswitchgroup.js .btnswitch.disabled").click(function () {
            return false;
        });

        $(".btnswitchgroup.js .btnswitch:not(.disabled)").click(function () {
            var $parent = $(this).parents(".btnswitchgroup").first();
            $parent.find(".active").removeClass("active");
            $(this).addClass("active");

            CheckOptions();
            return false;
        });

        CheckOptions();

        function CheckOptions() {
            if ($(".btnswitchgroup.showaddresses .btnswitch.first.active").size() > 0) {
                $(".fieldobject .fieldrow.mailaddresses").show();
            } else {
                $(".fieldobject .fieldrow.mailaddresses").hide();
            }
            if ($(".btnswitchgroup.showattachments .btnswitch.first.active").size() > 0) {
                $(".fieldobject .fieldrow.mailattachments").show();
            } else {
                $(".fieldobject .fieldrow.mailattachments").hide();
            }
            if ($(".btnswitchgroup.showoptions .btnswitch.first.active").size() > 0) {
                $(".fieldobject .fieldrow.mailoptions").show();
            } else {
                $(".fieldobject .fieldrow.mailoptions").hide();
            }
        }

    });
    </script>