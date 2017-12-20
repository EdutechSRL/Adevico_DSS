Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi


Public Class AdminG_AggiungiTipoRuolo
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents RPTnome As System.Web.UI.WebControls.Repeater
    Protected WithEvents LNBchiudi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcrea As System.Web.UI.WebControls.LinkButton
    Protected WithEvents RPTdescrizione As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBtitoloRuolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlinguaNome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlinguaDescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlivello_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoli_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlivelloRuolo_t As System.Web.UI.WebControls.Label

#Region "Pannello inerimento/modifica"
    Protected WithEvents RBlivello As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TBLlivelli As System.Web.UI.WebControls.Table
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBdescrizione As System.Web.UI.WebControls.TextBox
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oPersona As New COL_Persona

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            Session("azione") = "load"

            oPersona = Session("objPersona")
            Me.PNLpermessi.Visible = False
            Me.PNLcontenuto.Visible = False
            Me.SetupInternazionalizzazione()
			If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
				Me.PNLcontenuto.Visible = True
				Me.LNBcrea.Visible = True
				Me.Bind_Dati()
			Else
				Me.PNLpermessi.Visible = True
				Me.LNBcrea.Visible = False
			End If
        End If
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

        End Try
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
          
            Response.Write("<script language='javascript'>function AlertLogoutClose(Messaggio){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "window.close();" & vbCrLf & "} " & vbCrLf & "AlertLogoutClose('" & alertMSG & "');</script>")
            Return True
        End If
    End Function

    Private Sub Bind_Dati()
        Me.Bind_Lingue()
        Me.Bind_Livelli()
    End Sub
    Private Sub Bind_Lingue()
		Try
			Me.RPTnome.DataSource = ManagerLingua.List
			Me.RPTdescrizione.DataSource = ManagerLingua.List
			Me.RPTnome.DataBind()
			Me.RPTdescrizione.DataBind()
		Catch ex As Exception

		End Try
    End Sub

    Private Sub Bind_Livelli()
        Dim oTipoRuolo As New COL_TipoRuolo
        Dim nomeRuoli As String
        Dim oDataset As DataSet
        Dim TPRL_gerarchiaMAX, i, j, totale As Integer
        Try
            Dim oDataview As New DataView

            Me.RBlivello.Items.Clear()

            TPRL_gerarchiaMAX = oTipoRuolo.EstraiGerarchiaMassima
            oDataset = oTipoRuolo.Elenca(Session("LinguaID"), True)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then

            Else
                oDataview = oDataset.Tables(0).DefaultView
                For i = 0 To TPRL_gerarchiaMAX + 1
                    Dim oTbrow As New TableRow
                    Dim oCell As New TableCell
                    Dim oCell2 As New TableCell
                    RBlivello.Items.Insert(i, i)

                    oCell.CssClass = "ROW_TD_Small9"
                    oDataview.RowFilter = "TPRL_Gerarchia=" & i
                    If oDataview.Count = 0 Then
                        oCell.Text = "&nbsp;"
                    Else
                        nomeRuoli = ""
                        For j = 0 To oDataview.Count - 1
                            Dim oRow As DataRow
                            oRow = oDataview.Item(j).Row
                            If nomeRuoli = "" Then
                                nomeRuoli = oRow.Item("TPRL_Nome")
                            Else
                                nomeRuoli = nomeRuoli & ", " & oRow.Item("TPRL_Nome")
                            End If

                            oCell.BorderColor = System.Drawing.Color.LightGray
                            oCell.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
                            oCell2.BorderColor = System.Drawing.Color.LightGray
                            oCell2.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
                            oCell2.CssClass = "ROW_TD_Small"
                            oCell2.HorizontalAlign = HorizontalAlign.Center

                            oCell2.Text = i
                            oCell.Text = nomeRuoli
                            oTbrow.Cells.Add(oCell2)
                            oTbrow.Cells.Add(oCell)
                        Next
                    End If

                    Me.TBLlivelli.Rows.Add(oTbrow)
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_modificaTipoComunita"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBtitoloRuolo)
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBnome_t)
            .setLabel(Me.LBlinguaNome_t)
            .setLabel(Me.LBdescrizione_t)
            .setLabel(Me.LBlinguaDescrizione_t)
            .setLabel(Me.LBlivello_t)
            .setLabel(Me.LBruoli_t)
            .setLabel(Me.LBlivelloRuolo_t)
            .setLinkButton(Me.LNBchiudi, True, True)
            .setLinkButton(Me.LNBcrea, True, True)
            Me.LNBchiudi.Attributes.Add("onclick", "window.close(); return false")
        End With
    End Sub
