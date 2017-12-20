<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RenderCriterion.ascx.vb" Inherits="Comunita_OnLine.UC_RenderCriterion" %>

<asp:MultiView ID="MLVcriterion" runat="server">
    <asp:View ID="VIWunknown" runat="server">
    
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWsingleline" runat="server">
        <div class="fieldobject renderobject singleline">
            <div class="fieldrow fieldinput" runat="server" id="DVsingleline">
                <span class="revisionfield" id="SPNsinglelineRevisionField" runat="server" visible="false">
                    <asp:CheckBox ID="CBXsinglelineRevisionField" runat="server" visible="false"/>
                    <asp:Label ID="LBsinglelineRevisionField" runat="server"  visible="false"></asp:Label>
                </span>
                <asp:Label runat="server" id="LBsinglelineText" AssociatedControlID="LBsinglelineValue" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBsinglelineDescription" CssClass="description">Description</asp:Label>
                </div>
                <div class="fieldmaincontent clearfix">
                    <div class="left">
                        <asp:Label runat="server" id="LBsinglelineValue" CssClass="readonlyinput"></asp:Label>
                    </div>
                    <div class="right">
                        &nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
                    </div>
                </div>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharsingleline"  Visible="false">
                        <asp:Literal ID="LTmaxCharssingleline" runat="server"></asp:Literal>
                        <span class="availableitems"><asp:Literal ID="LTsinglelineUsed" runat="server"></asp:Literal></span>/<span class="totalitems"><asp:Literal ID="LTsinglelineTotal" runat="server"></asp:Literal></span>
                    </span>
                    <asp:Label ID="LBerrorMessagesingleline" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>        
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWmultiline" runat="server">
       <div class="fieldobject renderobject multiline">
            <div class="fieldrow fieldinput" runat="server" id="DVmultiline">
                <span class="revisionfield" id="SPNmultilineRevisionField" runat="server" visible="false">
                    <asp:CheckBox ID="CBXmultilineRevisionField" runat="server" visible="false"/>
                    <asp:Label ID="LBmultilineRevisionField" runat="server" visible="false"></asp:Label>
                </span>
                <asp:Label runat="server" id="LBmultilineText" AssociatedControlID="LBmultilineValue" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBmultilineDescription" CssClass="description">Description</asp:Label>
                </div>
                <div class="fieldmaincontent clearfix">
                    <div class="left">
                        <asp:Label runat="server" id="LBmultilineValue" CssClass="readonlytextarea"></asp:Label>
                    </div>
                    <div class="right">
                        &nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
                    </div>
                </div>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharmultiline"  Visible="false">
                        <asp:Literal ID="LTmaxCharsmultiline" runat="server"></asp:Literal>
                        <span class="availableitems"><asp:Literal ID="LTmultilineUsed" runat="server"/></span>/<span class="totalitems"><asp:Literal ID="LTmultilineTotal" runat="server"></asp:Literal></span>
                    </span>
                    <asp:Label ID="LBerrorMessagemultiline" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span> 
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdropdownlist" runat="server">
        <div class="fieldobject renderobject dropdownlist">
            <div class="fieldrow fieldinput" runat="server" id="DVdropdownlist">
                <span class="revisionfield" id="SPNdropdownlistRevisionField" runat="server" visible="false">
                    <asp:CheckBox ID="CBXdropdownlistRevisionField" runat="server" visible="false"/>
                    <asp:Label ID="LBdropdownlistRevisionField" runat="server" visible="false"></asp:Label>
                </span>
                <asp:Label runat="server" ID="LBdropdownlistText" AssociatedControlID="DDLitems" CssClass="fieldlabel">Items</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBdropdownlistDescription" CssClass="description">Description</asp:Label>
                </div>
                <div class="fieldmaincontent clearfix">
                    <div class="left">
                        <asp:DropDownList runat="server" ID="DDLitems" Enabled="false"></asp:DropDownList>
                    </div>
                    <div class="right">
                        &nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
                    </div>
                </div>
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagedropdownlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>   
            </div>
        </div>
    </asp:View>
</asp:MultiView>