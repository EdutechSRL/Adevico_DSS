Public Class UC_Switch
    Inherits System.Web.UI.UserControl

    Public Event StatusChange(ByVal Status As Boolean)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Public Sub Init()
    '    Me.Enabled = True
    'End Sub

    Public Property LinkOn_Text As String
        Get
            Return Me.LKBon.Text
        End Get
        Set(value As String)
            Me.LKBon.Text = value
        End Set
    End Property

    Public Property LinkOn_ToolTip As String
        Get
            Return Me.LKBon.ToolTip
        End Get
        Set(value As String)
            Me.LKBon.ToolTip = value
        End Set
    End Property

    Public Property LinkOff_Text As String
        Get
            Return Me.LKBoff.Text
        End Get
        Set(value As String)
            Me.LKBoff.Text = value
        End Set
    End Property

    Public Property LinkOff_ToolTip As String
        Get
            Return Me.LKBoff.ToolTip
        End Get
        Set(value As String)
            Me.LKBoff.ToolTip = value
        End Set
    End Property

    Public Property Enabled As Boolean
        Get
            Return ViewStateOrDefault(Of Boolean)("Enabled", True)
        End Get
        Set(value As Boolean)
            ViewState("Enabled") = value

            LKBoff.Enabled = value
            LKBon.Enabled = value

            If value Then
                LKBon.CssClass = LKBon.CssClass.Replace(" disabled", "")
                LKBoff.CssClass = LKBoff.CssClass.Replace(" disabled", "")
            Else
                LKBon.CssClass &= " disabled"
                LKBoff.CssClass &= " disabled"
            End If
        End Set
    End Property

    Public Sub SetDescriptionText(ByVal Pre As String, ByVal Post As String)
        Me.LTdescriptionPre.Text = Pre
        Me.LTdescriptionPost.Text = Post
    End Sub

    Public Sub SetDescriptionValue(ByVal OnText As String, ByVal OffText As String)
        Me.ViewState("onValueText") = OnText
        Me.ViewState("offValueText") = OffText

        If (Status) Then
            Me.LTdescriptionValue.Text = OnText
        Else
            Me.LTdescriptionValue.Text = OffText
        End If
    End Sub

    Public Property Status As Boolean
        Get
            Return Me.ViewStateOrDefault(Of Boolean)("Status", False)
        End Get
        Set(value As Boolean)

            Me.ViewState("Status") = value
            Me.LKBon.CssClass = Me.LKBon.CssClass.Replace(" active", "")
            LKBon.Enabled = False
            Me.LKBoff.CssClass = Me.LKBoff.CssClass.Replace(" active", "")
            LKBoff.Enabled = False

            If value Then
                LKBon.CssClass &= " active"
                LKBoff.Enabled = Enabled

                Me.LTdescriptionValue.Text = ViewStateOrDefault(Of String)("onValueText", "On")
            Else
                LKBoff.CssClass &= " active"
                LKBon.Enabled = Enabled

                Me.LTdescriptionValue.Text = ViewStateOrDefault(Of String)("offValueText", "Off")
            End If


            'If Enabled Then
            '    LKBon.CssClass = LKBon.CssClass.Replace(" alldisabled", "")
            '    LKBoff.CssClass = LKBoff.CssClass.Replace(" alldisabled", "")
            'Else
            '    LKBon.CssClass &= " alldisabled"
            '    LKBoff.CssClass &= " alldisabled"
            'End If

        End Set
    End Property

    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

    Private Sub LKBon_Click(sender As Object, e As System.EventArgs) Handles LKBon.Click
        Me.Status = True
        RaiseEvent StatusChange(True)
    End Sub

    Private Sub LKBoff_Click(sender As Object, e As System.EventArgs) Handles LKBoff.Click
        Me.Status = False
        RaiseEvent StatusChange(False)
    End Sub

    Public Property ShowDescription As Boolean
        Get
            Return Me.PLHdescription.Visible
        End Get
        Set(value As Boolean)
            Me.PLHdescription.Visible = value
        End Set
    End Property

    Public Property MainCss As String
        Get
            Return ViewStateOrDefault(Of String)("MainCss", "btnswitchgroup small")
        End Get
        Set(value As String)
            ViewState("MainCss") = value
        End Set
    End Property

    Public Property DataName As String
        Get
            Return ViewStateOrDefault(Of String)("DataName", "")
        End Get
        Set(value As String)
            ViewState("DataName") = value
        End Set
    End Property

    Public Property DataRel As String
        Get
            Return ViewStateOrDefault(Of String)("DataRel", "")
        End Get
        Set(value As String)
            ViewState("DataRel") = value
        End Set
    End Property

    Public Property DataTable As String
        Get
            Return ViewStateOrDefault(Of String)("DataTable", "")
        End Get
        Set(value As String)
            ViewState("DataTable") = value
        End Set
    End Property
