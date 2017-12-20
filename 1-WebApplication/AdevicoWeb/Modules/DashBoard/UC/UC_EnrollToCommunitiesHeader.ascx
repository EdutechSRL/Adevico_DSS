<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EnrollToCommunitiesHeader.ascx.vb" Inherits="Comunita_OnLine.UC_EnrollToCommunitiesHeader" %>
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EnrollDialogHeader" Src="~/Modules/DashBoard/UC/UC_EnrollDialogHeader.ascx" %>

 <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>

<script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>    
<CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVDSHBOARD" FilterModuleScope="subscribe" runat="server"></CTRL:FiltersHeader>
    <script type="text/javascript">
        $(function(){

            $(".tablewrapper.hasbulk table.table .select input[type='checkbox']").change(function(){
                var $tablew = $(this).parents(".tablewrapper.hasbulk");
                var $table = $(this).parents("table.table").first();
                var $checks = $table.find(".select input[type='checkbox']").filter(":not([disabled])");
                var n_checks = $checks.size();
                var n_checks_checked =$checks.filter(":checked").size();

                if($(this).parents(".select").first().is("th.select")){
                    $checks.prop("checked",$(this).is(":checked"));
                }else{
                    if(n_checks_checked >= n_checks-1){

                        $table.find("th.select input[type='checkbox']").prop("checked",true);
                    }else{
                        $table.find("th.select input[type='checkbox']").prop("checked",false);
                    }
                }

                n_checks_checked =$checks.filter(":checked").size();

                if (n_checks_checked>0){
                    $tablew.addClass("bulkon").removeClass("bulkoff");
                }else
                {
                    if (!$tablew.is(".keepopen")) {
                        $tablew.removeClass("bulkon").addClass("bulkoff");
                    }
                }
            });
        })
    </script>

 <CTRL:EnrollDialogHeader id="CTRLenrollDialogHeader" runat="server"></CTRL:EnrollDialogHeader>