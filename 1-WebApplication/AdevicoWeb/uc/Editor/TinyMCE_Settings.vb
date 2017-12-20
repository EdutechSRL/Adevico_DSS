
Public Class TinySettings
    Public ReadOnly Property GetLiteral As String
        Get
            Return ""
        End Get
    End Property

    Private CustonDialogScript As String
    Private AdvancedTool As String
    Private ShowAddDocument As String
    Private EditorEnabled As Boolean

    Private _FontSizes As String
    ''' <summary>
    ''' La stringa con le dimensioni dei caratteri
    ''' </summary>
    ''' <value>
    ''' Il valore è un elenco di:
    ''' #Disp=#Size
    ''' oppure
    ''' #Size
    ''' Separati da ;
    ''' In cui #Disp è il valore visualizzato, mentre #Size la dimensione in pixel.
    ''' Esempi:
    ''' "9px,11px,15px,19px,21px,23px,25px" (Questo funziona anche con la virgola...)
    ''' "1 (9px)=9px;2 (11px)=11px;3 (15px)=15px;4 (19px)=19px;5 (21px)=21px;6 (23px)=23px;7 (25px)=25px" (Questo DEVE andare con il ;)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FontSizes As String
        Get
            If (_FontSizes = "") Then
                '_FontSize = "9px,11px,15px,19px,21px,23px,25px"
                'Nell'attuale ci sono a valori: 2,3,4,5 che pero' danno TUTTI 48px...
                _FontSizes = "1 (9px)=9px;2 (11px)=11px;3 (15px)=15px;4 (19px)=19px;5 (21px)=21px;6 (23px)=23px;7 (25px)=25px"

                'Alcuni esempi:
                ' "11px,12px,13px"
                ' "piccolo = 9px; medio = 15px; grande = 55px; tanto tanto tanto grande = 250px"
            End If
            Return _FontSizes
        End Get
        Set(ByVal value As String)
            _FontSizes = value
        End Set
    End Property
    Public Sub SetFontSizes(ByVal FontSizes As String)
        _FontSizes = ""
        Dim i As Integer = 1
        For Each Size As String In FontSizes.Split(",")
            _FontSizes += i.ToString() + " (" + Size + "px)=" + Size + "px;"
        Next
    End Sub

    Private _FontNames As String = ""
    ''' <summary>
    ''' La stringa con i nomi dei font per l'editor
    ''' </summary>
    ''' <value>
    ''' Il valore dev'essere un elenco nel formato:
    ''' "#Name=#FontName;"
    ''' Dove #Name è il nome visualizzato e #FontName quello del font
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FontNames As String
        Get
            If _FontNames = "" Then
                'Attualmente in COMOL, ma è possibile aggiungerli/Configurarli a piacere...
                _FontNames = "Verdana=verdana;Arial=arial;Tahoma=tahoma;Courier New=courier new"
            End If

            Return _FontNames
        End Get
        Set(ByVal value As String)
            _FontNames = value
        End Set
    End Property

    Public Sub SetFontNames(ByVal FontNames As String)
        _FontNames = ""
        For Each Name As String In FontNames.Split(",")
            _FontNames += Name + "=" + Name.ToLower() + ";"
        Next
    End Sub

    Private _ImagePats As String
    Public Property ImagePath As String
        Get
            Return _ImagePats
        End Get
        Set(ByVal value As String)
            _ImagePats = value
        End Set
    End Property


    Private _ShowAddImage As Boolean
    Public Property ShowAddImage As Boolean
        Get
            Return _ShowAddImage
        End Get
        Set(ByVal value As Boolean)
            _ShowAddImage = value
        End Set
    End Property

    Private _ShowAddWiki As Boolean
    Public Property ShowAddWiki As Boolean
        Get
            Return _ShowAddWiki
        End Get
        Set(ByVal value As Boolean)
            _ShowAddWiki = value
        End Set
    End Property

    Private _ShowAddEmoticon As Boolean
    Public Property ShowAddEmoticon As Boolean
        Get
            Return _ShowAddEmoticon
        End Get
        Set(ByVal value As Boolean)
            _ShowAddEmoticon = value
        End Set
    End Property

    Private _ShowAddSmartTag As Boolean
    Public Property ShowAddSmartTag As Boolean
        Get
            Return _ShowAddSmartTag
        End Get
        Set(ByVal value As Boolean)
            _ShowAddSmartTag = value
        End Set
    End Property

    Public Shared Button1List As String = "cut,copy,paste,pasteword,|,undo,redo,|,link,unlink,|,charmap,paragraph,p,hr,tablecontrols"
    Public Shared Button2List As String = "forecolor,backcolor,fontselect,fontsizeselect,bold,italic,underline,|,justifyleft,justifycenter,justifyright,justifyfull,|,numlist,bullist,|,outdent,indent,|,help,|"

    Public Function GetButton1List() As String
        Dim Btn_Strin As String = ""
        Btn_Strin += "var TinyMceButtonList_1 = '" + Button1List
        If Me.ShowAddImage Then
            Btn_Strin += "image,"
        End If

        If Me.ShowAddWiki Then
            Btn_Strin += "Wiki,"
        End If

        If Me.ShowAddEmoticon Then
            Btn_Strin += "emotions,"
        End If

        Return Btn_Strin
    End Function

    Public Function GetButton2List() As String
        Dim Btn_Strin As String = ""
        Btn_Strin += "var TinyMceButtonList_2 =  '" + Button2List
        Btn_Strin += ",LATEX"
        Btn_Strin += ",SLIDESHARE"
        Btn_Strin += ",YOUTUBE"
        Btn_Strin += ",media'" + vbCrLf
        Return Btn_Strin
    End Function
End Class
