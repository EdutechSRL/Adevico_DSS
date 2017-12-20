Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo



Public Class UC_infoEsperienzeLavorative
    Inherits System.Web.UI.UserControl
    Protected oResourceEsperienze As ResourceManager

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


#Region "accessori"
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents HDNcreu_id As System.Web.UI.HtmlControls.HtmlInputHidden
    ' Protected WithEvents HDNeslv_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLesperienze As System.Web.UI.WebControls.Panel
#End Region
#Region "lista"

    Protected WithEvents RPTesperienze As System.Web.UI.WebControls.Repeater

    Protected WithEvents TBRsettore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRtipoImpiego As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmansione As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBinizio_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBfine_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBMeseInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBAnnoInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBMeseFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBAnnoFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBsettore_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoImpiego_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBmansione_s As System.Web.UI.WebControls.Label
#End Region

    Public Property PRSN_ID() As Integer
        Get
            If Me.HDNprsn_id.Value = "" Then
                Me.HDNprsn_id.Value = 0
            End If
            PRSN_ID = Me.HDNprsn_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNprsn_id.Value = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            If Page.IsPostBack = False Then
                Me.SetupInternazionalizzazione()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Function start() As Boolean
        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If

        Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
        Dim PRSN_id As Integer
        PRSN_id = Me.HDNprsn_id.Value

        oEsperienzeLavorative.EstraiByPersona(PRSN_id)

        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
        End If
        If oEsperienzeLavorative.ID = -1 Then
            PNLesperienze.Visible = False
            Return False
        Else
            PNLesperienze.Visible = True
            Bind_Dati()
            Return True
        End If
    End Function

#Region "Bind_Dati"
    Private Sub Bind_Dati()
        Dim PRSN_id As Integer
        Dim oDataset As DataSet
        Dim i, totale As Integer
        Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
        Try
            PRSN_id = Me.HDNprsn_id.Value
            oDataset = oEsperienzeLavorative.ElencaByPersona(PRSN_id, Main.FiltroVisibilità.Pubblici)
            totale = oDataset.Tables(0).Rows.Count

            If totale = 0 Then 'se datagrid vuota
                Me.PNLesperienze.Visible = False
            Else
                Viewstate("startEsperienze") = ""
                oDataset.Tables(0).Columns.Add(New DataColumn("oInizio"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oFine"))
                For i = 0 To totale - 1
                    Try
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)
                        If Not IsDBNull(oRow.Item("ESLV_inizio")) Then
                            oRow.Item("oInizio") = Format(CDate(oRow.Item("ESLV_inizio")), "d/M/yyyy")
                        Else
                            oRow.Item("oInizio") = oResourceEsperienze.getValue("null") '"Null" 'oResource.
                        End If

                        'If Not IsDBNull(oRow.Item("ESLV_fine")) Then
                        '    oRow.Item("oFine") = Format(CDate(oRow.Item("ESLV_fine")), "d/M/yyyy")
                        'Else
                        '    oRow.Item("oFine") = "Null" 'oResource.
                        'End If
                        If oRow.Item("ESLV_esperienzaInCorso") = "1" Then
                            oRow.Item("oFine") = "Esperienza in Corso" 'oResource.
                        Else
                            If Not IsDBNull(oRow.Item("ESLV_fine")) Then
                                oRow.Item("oFine") = Format(CDate(oRow.Item("ESLV_fine")), "d/M/yyyy")
                            Else
                                oRow.Item("oFine") = oResourceEsperienze.getValue("null") '"Null" 'oResource.
                            End If
                        End If

                        If Not IsDBNull(oRow.Item("ESLV_nomeDatore")) Then
                            oRow.Item("ESLV_nomeDatore") = Replace(oRow.Item("ESLV_nomeDatore"), vbCrLf, "<br>")
                        End If
                        If Not IsDBNull(oRow.Item("ESLV_settore")) Then
                            oRow.Item("ESLV_settore") = Replace(oRow.Item("ESLV_settore"), vbCrLf, "<br>")
                        End If
                        If Not IsDBNull(oRow.Item("ESLV_tipoImpiego")) Then
                            oRow.Item("ESLV_tipoImpiego") = Replace(oRow.Item("ESLV_tipoImpiego"), vbCrLf, "<br>")
                        End If
                        If Not IsDBNull(oRow.Item("ESLV_mansione")) Then
                            oRow.Item("ESLV_mansione") = Replace(oRow.Item("ESLV_mansione"), vbCrLf, "<br>")
                        End If

                    Catch ax As Exception

                    End Try
                Next
                RPTesperienze.DataSource = oDataset
                RPTesperienze.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "gestione repeater"
    Private Sub RPTesperienze_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTesperienze.ItemCreated
        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try


                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBinizio_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBinizio_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBfine_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBfine_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBnome_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBnome_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBsettore_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBsettore_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBtipoImpiego_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBtipoImpiego_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBmansione_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBmansione_s.text")
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If
    End Sub

#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceEsperienze = New ResourceManager

        oResourceEsperienze.UserLanguages = Code
        oResourceEsperienze.ResourcesName = "pg_UC_EsperienzeLavorative"
        oResourceEsperienze.Folder_Level1 = "Curriculum"
        oResourceEsperienze.Folder_Level2 = "UC"
        oResourceEsperienze.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceEsperienze
            '.setLabel(LBinizio_t)
            '.setLabel(LBfine_t)
            '.setLabel(LBMeseInizio)
            '.setLabel(LBAnnoInizio)
            '.setLabel(LBMeseFine)
            '.setLabel(LBAnnoFine)
            '.setLabel(LBnome_t)
            '.setLabel(LBsettore_t)
            '.setLabel(LBtipoImpiego_t)
            '.setLabel(LBmansione_t)
            '.setLabel(LBrendiPubblico_t)
        End With
    End Sub

#End Region


    Private Sub RPTesperienze_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTesperienze.ItemDataBound
        Dim oTableRow As New TableRow
        Try

            oTableRow = e.Item.FindControl("TBRsettore")
            If IsDBNull(e.Item.DataItem("ESLV_settore")) Then
                oTableRow.Visible = False
            ElseIf e.Item.DataItem("ESLV_settore") = "" Then
                oTableRow.Visible = False
            Else
                oTableRow.Visible = True
            End If
        Catch ex As Exception

        End Try

        Dim oTableRow2 As New TableRow
        Try

            oTableRow2 = e.Item.FindControl("TBRtipoImpiego")
            If IsDBNull(e.Item.DataItem("ESLV_tipoImpiego")) Then
                oTableRow2.Visible = False
            ElseIf e.Item.DataItem("ESLV_tipoImpiego") = "" Then
                oTableRow2.Visible = False
            Else
                oTableRow2.Visible = True

            End If
        Catch ex As Exception

        End Try

        Dim oTableRow3 As New TableRow
        Try

            oTableRow3 = e.Item.FindControl("TBRmansione")
            If IsDBNull(e.Item.DataItem("ESLV_mansione")) Then
                oTableRow3.Visible = False
            ElseIf e.Item.DataItem("ESLV_mansione") = "" Then
                oTableRow3.Visible = False
            Else
                oTableRow3.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
