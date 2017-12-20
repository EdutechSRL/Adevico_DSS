Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi


Public Class DettagliServizio
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

#Region "Form Dettagli Servizio"
    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoNome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoDescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescrizione As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoCodice_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcodice As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoAttivo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBattivo As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoDisattivabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdisattivabile As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoPermessi_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBpermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLimpostazioni As System.Web.UI.WebControls.Panel

    Protected WithEvents LBinfoTipiComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBLtipicomunità As System.Web.UI.WebControls.Table
    Protected WithEvents LNBchiudi As System.Web.UI.WebControls.LinkButton
#End Region

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
            Me.LNBchiudi.Attributes.Add("onclick", "Javascript:window.close();return false;")
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
            Try
                Dim ServizioID As Integer
                ServizioID = Me.Request.QueryString("SRVZ_ID")
                Me.Bind_Dati(ServizioID)
            Catch ex As Exception
                Response.Write("<script language='javascript'>window.close();');</script>")
                Response.End()
                Me.PNLimpostazioni.Visible = False
            End Try

        End If

    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AdminG_ManagementServizi"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBinfoAttivo_t)
            .setLabel(Me.LBinfoCodice_t)
            .setLabel(Me.LBinfoDescrizione_t)
            .setLabel(Me.LBinfoDisattivabile_t)
            .setLabel(Me.LBinfoNome_t)
            .setLabel(Me.LBinfoPermessi_t)
            .setLabel(Me.LBinfoTipiComunita_t)
            .setLinkButton(Me.LNBchiudi, True, True)
            .setLabel(Me.LBtitolo)
        End With
    End Sub
#End Region

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
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
        Else
            Try
                If Me.Request.QueryString("SRVZ_ID") > 0 Then
                    Return False
                End If
            Catch ex As Exception

            End Try
            Response.Write("<script language='javascript'>window.close();</script>")
            Response.End()
            Return True
        End If
    End Function

    Private Sub Bind_Dati(ByVal ServizioID As Integer)
        Dim oServizio As New COL_Servizio
        Dim oDataset As New DataSet
        Dim i, totale As Integer

        Try
            oServizio.ID = ServizioID
            oServizio.EstraiByLingua(Session("LinguaID"))

            If oServizio.Errore = Errori_Db.None Then
                Me.LBnome.Text = oServizio.Nome
                Me.LBdescrizione.Text = oServizio.Descrizione
                Me.LBcodice.Text = oServizio.Codice
                If oServizio.isNonDisattivabile Then
                    Me.LBdisattivabile.Text = Me.oResource.getValue("no")
                Else
                    Me.LBdisattivabile.Text = Me.oResource.getValue("si")
                End If
                If oServizio.isAttivato Then
                    Me.LBattivo.Text = Me.oResource.getValue("si")
                Else
                    Me.LBattivo.Text = Me.oResource.getValue("no")
                End If

                Try
                    Dim permessi As String

                    oDataset = oServizio.ElencaPermessiAssociatiByLingua(Session("LinguaID"))
                    totale = oDataset.Tables(0).Rows.Count
                    permessi = ""

                    If totale > 0 Then
                        For i = 0 To totale - 1
                            Dim oRow As DataRow

                            oRow = oDataset.Tables(0).Rows(i)
                            If Not IsDBNull(oRow.Item("Nome")) Then
                                If permessi = "" Then
                                    permessi = oRow.Item("Nome")
                                Else
                                    permessi = permessi & ", " & oRow.Item("Nome")
                                End If
                            Else
                                If Not IsDBNull(oRow.Item("NomeDefault")) Then
                                    If permessi = "" Then
                                        permessi = oRow.Item("NomeDefault")
                                    Else
                                        permessi = permessi & ", " & oRow.Item("NomeDefault")
                                    End If
                                End If
                            End If
                        Next
                        Me.LBpermessi.Text = permessi
                    Else
                        Me.LBpermessi.Text = Me.oResource.getValue("noPermesso")
                    End If
                Catch ex As Exception
                    Me.LBpermessi.Text = Me.oResource.getValue("noPermesso")
                End Try

                'Definizione tipologie comunità associate:

                Try
                    oDataset = oServizio.ElencaTipiComunita(-1)
                    totale = oDataset.Tables(0).Rows.Count

                    Me.TBLtipicomunità.Rows.Clear()

                    If totale > 0 Then
                        For i = 0 To totale - 1
                            Dim oTableRow As New TableRow
                            Dim oTableCell_0 As New TableCell
                            Dim oTableCell_1 As New TableCell
                            Dim oRow As DataRow

                            oRow = oDataset.Tables(0).Rows(i)
                            If CBool(oRow.Item("Associato")) Then

                                oTableCell_0.Text = oRow.Item("TPCM_descrizione") & "&nbsp;"
                                oTableCell_0.CssClass = "Testo_CampoSmall"
                                oTableCell_1.CssClass = "Testo_CampoSmall"
                                If CBool(oRow.Item("LKST_default")) Then
                                    oTableCell_1.Text = Me.oResource.getValue("servizio.Default") ' "Attivo di default"
                                Else
                                    oTableCell_1.Text = Me.oResource.getValue("servizio.Disattivo") ' "Disattivo"
                                End If
                                oTableRow.Cells.Add(oTableCell_0)
                                oTableRow.Cells.Add(oTableCell_1)
                                Me.TBLtipicomunità.Rows.Add(oTableRow)
                            End If
                        Next
                    Else
                    End If
                Catch ex As Exception
                End Try
            Else
                Response.Write("<script language='javascript'>window.close();</script>")
                Me.PNLimpostazioni.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class