#Region "localizzazione"
    ''' <summary>
    ''' Internazionalizza le stringhe necessarie
    ''' </summary>
    ''' <param name="Resource">Resource: dalla pagina</param>
    ''' <param name="SetValue">Reimposta i valori On/Off interni allo switch</param>
    ''' <param name="SetDescription">Reimposta la descrizione. Se False viene nascosta.</param>
    ''' <remarks>
    ''' Stringhe necessarie per l'XML (Anteporre Id controllo):
    ''' 
    ''' Se SetValue = true
    ''' .On.Text - Testo visualizzato per l'ON del bottone
    ''' .On.ToolTip - eventuale tooltip
    ''' .Off.Text - Testo visualizzato per l'OFF del bottone
    ''' .Off.ToolTip - eventuale tooltip
    ''' 
    ''' SE SetDescription = true
    ''' .Description.Pre - testo visualizzato subito dopo il controllo e prima del valore selezionato
    ''' .Description.Post - testo visualizzato dopo .pre e valore
    ''' .Description.On - valore visualizzato se attivo
    ''' .Description.Off - Valore visualizzato se disattivo
    ''' 
    ''' Esempio:
    ''' 
    ''' (.On.Text/.Off.Text) .Description.Pre #Valore .Description.Post
    ''' 
    ''' Elenco completo
    ''' .On.Text
    ''' .On.ToolTip
    ''' .Off.Text
    ''' .Off.ToolTip
    ''' .Description.Pre
    ''' .Description.Post
    ''' .Description.On
    ''' .Description.Off
    ''' </remarks>
    Public Sub SetText(ByRef Resource As COL_BusinessLogic_v2.Localizzazione.ResourceManager, _
                       ByVal SetValue As Boolean, _
                       ByVal SetDescription As Boolean)
        If Not IsNothing(Resource) Then

            Dim MyId As String = Me.ID

            If SetValue Then
                Me.LinkOn_Text = Resource.getValue(MyId & ".On.Text")
                Me.LinkOn_ToolTip = Resource.getValue(MyId & ".On.ToolTip")
                Me.LinkOff_Text = Resource.getValue(MyId & ".Off.Text")
                Me.LinkOff_ToolTip = Resource.getValue(MyId & ".Off.ToolTip")
            End If

            If (SetDescription) Then
                Me.SetDescriptionText(Resource.getValue(MyId & ".Description.Pre"), Resource.getValue(MyId & ".Description.Post"))

                Me.SetDescriptionValue(Resource.getValue(MyId & ".Description.On"), Resource.getValue(MyId & ".Description.Off"))
            Else
                Me.ShowDescription = False
            End If

        End If
    End Sub
    'Default value per XML:
    '<item name=".On.Text">On</item>
    '<item name=".On.ToolTip">On</item>
    '<item name=".Off.Text">Off</item>
    '<item name=".Off.ToolTip">Off</item>
    '<item name=".Description.Pre">Value is </item>
    '<item name=".Description.Post"> (On/Off)</item>
    '<item name=".Description.On">on</item>
    '<item name=".Description.Off">off</item>
#End Region

    Private Sub SetCss()

        Dim CssDisabled As String = ""
        If Not Me.Enabled Then
            CssDisabled = " disabled"
        End If
        Me.LTmainContainer.Text = Me.LTmainContainer_template.Text.Replace("{css}", MainCss).Replace("{cssdisabled}", CssDisabled).Replace("{dataname}", DataName).Replace("{datarel}", DataRel).Replace("{datatable}", DataTable)


        ' {cssdisabled}" data-name="{dataname}" data-rel="{datarel}" data-table="{datatable}
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        SetCss()
    End Sub
End Class