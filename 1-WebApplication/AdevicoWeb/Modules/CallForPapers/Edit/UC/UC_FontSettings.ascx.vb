Imports lm.Comol.Modules.CallForPapers.Domain

Public Class UC_FontSettings
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property FontSettings As CallPrintFontSets
        Get
            Dim cpf As New CallPrintFontSets
            With cpf
                cpf.FontName = Me.DDLfontName.SelectedValue
                cpf.Size = System.Convert.ToInt32(Me.DDLfontSize.SelectedValue)
                cpf.Variant = FontVariant.None

                If (Me.CBXbold.Checked) Then
                    cpf.Variant = cpf.Variant & FontVariant.Bold
                End If

                If (Me.CBXitalic.Checked) Then
                    cpf.Variant = cpf.Variant & FontVariant.Italic
                End If

                If (Me.CBXunderline.Checked) Then
                    cpf.Variant = cpf.Variant & FontVariant.Underline
                End If
            End With

            Return cpf
        End Get

        Set(value As CallPrintFontSets)

            Try
                Me.DDLfontName.SelectedValue = value.FontName
            Catch ex As Exception
                Me.DDLfontName.SelectedValue = "TIMES"
            End Try

            Try
                Me.DDLfontSize.SelectedValue = value.Size
            Catch ex As Exception
                Me.DDLfontName.SelectedValue = "12"
            End Try


            Me.CBXbold.Checked = ((value.Variant And FontVariant.Bold) = FontVariant.Bold)
            Me.CBXitalic.Checked = ((value.Variant And FontVariant.Italic) = FontVariant.Italic)
            Me.CBXunderline.Checked = ((value.Variant And FontVariant.Underline) = FontVariant.Underline)

        End Set
    End Property

    Public WriteOnly Property IsReadonly As Boolean
        Set(value As Boolean)
            Me.DDLfontName.Enabled = Not value
            Me.DDLfontSize.Enabled = Not value
            Me.CBXbold.Enabled = Not value
            Me.CBXitalic.Enabled = Not value
            Me.CBXunderline.Enabled = Not value
        End Set
    End Property

    ''' <summary>
    ''' Value dovrebbe distinguere un controllo dall'altro.
    ''' Nel caso specifico, però, i testi contenuti sono uguali per tutti i controlli: le etichette che indicano il campo sono esterne.
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <param name="Resource"></param>
    ''' <remarks></remarks>
    Public Sub SetInternazionalizzazione( _
                                        ByVal Value As String, _
                                        ByVal Resource As ResourceManager)
        With Resource
            .setCheckBox(CBXbold)
            .setCheckBox(CBXitalic)
            .setCheckBox(CBXunderline)
        End With
    End Sub

End Class