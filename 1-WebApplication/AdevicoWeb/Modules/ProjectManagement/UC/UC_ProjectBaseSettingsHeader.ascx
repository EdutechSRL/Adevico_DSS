<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectBaseSettingsHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ProjectBaseSettingsHeader" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>

<!-- START BASE PROJECT SETTINGS HEADER-->
<script type="text/javascript">
    var RDTstartDate;
    var RDTendDate;
    var RDTdeadlineDate;

    function validateStartEndDate(sender, args) {
        if (RDTstartDate != null && RDTendDate != null) {
            var fDate = new Date(RDTstartDate.get_selectedDate());
            if (RDTendDate.get_selectedDate() != null) {
                var lDate = new Date(RDTendDate.get_selectedDate());
                args.IsValid = !isInvalidRange(fDate, lDate);
                if (!args.IsValid) {
                    alert("<%=GetTranslateLastDateError() %>");
//                    lDate.setDate(fDate.getDate() + 1);
//                    RDTendDate.set_selectedDate(lDate);
                }
            }
        }
    }
    function validateDeadline(sender, args) {
        if (RDTstartDate != null && RDTdeadlineDate != null) {
            var fDate = new Date(RDTstartDate.get_selectedDate());
            if (RDTendDate!=null){
                if (RDTendDate.get_selectedDate() != null) {
                    var lDate = new Date(RDTdeadlineDate.get_selectedDate());
                    args.IsValid = !isInvalidRange(fDate, lDate);
                    if (!args.IsValid) {
                        alert("<%=GetTranslateDeadlineDateError() %>");
                    }
                }
            }
        }
    }
    function isInvalidRange(firstDate, secondDate) {
        return ((secondDate - firstDate) < 0);
    }

    function onLoadStartDate(sender, args) {
        RDTstartDate = sender;
    }
    function onLoadEndDate(sender, args) {
        RDTendDate = sender;
    }
    function onLoadDeadline(sender, args) {
        RDTdeadlineDate = sender;
    }
    function onPopupOpening(sender, args) {
        sender.set_minDate(new Date(RDTstartDate.get_selectedDate()));
    }
    function onStartDateSelected(sender, args) {
        if (RDTstartDate != null && RDTendDate != null) {
            var fDate = new Date(RDTstartDate.get_selectedDate());
            if (RDTstartDate.get_selectedDate()!=null && RDTendDate.get_selectedDate() != null) {
                var lDate = new Date(RDTendDate.get_selectedDate());
                if (isInvalidRange(fDate, lDate)) {
                    RDTendDate.set_selectedDate(fDate);
                }
            }
        }
        if (RDTstartDate != null && RDTdeadlineDate != null) {
            var fDate = new Date(RDTstartDate.get_selectedDate());
            if (RDTstartDate.get_selectedDate() != null && RDTdeadlineDate.get_selectedDate() != null) {
                var lDate = new Date(RDTdeadlineDate.get_selectedDate());
                if (isInvalidRange(fDate, lDate)) {
                    RDTdeadlineDate.set_selectedDate(fDate);
                }
            }
        }
    }
</script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".view-modal.view-users-community").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 890,
                height: 600,
                minHeight: 300,
                minWidth: 700,
                title: '<%=SelectOwnerFromCommunityDialogTitleTranslation() %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });

            $(".view-modal.view-users-project").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 600,
                height: 250,
                minHeight: 100,
                minWidth: 500,
                title: '<%=SelectOwnerFromProjectResourceslDialogTitleTranslation() %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });
    </script>
    <CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
<!-- END BASE PROJECT SETTINGS HEADER-->