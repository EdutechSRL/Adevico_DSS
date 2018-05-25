
Imports lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation
Imports lm.Comol.Modules.CallForPapers.Presentation

Imports System.IO

''' <summary>
''' Controllo esportazione XLSX
''' </summary>
Public Class Uc_AdvTableExport
    Inherits BaseControl
    Implements View.iViewTableExport

#Region "Context"
    Private _Presenter As AdvEcoTableExportPresenter
    ''' <summary>
    ''' Presenter
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property CurrentPresenter() As AdvEcoTableExportPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AdvEcoTableExportPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetCultureSettings()
        'Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        'Throw New NotImplementedException()
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        'Throw New NotImplementedException()
    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        'Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' Evento esportazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LKB_Export_Click(sender As Object, e As EventArgs) Handles LKB_Export.Click

        Dim stream As New MemoryStream()
        Dim renderedBytes As Byte() = Nothing

        Using stream

            Me.CurrentPresenter.ExportStream(stream)
            renderedBytes = stream.ToArray()
            '        current
            '        Me.pres(stream, rowsCount)
            '        renderedBytes = stream.ToArray()
        End Using


        'If SelectedDocumentFormat = SpreadDocumentFormat.Xlsx Then
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AppendHeader("content-disposition", String.Format("attachment; filename={0}", FileName))
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.BinaryWrite(renderedBytes)
        Response.[End]()
        'ElseIf SelectedDocumentFormat = SpreadDocumentFormat.Csv Then
        '    Response.Clear()
        '    Response.AppendHeader("content-disposition", "attachment; filename=ExportedFile.csv")
        '    Response.ContentType = "text/csv"
        '    Response.BinaryWrite(renderedBytes)
        '    Response.[End]()
        'End If

    End Sub

    ''' <summary>
    ''' Nome del file esportato
    ''' </summary>
    ''' <returns></returns>
    Public Property FileName
        Get
            If String.IsNullOrWhiteSpace(HDNfileName.Value) Then
                Return "ExportedFile.xlsx"
            Else
                Return HDNfileName.Value & ".xlsx"
            End If
        End Get
        Set(value)

            Dim out As String = value
            For Each c As Char In System.IO.Path.GetInvalidFileNameChars()
                out = out.Replace(c, "_")
            Next

            HDNfileName.Value = out
        End Set
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            'Throw New NotImplementedException()
        End Get
    End Property

    ''' <summary>
    ''' Id valutazione associata. Se vuoto l'UC non esporta!
    ''' </summary>
    ''' <returns></returns>
    Public Property EvaluationId As Long Implements View.iViewTableExport.EvaluationId
        Get
            Dim CurEvalId As Int64 = 0
            Try
                CurEvalId = ViewStateOrDefault("CurrentEvalId", 0)
            Catch ex As Exception

            End Try
            Return CurEvalId

        End Get
        Set(value As Long)
            ViewState("CurrentEvalId") = value
            'LTexport.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Classe link esportazione. Default: linkMenu, ma puo' essere impostato ad esempio "icon download" per visualizzarlo come icona
    ''' </summary>
    ''' <returns></returns>
    Public Property LinkClass As String
        Get
            Return LKB_Export.CssClass
        End Get
        Set(value As String)
            LKB_Export.CssClass = value
        End Set
    End Property

    ''' <summary>
    ''' Testo visualizzato
    ''' </summary>
    ''' <returns></returns>
    Public Property LinkText As String
        Get
            Return LKB_Export.Text
        End Get
        Set(value As String)
            LKB_Export.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Tooltip link
    ''' </summary>
    ''' <returns></returns>
    Public Property LinkToolTip As String
        Get
            Return LKB_Export.ToolTip
        End Get
        Set(value As String)
            LKB_Export.ToolTip = value
        End Set
    End Property
End Class