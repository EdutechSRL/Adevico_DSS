<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CategoryDelete.ascx.vb" Inherits="Comunita_OnLine.UC_CategoryDelete" %>
<%--Nomi Standard: OK--%>
<%@ Register TagPrefix="CTRL" TagName="DDLcategory" Src="~/Modules/Ticket/UC/UC_CategoryDDL.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RadioButtonList" Src="~/uc/UC_RadioButtonList.ascx" %>
<%--<asp:Literal ID="LITrblItemClass" runat="server">inlinewrapper</asp:Literal>--%>
 
<asp:Panel ID="PNLmain" runat="server" Visible="false">

<%--<asp:literal ID="LITrblLayout" runat="server" Visible="false">
	{text}
	<div class="description">{description}</div>
</asp:literal>--%>

<div class="view-modal view-delete dlgdelcategory" title="Cancellazione categoria">

	<asp:MultiView ID="MLVwizDelete" runat="server">
		<asp:View ID="V_1subCategory" runat="server">
			
			<div class="fieldobject step1">
	 
				<div class="fieldrow info">
					<div class="description disclaimer">
						<p><strong><asp:literal ID="LTst1Text1" runat="server">La categoria selezionata per la cancellazione contiene sottocategorie.</asp:literal></strong></p>
						<p><asp:literal ID="LTst1Text2" runat="server">Come si intende procedere con le sottocategorie?</asp:literal></p>
					</div>
	
				</div>

				<div class="fieldrow deleteoptions">
				<CTRL:RadioButtonList ID="CTRLrblDelCAtegory" runat="server" AutoPostBack="false"/>

					<%--<asp:RadioButtonList ID="RBLdelCAtegory" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="">
						<asp:ListItem Text="*MoveUP" Value="MoveUp" class="inlinewrapper"></asp:ListItem>
						<asp:ListItem Text="*DeleteAll" Value="DeleteAll" class="inlinewrapper"></asp:ListItem>	
						<asp:ListItem Text="*Reorder" Value="Reorder" class="inlinewrapper"></asp:ListItem>	

					</asp:RadioButtonList>--%>
				</div>
			</div>
		
		</asp:View>

		<asp:View ID="V_2subCatTkReassign" runat="server">
	<div class="fieldobject step1">
	 
				<div class="fieldrow info">
					<div class="description disclaimer">
						<p><strong>
						<asp:Literal ID="LTst2Text1" runat="server">*Ci sono {num} ticket correntemente assegnate alle categorie in cancellazione.</asp:Literal>
						</strong></p>
						<p><asp:Literal ID="LTst2Text2" runat="server">*Come si intende procedere con le sottocategorie?</asp:Literal></p>
					</div>
	
				</div>
                <div class="fieldrow deleteoptions">
				    <CTRL:RadioButtonList ID="CTRLrblTicketAss" runat="server" AutoPostBack="false"/>
                </div>
				<%--<asp:RadioButtonList ID="RBLticketAss" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="fieldrow deleteoptions">
					<asp:ListItem Text="*ReassignAll" Value="ReassignALL" class="inlinewrapper"></asp:ListItem>
					<asp:ListItem Text="*ReassignSingle" Value="ReassignSingle" class="inlinewrapper"></asp:ListItem>	
				</asp:RadioButtonList>--%>
			</div>
		</asp:View>

		<asp:View ID="V_3reassignALL" runat="server">
			<div class="fieldobject step3a reassignmap">
				<div class="fieldrow info">
					<div class="description">
						<p><asp:Literal ID="LTst3ReassignALL_t" runat="server">Tutte le categorie in cancellazione saranno riassegnate ad una singola categoria esistente.</asp:Literal></p>
					</div>
				</div>
				<div class="fieldrow ">
					<span class="fieldlabel auto"><asp:Literal ID="LTst3ReassignALLcate_t" runat="server">Categoria</asp:Literal></span>
					<CTRL:DDLcategory ID="CTRLddlCategory" runat="server" />
					<asp:Literal ID="LTerrorSingle" runat="server" Visible="false"></asp:Literal>
				</div>
			</div>
		</asp:View>

		<asp:View ID="V_4reassignSingle" runat="server">
			
			
			
			<div class="fieldobject step3b reassignmap">
						 
				<div class="fieldrow info">
					<div class="description">
						<p><asp:Literal ID="LTst3ReassignSinglecate_t" runat="server">Selezionare per ogni categoria in cancellazione la categoria di destinazione.</asp:Literal></p>
					</div>
				</div>

				<div class="fieldrow ">
					<div class="tablewrapper">


						<asp:Repeater ID="RPTreassignSingle" runat="server">
							<HeaderTemplate>
								<table class="table treetable light categorymap fullwidth">
									<thead>
									<tr>
										<th class="name">
											<asp:literal id="LTcateName_t" runat="server">Current category</asp:literal>
										</th>
										<th class="reassignedcat">
											<asp:Literal ID="LTnewCate_t" runat="server">Mapped category</asp:Literal>
										</th>
									</tr>
								</thead>
								<tbody>
							</HeaderTemplate>
							
							<ItemTemplate>
								<asp:Literal id="LITtr" runat="server"><tr id="ctg-{Id}" class="category {css}"></asp:Literal>
									<td class="currentcat">
										<asp:HiddenField ID="HIDid" runat="server" />
										<asp:Label ID="LBLcateName" runat="server">#Category name</asp:Label>
									</td>
									<td class="reassignedcat">
										<CTRL:DDLcategory ID="UCddlCate" runat="server" />
										<asp:literal ID="LTerrorCate" runat="server" Visible="false"><span class="error">*</span></asp:literal>
									</td>
								</tr>
							</ItemTemplate>

							<FooterTemplate>
								</tbody>
							</table>
							</FooterTemplate>
						</asp:Repeater>
						<asp:Literal ID="LTerrorMulti" runat="server" Visible="false"></asp:Literal>

					</div>
				</div>
			</div>
		</asp:View>

		<asp:View ID="V_5confirm" runat="server">
			<asp:Literal ID="LTconfirm" runat="server"></asp:Literal>
		</asp:View>

		<asp:View ID="V_6close" runat="server">
			<asp:Literal ID="LTclose" runat="server"></asp:Literal>
		</asp:View>
	</asp:MultiView>

	<div class="fieldrow buttons">
		<asp:LinkButton id="LNBundo" runat="server" CssClass="linkMenu close">*Cancel</asp:LinkButton><%--
		--%><asp:LinkButton id="LNBback" runat="server" CssClass="linkMenu back">*Back</asp:LinkButton><%--
		--%><asp:LinkButton id="LNBnext" runat="server" CssClass="linkMenu next">*Next</asp:LinkButton><%--
		--%><asp:LinkButton id="LNBconfirm" runat="server" CssClass="linkMenu confirm">*Save</asp:LinkButton><%--
		--%><asp:LinkButton id="LNBexit" runat="server" CssClass="linkMenu quit">*Quit</asp:LinkButton>
	</div>
</div>
</asp:Panel>