#End Region

    Private Sub LNBcrea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcrea.Click
        Dim alertMSG As String = ""
        Dim oTipoRuolo As New COL_TipoRuolo

        If Me.RBlivello.SelectedIndex < 0 Then
            alertMSG = Me.oResource.getValue("ErroreLivello")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            Me.Bind_Livelli()
            Exit Sub
        ElseIf Me.TXBnome.Text = "" Then
            alertMSG = Me.oResource.getValue("ErroreNome")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            Me.Bind_Livelli()
            Exit Sub
        End If

        Try
            oTipoRuolo.Nome = Me.TXBnome.Text
            oTipoRuolo.Descrizione = Me.TXBdescrizione.Text
            oTipoRuolo.Gerarchia = Me.RBlivello.SelectedValue
            oTipoRuolo.noDelete = False
            oTipoRuolo.noModify = False
            oTipoRuolo.Aggiungi()
            If oTipoRuolo.Errore = Errori_Db.None Then
                Dim i, totale, LinguaID As Integer
                Dim Termine As String = ""
                Dim Descrizione As String = ""

                totale = Me.RPTnome.Items.Count
                If totale > 0 Then
                    For i = 0 To totale - 1
                        Dim oLabel As Label
                        Dim oText As TextBox

                        Try
                            oLabel = Me.RPTnome.Items(i).FindControl("LBlinguaID")
                            LinguaID = oLabel.Text
                        Catch ex As Exception
                            LinguaID = 0
                        End Try
                        If LinguaID > 0 Then
                            Try
                                oText = Me.RPTnome.Items(i).FindControl("TXBtermine")
                                Termine = oText.Text
                            Catch ex As Exception
                                Termine = ""
                            End Try

                            If Termine = "" Then
                                Termine = Me.TXBnome.Text
                            End If

                            Try
                                oText = Me.RPTdescrizione.Items(i).FindControl("TXBtermine2")
                                Descrizione = oText.Text
                            Catch ex As Exception
                                Descrizione = ""
                            End Try

                            If Descrizione = "" Then
                                Descrizione = Me.TXBdescrizione.Text
                            End If

                            oTipoRuolo.Translate(Termine, Descrizione, LinguaID)
                        End If
                    Next
                    'this.opener.document.forms[0].HDNazione.value

                    Dim ScriptJava As String = "<script language='javascript'>try{" & vbCrLf & _
                    "var Azione = window.opener.$(""input[id$='HDNazione']"").val();" & vbCrLf & _
                    "if (!(Azione=='gestioneTipo'))" & vbCrLf & _
                    "this.close();" & vbCrLf & _
                    "else {" & vbCrLf & _
                    "    Azione='reload';" & vbCrLf & _
                    "		this.opener.document.forms[0].submit(); //this.opener.document.forms[0].__doPostBack(); //this.window.close();" & vbCrLf & _
                    "	}}" & vbCrLf & _
                    "catch(e){this.close();} </script>" & vbCrLf
                    Response.Write(ScriptJava)
                Else
                    Me.Bind_Livelli()
                    alertMSG = Me.oResource.getValue("ErroreRuolo")
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub RPTnome_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnome.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaNome_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTdescrizione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTdescrizione.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaDescrizione_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub

End Class