Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_WZDid_Fase1SceltaDati
    Inherits System.Web.UI.UserControl
    Private oResourceUCsceltaDati As ResourceManager

    Protected WithEvents CTRLsorgenteComunita As Comunita_OnLine.UC_FiltroComunitaByServizio_NEW

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
        If IsNothing(oResourceUCsceltaDati) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub

    Public Sub SetupControl(ByVal ComunitaID As Integer, ByVal ComunitaPath As String)
        If IsNothing(oResourceUCsceltaDati) Then
            'Me.SetCulture(Session("LinguaCode"))
        End If
        Me.CTRLsorgenteComunita.ShowFiltro = True
        Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_GestioneIscritti.Codex
        Me.CTRLsorgenteComunita.SetupControl(, , ComunitaID, ComunitaPath)
        Me.CTRLsorgenteComunita.Visible = True


        'Me.HDN_Livello.Value = Livello
        'Me.HDN_ComunitaAttualePercorso.Value = ComunitaAttualePercorso
        'Me.HDN_ComunitaAttualeID.Value = ComunitaAttualeID
        'Me.HDNhasSetup.Value = True
        'Me.SetupInternazionalizzazione()
        'Me.SetupFiltri()
    End Sub
    Private Sub Setup_ComunitàOrigine()
        Dim oComunitaAttuale As New COL_Comunita
        Dim ComunitaId As Integer
        Dim ComunitaPath As String = ""

        Try
            If Session("AdminForChange") = False Then
                ComunitaId = Session("IdComunita")
                Try
                    Dim ArrComunita(,) As String = Session("ArrComunita")
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch ex As Exception

                End Try
            Else
                ComunitaId = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            End If
        Catch ex As Exception

        End Try

        oComunitaAttuale.Id = ComunitaId
        oComunitaAttuale.Estrai()

        If oComunitaAttuale.Errore = Errori_Db.None Then
            'If Me.HDN_ComunitaPadreID.Value = 0 Then
            '    Me.RBLimporta.SelectedIndex = 0
            '    Me.TBRcomunita.Visible = False
            '    Me.BTNavanti.Enabled = True
            '    Me.BTNavanti2.Enabled = True
            'ElseIf Me.HDN_ComunitaPadreID.Value = -1 Then
            '    ComunitaPathPadre = Replace(ComunitaPath, "." & ComunitaId & ".", ".")
            '    If ComunitaPathPadre = "" Or ComunitaPathPadre = "." Or ComunitaPathPadre = ".." Then
            '        ComunitaPadreId = 0
            '    Else
            '        Dim oArrayPadri() As String
            '        oArrayPadri = ComunitaPathPadre.Split(".")
            '        ComunitaPadreId = oArrayPadri(oArrayPadri.Length - 2)
            '    End If
            '    


            '    If Me.CTRLsorgenteComunita.ComunitaID > 0 Then
            '        Me.BTNavanti.Enabled = True
            '    Else
            '        Me.BTNavanti.Enabled = False
            '    End If
            '    Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
            'End If

        End If
    End Sub


#Region "internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceUCsceltaDati = New ResourceManager

        oResourceUCsceltaDati.UserLanguages = code
        oResourceUCsceltaDati.ResourcesName = "pg_WZDid_Fase1SceltaDati"
        oResourceUCsceltaDati.Folder_Level1 = "Comunita"
        oResourceUCsceltaDati.Folder_Level2 = "UC_WizardImportaDati"
        oResourceUCsceltaDati.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        If IsNothing(oResourceUCsceltaDati) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        With oResourceUCsceltaDati
          
        End With
    End Sub
#End Region
End Class
