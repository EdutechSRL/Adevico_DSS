'X chi utilizza questa pagina
'Nel link che la richiama aggiungere il parametro From
'ed aggiungere il valore passato nel select case Di Page_Load
'e ricostruire il link della pagina che richiama questa...
'
'Per eventuali esempi vedere /Chat/ChatEmoticon.aspx

Public Class AggiungiFaccina
    Inherits System.Web.UI.Page
    Private PageFrom As String


    Protected WithEvents LBname_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents RFVNome As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents LBabbreviazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBabbreviazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents RFVAbb As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents LBemoticon As System.Web.UI.WebControls.Label

    Protected WithEvents UplImg As System.Web.UI.HtmlControls.HtmlInputImage
    Protected WithEvents UplFile As System.Web.UI.HtmlControls.HtmlInputFile

    Protected WithEvents BTNupload As System.Web.UI.WebControls.Button
    Protected WithEvents BTNback As System.Web.UI.WebControls.Button
    Protected WithEvents BTNnew As System.Web.UI.WebControls.Button


    Protected WithEvents LBerrore As System.Web.UI.WebControls.Label


    Protected WithEvents REVNome As System.Web.UI.WebControls.RegularExpressionValidator

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
        'Inserire qui il codice utente necessario per inizializzare la pagina
        PageFrom = Request.QueryString("From")
        Select Case PageFrom
            Case "Chat" 'Per ritornare alla pagina di visualizzazione delle faccine della chat
                PageFrom = "./../Chat/ChatEmoticon.aspx"
            Case Else
                PageFrom = "" 'Da verificare...
        End Select
    End Sub

#Region " Bottoni "

    Private Sub BtBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNback.Click
        Response.Redirect(PageFrom)
    End Sub

    Private Sub BtNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNnew.Click
        Me.TXBabbreviazione.Text = ""
        Me.TXBnome.Text = ""
        'Me.UplFile.Value = ""
    End Sub

    Private Sub BtUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupload.Click

        If Page.IsValid Then
            If Not TXBabbreviazione.Text = "" And Not TXBnome.Text = "" Then
                Dim strFileName As String = Me.ImgUrl() 'Nome del file comprendente che verrà salvato sul server
                If Not Me.UplFile.PostedFile Is Nothing Then
                    Try
                        lm.Comol.Core.File.Create.UploadFile(UplFile, strFileName)
                        Me.LBerrore.Text = ""
                        Me.LBerrore.DataBind()

                        'Aggiornamento immagine
                        '.\images\Emoticon\smile.gif
                        Me.UplImg.Src = ".\images\Emoticon\" & Me.TXBnome.Text & ".gif"
                        Me.UplImg.DataBind()

                        'Codice inserimento nell'xml
                        'Try
                        '    Dim StrXMLDataPath As String = Request.MapPath(GetPercorsoApplicazione(me.Request) & "/Emoticon.xml") '"Emoticon.xml" 'GetPercorsoApplicazione(me.Request) & "/Emoticon.xml"
                        '    DSEmo.ReadXml(StrXMLDataPath)
                        'Catch ex As Exception
                        'End Try
                        Dim StrXMLSchemaPath As String = Request.MapPath(GetPercorsoApplicazione(me.Request) & "/Emoticon.xsd") '"Emoticon.xml" 'GetPercorsoApplicazione(me.Request) & "/Emoticon.xml"
                        Dim StrXMLDataPath As String = Request.MapPath(GetPercorsoApplicazione(me.Request) & "/Emoticon.xml")
                        Dim DsEmo As New DataSet
                        DsEmo.ReadXmlSchema(StrXMLSchemaPath)
                        DsEmo.ReadXml(StrXMLDataPath)
                        Dim Row As DataRow
                        Row = DsEmo.Tables(0).NewRow
                        With Row
                            .Item(0) = Me.TXBnome.Text
                            .Item(1) = Me.TXBnome.Text.ToLower & ".gif"
                            .Item(2) = Me.TXBabbreviazione.Text
                        End With
                        DsEmo.Tables(0).Rows.Add(Row)
                        DsEmo.WriteXml(StrXMLDataPath)
                    Catch ex As Exception
                        Me.LBerrore.Text = ex.ToString
                        Me.LBerrore.DataBind()
                    End Try
                End If
            End If
        Else
            'Me.LBerrore.Text = "errori"
        End If
    End Sub

#End Region

#Region "Private Funztion"
    Private Function ImgUrl() As String
        'URL sul server
        'Request.MapPath(GetPercorsoApplicazione(me.Request) & "/Emoticon.xml")
        Dim str As String = TXBnome.Text()

        'Il controllo per impedire che vengano inseriti caratteri strani
        'è stato implementato nella pagina...
        Return Server.MapPath(GetPercorsoApplicazione(me.Request)) & "./../images/Emoticon/" & str & ".gif"

    End Function
#End Region

End Class
