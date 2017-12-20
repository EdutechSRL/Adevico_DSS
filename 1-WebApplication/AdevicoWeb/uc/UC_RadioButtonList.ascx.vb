Public Class UC_RadioButtonList
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' Evento generato alla selezione di un elemento, se AutoPostback è impostato a True
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Event ItemSelected(ByVal Value As String)

    ''' <summary>
    ''' Tag per elementi avanzati che verrà sostituito con il testo
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TextTAG As String = "{text}"

    ''' <summary>
    ''' Tag per elementi avanzati che verrà sostituito con la descrizione
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DescriptionTAG As String = "{description}"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Classe CSS applicata alla RadioButton (Span contenitore)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RBLcssClass As String
        Get
            Return Me.RBLitems.CssClass
        End Get
        Set(value As String)
            'Me.LITrblClass.Text = value
            Me.RBLitems.CssClass = value 'Attributes.Add("class", value)
        End Set
    End Property

    ''' <summary>
    ''' Classe CSS applicata di default ad ogni singolo elemento.
    ''' NOTA: va impostato PRIMA di aggiungere elementi senza stile.
    ''' </summary>
    ''' <value>
    ''' La classe CSS.
    ''' SE non impostata verrà utilizzato il valore di default nel literal relativo
    ''' SE impostata a "" gli elementi della radiobutton NON AVRANNO un loro contenitore (SPAN che li racchiude)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RBLcssItemClass As String
        Get
            Return (Me.LITrblItemClass.Text)
        End Get
        Set(value As String)
            Me.LITrblItemClass.Text = value
        End Set
    End Property
    ''' <summary>
    ''' Indica se fare o meno il postback alla selezione di un elemento
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoPostBack As Boolean
        Get
            Return Me.RBLitems.AutoPostBack
        End Get
        Set(value As Boolean)
            Me.RBLitems.AutoPostBack = value
        End Set
    End Property

    Public Property ItemLayout As String
        Get
            Return Me.LITrblLayout.Text
        End Get
        Set(value As String)
            Me.LITrblLayout.Text = value
        End Set
    End Property

    Private Sub RBLitems_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLitems.SelectedIndexChanged
        RaiseEvent ItemSelected(Me.RBLitems.SelectedValue)
    End Sub

    ''' <summary>
    ''' Aggiunta semplice di un ITEM (testo = valore)
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks></remarks>
    Public Sub AddItem(ByVal Text As String, Optional ByVal Selected As Boolean = False)
        Dim itm As New ListItem(Text)

        If Not String.IsNullOrEmpty(RBLcssItemClass) Then
            itm.Attributes.Add("class", RBLcssItemClass)
        End If
        itm.Selected = Selected
        Me.RBLitems.Items.Add(itm)
    End Sub

    ''' <summary>
    ''' Aggiunta di un elemento con testo e valore
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub AddItem(ByVal Text As String, ByVal Value As String, Optional ByVal Selected As Boolean = False)
        Dim itm As New ListItem(Text, Value)

        If Not String.IsNullOrEmpty(RBLcssItemClass) Then
            itm.Attributes.Add("class", RBLcssItemClass)
        End If
        itm.Selected = Selected
        Me.RBLitems.Items.Add(itm)
    End Sub

    ''' <summary>
    ''' Aggiunta di un elemento con testo, valore e classe CSS
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="Value"></param>
    ''' <param name="ItemCssClass">SE vuoto prende valore di DEFAULT</param>
    ''' <remarks></remarks>
    Public Sub AddItem(ByVal Text As String, ByVal Value As String, ByVal ItemCssClass As String, Optional ByVal Selected As Boolean = False)
        Dim itm As New ListItem(Text, Value)

        If Not String.IsNullOrEmpty(ItemCssClass) Then
            itm.Attributes.Add("class", ItemCssClass)
        ElseIf Not String.IsNullOrEmpty(RBLcssItemClass) Then
            itm.Attributes.Add("class", RBLcssItemClass)
        End If
        itm.Selected = Selected
        Me.RBLitems.Items.Add(itm)
    End Sub

    ''' <summary>
    ''' Aggiunta di un elemento con testo, valore e classe CSS
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="Value"></param>
    ''' <param name="ItemCssClass">SE vuoto prende valore di DEFAULT</param>
    ''' <remarks></remarks>
    Public Sub AddItemAdvance(ByVal Text As String, ByVal Description As String, ByVal Value As String, ByVal ItemCssClass As String, Optional ByVal Selected As Boolean = False)
        Me.AddItem(GetAdvancedItemText(Text, Description), Value, ItemCssClass, Selected)
    End Sub

    ''' <summary>
    ''' Aggiunge un elemento, partendo dal suo valore e recuperando il testo e la descrizione dall'internazionalizzazione della pagina
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <param name="Resource"></param>
    ''' <remarks></remarks>
    Public Sub AddItemAdvance(ByVal Value As String, ByRef Resource As COL_BusinessLogic_v2.Localizzazione.ResourceManager, Optional ByVal ItemCssClass As String = "", Optional ByVal Selected As Boolean = False)

        If IsNothing(Resource) Then
            Me.AddItem(Value)
        Else
            Me.AddItemAdvance(Resource.getValue(Me.ID & "." & Value & ".text"), Resource.getValue(Me.ID & "." & Value & ".description"), Value, ItemCssClass, Selected)

        End If

    End Sub



    ''' <summary>
    ''' Recupera il testo formattato in HTML per un elemento "avanzato"
    ''' </summary>
    ''' <param name="Text">Testo</param>
    ''' <param name="Description">Descrizione</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAdvancedItemText(ByVal Text As String, ByVal Description As String) As String
        Return Me.LITrblLayout.Text.Replace(TextTAG, Text).Replace(DescriptionTAG, Description)
    End Function

    ''' <summary>
    ''' Inizializzazione RBL. OPZIONALE.
    ''' Permette di impostare subito le classi necessarie.
    ''' </summary>
    ''' <param name="CssClass"></param>
    ''' <param name="ItemCssClass"></param>
    ''' <remarks></remarks>
    Public Sub SetCss(Optional ByVal CssClass As String = "", Optional ByVal ItemCssClass As String = "")

        If Not String.IsNullOrEmpty(CssClass) Then
            RBLcssClass = CssClass
        End If

        If Not String.IsNullOrEmpty(ItemCssClass) Then
            RBLcssItemClass = ItemCssClass
        End If
    End Sub

    ''' <summary>
    ''' Imposta direttamente gli elementi della radiobutton con gli elementi passati:
    ''' Nessuna modifica agli elementi, che manterranno eventuali attributi impostati nella lista.
    ''' </summary>
    ''' <param name="Items"></param>
    ''' <remarks></remarks>
    Public Sub SetRBL(ByVal Items As IList(Of ListItem))
        If Not IsNothing(Items) Then
            For Each li As ListItem In Items
                Me.RBLitems.Items.Add(li)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Imposta la radiobutton con i parametri di un ENUM
    ''' </summary>
    ''' <typeparam name="T">Tipo dell'enum</typeparam>
    ''' <param name="Resource">Per l'internazionalizzazione (vedi REMARKS)</param>
    ''' <param name="SelectedItem">Elemento selezionato (se Nothing non seleziona nulla)</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Nel file XML di internazionalizzazione sarà necessario avere per ogni elemento dell'enum una serie di chiavi composte da:
    ''' NomeDellEnum.Valore
    ''' ESEMPIO:
    ''' public enum lm.Comol.Namespace.EnumTest
    '''        Valore1
    ''' end enum
    ''' 
    ''' l'XML DOVRA' contenere EnumTest.Valore1
    '''</remarks>
    Public Function SetRBLByEnum(Of T)(ByVal Resource As COL_BusinessLogic_v2.Localizzazione.ResourceManager, Optional ByVal SelectedItem As T = Nothing) As Boolean

        Dim EnumName As String = ""
        If Not IsNothing(Resource) Then
            EnumName = GetType(T).ToString()
            Dim DotLastIndex As Integer = EnumName.LastIndexOf(".")
            EnumName = EnumName.Remove(0, DotLastIndex + 1)
        End If

        Dim LstItm As Array
        Try
            LstItm = [Enum].GetValues(GetType(T))
        Catch ex As Exception
            Return False
        End Try

        For i As Integer = 0 To LstItm.Length - 1

            Dim li As New ListItem()
            If IsNothing(Resource) Then
                li.Text = LstItm(i).ToString()
            Else
                li.Text = Resource.getValue(EnumName & LstItm(i).ToString())
            End If

            li.Value = LstItm(i).ToString()

            If Not String.IsNullOrEmpty(RBLcssItemClass) Then
                li.Attributes.Add("class", RBLcssItemClass)
            End If
            Me.RBLitems.Items.Add(li)
        Next

        Return True

    End Function

    ''' <summary>
    ''' Imposta gli elementi della radiobutton recuperandoli dall'enum indicato.
    ''' </summary>
    ''' <typeparam name="T">Tipo dell'enum</typeparam>
    ''' <param name="Resource">File di risorse da cui recuperare il testo, nel formato "NomeEnum.Valore"</param>
    ''' <param name="CssClass">Eventuale stile da applicare agli ELEMENTI</param>
    ''' <param name="SelectedItem">Eventuale elemento preselezionato. Se Null nessun elemento selezioanto.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetRBLByEnum(Of T)(ByVal Resource As COL_BusinessLogic_v2.Localizzazione.ResourceManager, ByVal CssClass As String, Optional ByVal SelectedItem As T = Nothing) As Boolean

        'If IsNothing(Resource) Then
        '    Return False
        'End If

        Dim LstItm As Array
        Try
            LstItm = [Enum].GetValues(GetType(T))
        Catch ex As Exception
            Return False
        End Try

        Dim EnumName As String = GetType(T).ToString()
        Dim DotLastIndex As Integer = EnumName.LastIndexOf(".")
        EnumName = EnumName.Remove(0, DotLastIndex + 1)

        For i As Integer = 0 To LstItm.Length - 1

            Dim li As New ListItem()
            If IsNothing(Resource) Then
                li.Text = LstItm(i).ToString()
            Else
                li.Text = Resource.getValue(EnumName & LstItm(i).ToString())
            End If

            If LstItm(i) = SelectedItem Then
                li.Selected = True
            End If

            li.Value = LstItm(i).ToString()
            li.Attributes.Add("class", CssClass)

            Me.RBLitems.Items.Add(li)
        Next

        Return True

    End Function

    ''' <summary>
    ''' Recupera il valore con il tipo di Enum indicato
    ''' </summary>
    ''' <typeparam name="T">Tipo dell'enum</typeparam>
    ''' <returns></returns>
    ''' <remarks>In caso di ERRORI viene restituito NOTHING!</remarks>
    Public Function GetEnumValue(Of T)() As T

        Dim value As T = Nothing

        Try
            'Potrebbe non essere ancora inizializzato quando richiamato...
            value = DirectCast([Enum].Parse(GetType(T), Me.RBLitems.SelectedValue), T)
        Catch ex As Exception

        End Try

        Return value
    End Function

    Public ReadOnly Property SelectedValue As String
        Get
            Return Me.RBLitems.SelectedValue
        End Get
    End Property

    ''' <summary>
    ''' Da quando impostato gli elementi non avranno più uno stile, nè uno span contenitore, salvo quelli impostati con uno stile definito.
    ''' </summary>
    ''' <remarks>In pratica setta RBLcssItemClass a ""</remarks>
    Public Sub RemoveItemSpan()
        Me.RBLcssItemClass = ""
    End Sub

    ''' <summary>
    ''' Rimuove TUTTI gli attributi degli elementi della radiobuttonlist
    ''' </summary>
    ''' <remarks>Da usare nel PRERENDER in caso di problemi, MA VALE PER TUTTI!!!</remarks>
    Public Sub ItemsAttributesRemoveAll()
        For Each item As ListItem In Me.RBLitems.Items
            item.Attributes.Clear()
        Next
    End Sub

    ''' <summary>
    ''' Riassegna un css specifico ad TUTTI gli elementi della readiobuttonlist
    ''' </summary>
    ''' <param name="NewCssClass"></param>
    ''' <remarks>Da usare nel PRERENDER in caso di problemi, MA VALE PER TUTTI!!!</remarks>
    Public Sub ItemAttributesAddCssClass(ByVal NewCssClass As String)
        For Each item As ListItem In Me.RBLitems.Items
            item.Attributes.Add("class", NewCssClass)
        Next
    End Sub

    ''' <summary>
    ''' Elimina TUTTI gli attributi di TUTTI gli elementi della radiobuttonlist e ne riassegna lo stile
    ''' </summary>
    ''' <param name="NewCssClass"></param>
    ''' <remarks>Da usare nel PRERENDER in caso di problemi, MA VALE PER TUTTI!!!</remarks>
    Public Sub ItemAttributesResetCssClass(ByVal NewCssClass As String)
        For Each item As ListItem In Me.RBLitems.Items
            item.Attributes.Clear()
            item.Attributes.Add("class", NewCssClass)
        Next
    End Sub

End Class