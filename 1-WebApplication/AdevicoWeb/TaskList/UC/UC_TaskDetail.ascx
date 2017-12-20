<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TaskDetail.ascx.vb"    
    Inherits="Comunita_OnLine.UC_TaskDetail" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<style type="text/css">
    .InternalItem
    {
        padding: 0px 5px 0px 5px;
        float: left;
        text-align: left;
        width: 170px;
        height: auto;
        clear:right
        
        
    }
    .ExternalItem
    {
        height: 24px;
        padding: 5px 0px 2px 0px;
        width: 900px;
        clear:right
          
       
    }
    .DescriptionItem
    {
        padding: :2px 0px 2px 5px;
        clear:both;
    }
</style>
<asp:MultiView ID="MLVdetail" runat="server">
    <asp:View ID="VIWdetailReadOnly" runat="server">
        <div style="display: compact">
            <div id="DVtitle" style="width: 900px; height:auto; text-align: left;" class="RigaTitolo" align="center">                
                    <asp:Image runat="server" ID="IMpriority"></asp:Image>
                    <asp:Image runat="server" ID="IMstatus"></asp:Image>               
                    <asp:Label ID="LBwbs" runat="server"></asp:Label>
                    <asp:Label ID="LBtaskName" runat="server"></asp:Label>              
            </div>
            <div align="right" style="text-align: right; clear: right; height: 24px; padding: 5px">
                <asp:HyperLink ID="HYPgantt" runat="server" Text="gantt**" CssClass="Link_Menu" />
                <asp:HyperLink ID="HYPgoToProjectMap" runat="server" Text="GoToProjectMap**" CssClass="Link_Menu" />
                <asp:HyperLink ID="HYPaddSubTask" runat="server" Text="Add Sub Task**" CssClass="Link_Menu" />
                <asp:HyperLink ID="HYPreturnToTaskList" runat="server" Text="Return To TaskList**"
                    CssClass="Link_Menu" />
            </div>
            <div id="DIVcommunityName" class="ExternalItem" style="clear:both">
                <div class="InternalItem">
                    <asp:Label ID="LBcommunityTitle" runat="server" CssClass="Titolo_campoSmall">Community:**</asp:Label>
                </div>
                <div style="text-align: left;">
                    <asp:Label ID="LBcommunityName" runat="server" CssClass="dettagli_CampoSmall">nome**</asp:Label>
                </div>
            </div>
            <div id="DIVprojectName" class="ExternalItem">
                <div class="InternalItem">
                    <asp:Label ID="LBprojectTitle" runat="server" CssClass="Titolo_campoSmall">Project:**</asp:Label>
                </div>
                <div style="width : 80%;  text-align: left; float:right; " >
                    <asp:Label ID="LBprojectName" runat="server" CssClass="dettagli_CampoSmall">nome**</asp:Label>
                </div>
            </div>
            <div id="DIVdescription" style="padding: 5px 0px 2px 0px; width: 900px;  min-height: 24px; clear: both ">
                <div class="InternalItem">
                    <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
                </div>
                <div style="text-align: left; width: 80%; float:right;  ">
                    <asp:Label ID="LBdescriptionText" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                    <asp:Literal ID="LTspace" runat="server"></asp:Literal>
                </div>
            </div>
            <div id="DIVcategory" class="ExternalItem" style="width: 900px; ">
                <div class="InternalItem">
                    <asp:Label ID="LBcategoryTitle" runat="server" CssClass="Titolo_campoSmall">Category:**</asp:Label>
                </div>
                <div style="text-align: left;">
                    <asp:Label ID="LBcategoryName" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                </div>
            </div>
            <div id="DIVdate" class="ExternalItem">
                <div class="InternalItem">
                    <asp:Label ID="LBstartDateTitle" runat="server" CssClass="Titolo_campoSmall">Start Date:**</asp:Label>
                </div>
                <div style="text-align: left; float: left; width: auto">
                    <asp:Label ID="LBstartDate" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                </div>
                <div style="float: left; width: 50px;">
                    &nbsp;&nbsp;&nbsp;&nbsp;</div>
                <div class="InternalItem">
                    <asp:Label ID="LBendDateTitle" runat="server" CssClass="Titolo_campoSmall">End Date:**</asp:Label>
                </div>
                <div style="text-align: left; float: left; width: auto">
                    <asp:Label ID="LBendDate" runat="server" CssClass="dettagli_CampoSmall">11/1/2010</asp:Label>
                </div>
            </div>
            <div id="DIVdeadline" class="ExternalItem">
                <div class="InternalItem">
                    <asp:Label ID="LBdeadlineTitle" runat="server" CssClass="Titolo_campoSmall">Deadline:**</asp:Label>
                </div>
                <div style="text-align: left;">
                    <asp:Label ID="LBdeadline" runat="server"></asp:Label>
                </div>
            </div>
            <div id="DIVtaskCompleteness" class="ExternalItem" style="clear:none" runat="server">
                <div class="InternalItem" style="float: left">
                    <asp:Label ID="LBtaskCompletenessTitle" runat="server" CssClass="Titolo_campoSmall">% Task Completeness:**</asp:Label>
                </div>
                <div style="float: left; width: 60px; text-align: left; padding: 0px 2px 0px 0px">
                    <asp:Label ID="LBtaskCompleteness" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                    %
                </div>
                <div style="float: left; width: 100px; text-align: left; border: 1px solid black; height:15px " >
                    <asp:Image ID="IMtaskCompleteness" runat="server"  Height="15px" />
                </div>
            </div>
            <div id="DIVpersonalCompleteness" class="ExternalItem" runat="server">
                <div class="InternalItem">
                    <asp:Label ID="LBpersonalCompletnessTitle" runat="server" CssClass="Titolo_campoSmall">% Personal Completeness:**</asp:Label>
                </div>
                <div>
                    <div style="float: left; width: 50px; text-align: left; padding: 0px 3px 0px 0px">
                        <asp:TextBox ID="TXTpersonalCompleteness" runat="server" Width="30px" MaxLength="3"
                            TextMode="SingleLine"></asp:TextBox>
                        %
                    </div>
                    <div style="float: left; width: 10px">
                        <asp:RangeValidator ID="RNVpersonalCompleteness" runat="server" ErrorMessage="Il valore deve essere compreso tra 0 e 100"
                            Text="*" ControlToValidate="TXTpersonalCompleteness" MinimumValue="0" MaximumValue="100"
                            Type="Integer"></asp:RangeValidator>
                        <asp:CompareValidator ID="CMPpersonalCompleteness" runat="server" ErrorMessage="Il valore deve essere un intero"
                            Text="*" ControlToValidate="TXTpersonalCompleteness" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                    </div>
                    <div style="padding: 0px 6px 0px 0px; float: left; width: 100px; text-align: left;">
                        <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                            border: 1px solid black">
                            <asp:Image ID="IMpersonalCompleteness" runat="server" BackColor="Green" Height="15px" />
                        </div>
                    </div>
                </div>
                <div style="float: left">
                    <asp:Button ID="BTNupdatePersonalCompleteness" runat="server" Text="**Update" CssClass="Link_Menu" />
                </div>
            </div>
            <div id="DIVnote" runat="server" class="ExternalItem" style="height: auto; display: block">
                <div class="InternalItem">
                    <asp:Label ID="LBnote" runat="server" CssClass="Titolo_campoSmall">Note:**</asp:Label>
                </div>
                <div style="text-align: left; width: 100%">
                    <asp:Label ID="LBnoteText" runat="server" CssClass="dettagli_CampoSmall">nome**</asp:Label>
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdetailEditable" runat="server">
        <div id="DIVtitleEdit" style="width: 900px; text-align: left;" class="RigaTitolo"
            align="center" runat="server">
            <div style="float: left; width: auto;">
                <asp:Label ID="LBtitleEdit" runat="server">Update Task Detail**</asp:Label>
            </div>
        </div>
        <div runat="server" id="DIVbuttonEditable" align="right" style="text-align: right;
            clear: right; height: 24px; padding: 5px">
            <asp:Button ID="BTNsaveTask" runat="server" Text="**Update Task" CssClass="Link_Menu" />
            <asp:HyperLink ID="HYPganttEdit" runat="server" Text="gantt**" CssClass="Link_Menu" />
            <asp:HyperLink ID="HYPgoToProjectMapEdit" runat="server" Text="GoToProjectMap**"
                CssClass="Link_Menu" />
            <asp:HyperLink ID="HYPaddSubTaskEdit" runat="server" Text="AddSubTask*f*" CssClass="Link_Menu" />
            <asp:HyperLink ID="HYPreturnToTaskListEdit" runat="server" Text="Return To TaskList"
                CssClass="Link_Menu" />
        </div>
        <div id="DIVnameEdit" class="ExternalItem">
            <div class="InternalItem">
                <asp:Label ID="LBnameTitleEdit" runat="server" CssClass="Titolo_campoSmall">Name:**</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:Label ID="LBwbsEdit" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                <asp:TextBox ID="TXTnameEdit" runat="server" Width="70%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVname" runat="server" ControlToValidate="TXTnameEdit"
                    Display="Static" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div id="DIVcommunityEdit" class="ExternalItem" >
            <div class="InternalItem">
                <asp:Label ID="LBcommunityTitleEdit" runat="server" CssClass="Titolo_campoSmall">Community:**</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:Label ID="LBcommunityEdit" runat="server" CssClass="dettagli_CampoSmall">nome</asp:Label>
            </div>
        </div>
        <div id="DIVprojectEdit" runat="server" class="ExternalItem" >
            <div class="InternalItem">
                <asp:Label ID="LBprojectTitleEdit" runat="server" CssClass="Titolo_campoSmall">Project:**</asp:Label>
            </div>
            <div style="width:80%; text-align: left; float:right;">
                <asp:Label ID="LBprojectEdit" runat="server" CssClass="dettagli_CampoSmall">nome</asp:Label>
            </div>
        </div>
        <div id="DIVdescriptionEdit" class="DescriptionItem" style="display:block ; padding-top: 7px; ">
            <div class="InternalItem">
                <asp:Label ID="LBdescriptionTitleEdit" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
            </div>
            <div style="text-align: left; height: 30%; width:80%; float:right; ">
                <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                    LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="230px" />
            </div>
        </div>
        <div id="DIVcategoryEdit" class="ExternalItem">
            <div class="InternalItem">
                <asp:Label ID="LBcategoryEdit" runat="server" CssClass="Titolo_campoSmall">Category:**</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:DropDownList ID="DDLcategory" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div id="DIVdateEdit" class="ExternalItem">
            <div class="InternalItem">
                <asp:Label ID="LBstartDateTitleEdit" runat="server" CssClass="Titolo_campoSmall">Start Date:**</asp:Label>
            </div>
            <div style="text-align: left; float: left; width: auto">
                <asp:Label ID="LBstartDateEdit" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                 <telerik:RadDatePicker ID="RDPstartDateEdit" runat="server">
                </telerik:RadDatePicker>
            </div>
            <div style="float: left; width: 50px;">
                &nbsp;&nbsp;&nbsp;&nbsp;</div>
            <div class="InternalItem">
                <asp:Label ID="LBendDateTitleEdit" runat="server" CssClass="Titolo_campoSmall">End Date:**</asp:Label>
            </div>
            <div style="text-align: left; float: left; width: auto">
                <asp:Label ID="LBendDateEdit" runat="server" CssClass="dettagli_CampoSmall">11/1/2010</asp:Label>
                <telerik:RadDatePicker ID="RDPendDateEdit" runat="server">
                </telerik:RadDatePicker>
                <asp:CompareValidator ID="CVdate" runat="server" ControlToValidate="RDPstartDateEdit"
                    ControlToCompare="RDPendDateEdit" Type="Date" Operator="LessThanEqual" ErrorMessage="*"
                    ForeColor="Red"></asp:CompareValidator>
            </div>
        </div>
        <div id="DIVdeadlineEditRead" class="ExternalItem">
            <div class="InternalItem">
                <asp:Label ID="LBdeadlineTitleEdit" runat="server" CssClass="Titolo_campoSmall">Deadline:**</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:Label ID="LBdeadlineEdit" runat="server" CssClass="erroreSmall"></asp:Label>
                <telerik:RadDatePicker ID="RDPdeadlineEdit" runat="server">
                </telerik:RadDatePicker>
                <asp:CompareValidator ID="CVdate2" runat="server" ControlToValidate="RDPdeadlineEdit"
                    ControlToCompare="RDPendDateEdit" Type="Date" Operator="GreaterThanEqual" ErrorMessage="*"
                    ForeColor="Red"></asp:CompareValidator>
            </div>
        </div>
        <div id="DIVtaskCompletenessEdit" class="ExternalItem" runat="server">
            <div class="InternalItem" style="float: left">
                <asp:Label ID="LBtaskCompletenessTitleEdit" runat="server" CssClass="Titolo_campoSmall">% Task Completeness:</asp:Label>
            </div>
            <div style="text-align: left; float: left">
                <div>
                    <div style="float: left; width: 60px; text-align: left; padding: 0px 2px 0px 0px">
                        <asp:Label ID="LBtaskCompletenssEdit" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                        %
                    </div>
                    <div style="padding: 0px 6px 0px 5px; float: left; width: 100px; text-align: left;">
                        <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                            border: 1px solid black">
                            <asp:Image ID="IMtaskCompletenessEdit" runat="server" BackColor="Green" Height="15px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="DIVpersonalCompletenessEdit" class="ExternalItem" runat="server">
            <div class="InternalItem">
                <asp:Label ID="LBpersonalCompletenessTitleEdit" runat="server" CssClass="Titolo_campoSmall">% Personal Completeness:**</asp:Label>
            </div>
            <div>
                <div style="float: left; width: 50px; text-align: left; padding: 0px 3px 0px 0px">
                    <asp:TextBox ID="TXTpersonalCompletenessEdit" runat="server" Width="30px" MaxLength="3"
                        TextMode="SingleLine"></asp:TextBox>
                    %
                </div>
                <div style="width: 10px; float: left">
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Il valore deve essere compreso tra 0 e 100"
                        Text="*" ControlToValidate="TXTpersonalCompletenessEdit" MinimumValue="0" MaximumValue="100"
                        Type="Integer"></asp:RangeValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Il valore deve essere un intero"
                        Text="*" ControlToValidate="TXTpersonalCompletenessEdit" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                </div>
                <div style="padding: 0px 6px 0px 5px; float: left; width: 100px; text-align: left;">
                    <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                        border: 1px solid black">
                        <asp:Image ID="IMpersonalCompletenessEdit" runat="server" BackColor="Green" Height="15px" />
                    </div>
                </div>
                <div style="float: left">
                    <asp:Button ID="BTNupdatePersonalCompletenessEdit" runat="server" Text="**Update"
                        CssClass="Link_Menu" />
                </div>
            </div>
        </div>
        <div id="DIVpriorityEdit" class="ExternalItem">
            <div class="InternalItem">
                <asp:Label ID="LBpriorityEdit" runat="server" CssClass="Titolo_campoSmall">Priority:</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:DropDownList ID="DDLpriorityEdit" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div id="DIVstatusEdit" class="ExternalItem">
            <div class="InternalItem">
                <asp:Label ID="LBstatusEdit" runat="server" CssClass="Titolo_campoSmall">Status:</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:DropDownList ID="DDLstatusEdit" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div id="DIVnoteEdit">
            <div class="InternalItem">
                <asp:Label ID="LBnoteTitleEdit" runat="server" CssClass="Titolo_campoSmall">Note:</asp:Label>
            </div>
            <div style="text-align: left;">
                <asp:TextBox ID="TXTnoteEdit" TextMode="MultiLine" runat="server"  Width="80%" Rows="2" CssClass="Testo_campoSmall"
                    Wrap="true" MaxLength="10"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REVnoteLength" runat="server" ControlToValidate="TXTnoteEdit"
                    Display="Dynamic" ErrorMessage="*" ValidationExpression="[\s\S]{1,4000}"></asp:RegularExpressionValidator>
            </div>
        </div>
    </asp:View>
</asp:MultiView>
