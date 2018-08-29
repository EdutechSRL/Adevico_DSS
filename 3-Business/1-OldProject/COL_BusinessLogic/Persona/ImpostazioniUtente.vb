Imports System
'Imports System.IO
Imports System.Xml
Imports System.Text
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona


Namespace CL_persona
    <Serializable>
    Public Class COL_ImpostazioniUtente

#Region "Private Property"

        Private n_showLogo As Boolean
        Private n_TPCM_ID_iscritto As Integer
        Private n_record_iscritto As Integer
        Private n_ORGN_ID_iscritto As Integer
        Private n_AA_iscritto As Integer
        Private n_PRDO_ID_iscritto As Integer
        Private n_TPCS_ID_iscritto As Integer
        Private n_Visualizza_iscritto As Integer

        Private n_showConferma As Boolean
        Private n_TPCM_ID_Ricerca As Integer
        Private n_record_Ricerca As Integer
        Private n_ORGN_ID_Ricerca As Integer
        Private n_AA_Ricerca As Integer
        Private n_PRDO_ID_Ricerca As Integer
        Private n_TPCS_ID_Ricerca As Integer
        Private n_Visualizza_Ricerca As Integer
        Private n_StatoComunita As Main.FiltroStatoComunita

        Private n_showNews As Boolean
        Private n_showNewsAllPage As Boolean
        Private n_velocitaNews As Integer
        Private n_OrdinamentoPost As Integer
        Private n_OrdinamentoDiario As Integer
        Private n_ApriEditorDiario As Boolean
        Private n_name As String
        Private n_Errore As FileError
        Private n_Directory As String
#End Region

#Region "Public Property"
        Public Enum FileError
            none = 0
            exsist = 1
            zeroByte = 3
            dirNotFound = 4
            fileNotFound = 5
            fileNotCreated = 6
        End Enum
        Public Property Nome() As String
            Get
                Nome = n_name
            End Get
            Set(ByVal Value As String)
                n_name = Value
            End Set
        End Property
        Public Property Directory() As String
            Get
                Directory = n_Directory
            End Get
            Set(ByVal Value As String)
                n_Directory = Value
            End Set
        End Property

        Public Property ShowLogo() As Boolean
            Get
                ShowLogo = n_showLogo
            End Get
            Set(ByVal Value As Boolean)
                n_showLogo = Value
            End Set
        End Property
        Public Property TipoComunita_Iscritto() As Integer
            Get
                TipoComunita_Iscritto = n_TPCM_ID_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_TPCM_ID_iscritto = Value
            End Set
        End Property
        Public Property Nrecord_Iscritto() As Integer
            Get
                Nrecord_Iscritto = n_record_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_record_iscritto = Value
            End Set
        End Property
        Public Property Organizzazione_Iscritto() As Integer
            Get
                Organizzazione_Iscritto = n_ORGN_ID_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_ORGN_ID_iscritto = Value
            End Set
        End Property
        Public Property TipoCDS_Iscritto() As Integer
            Get
                TipoCDS_Iscritto = n_TPCS_ID_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_TPCS_ID_iscritto = Value
            End Set
        End Property


        Public Property AA_Iscritto() As Integer
            Get
                AA_Iscritto = n_AA_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_AA_iscritto = Value
            End Set
        End Property
        Public Property Periodo_Iscritto() As Integer
            Get
                Periodo_Iscritto = n_PRDO_ID_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_PRDO_ID_iscritto = Value
            End Set
        End Property
        Public Property Visualizza_Iscritto() As Integer
            Get
                Visualizza_Iscritto = n_Visualizza_iscritto
            End Get
            Set(ByVal Value As Integer)
                n_Visualizza_iscritto = Value
            End Set
        End Property

        Public Property ShowConferma() As Boolean
            Get
                ShowConferma = n_showConferma
            End Get
            Set(ByVal Value As Boolean)
                n_showConferma = Value
            End Set
        End Property

        Public Property TipoComunita_Ricerca() As Integer
            Get
                TipoComunita_Ricerca = n_TPCM_ID_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_TPCM_ID_Ricerca = Value
            End Set
        End Property
        Public Property Nrecord_Ricerca() As Integer
            Get
                Nrecord_Ricerca = n_record_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_record_Ricerca = Value
            End Set
        End Property
        Public Property Organizzazione_Ricerca() As Integer
            Get
                Organizzazione_Ricerca = n_ORGN_ID_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_ORGN_ID_Ricerca = Value
            End Set
        End Property
        Public Property TipoCDS_Ricerca() As Integer
            Get
                TipoCDS_Ricerca = Me.n_TPCS_ID_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_TPCS_ID_Ricerca = Value
            End Set
        End Property
        Public Property AA_Ricerca() As Integer
            Get
                AA_Ricerca = n_AA_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_AA_Ricerca = Value
            End Set
        End Property
        Public Property Periodo_Ricerca() As Integer
            Get
                Periodo_Ricerca = n_PRDO_ID_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_PRDO_ID_Ricerca = Value
            End Set
        End Property
        Public Property Visualizza_Ricerca() As Integer
            Get
                Visualizza_Ricerca = n_Visualizza_Ricerca
            End Get
            Set(ByVal Value As Integer)
                n_Visualizza_Ricerca = Value
            End Set
        End Property

        Public Property showNews() As Boolean
            Get
                showNews = n_showNews
            End Get
            Set(ByVal Value As Boolean)
                n_showNews = Value
            End Set
        End Property
        Public Property showNewsAllPage() As Boolean
            Get
                showNewsAllPage = n_showNewsAllPage
            End Get
            Set(ByVal Value As Boolean)
                n_showNewsAllPage = Value
            End Set
        End Property
        Public Property velocitaNews() As Integer
            Get
                velocitaNews = n_velocitaNews
            End Get
            Set(ByVal Value As Integer)
                n_velocitaNews = Value
            End Set
        End Property

        Public Property OrdinamentoPost() As Integer
            Get
                OrdinamentoPost = n_OrdinamentoPost
            End Get
            Set(ByVal Value As Integer)
                n_OrdinamentoPost = Value
            End Set
        End Property
        Public Property OrdinamentoDiario() As Integer
            Get
                OrdinamentoDiario = n_OrdinamentoDiario
            End Get
            Set(ByVal Value As Integer)
                n_OrdinamentoDiario = Value
            End Set
        End Property
        Public Property ApriEditorDiario() As Boolean
            Get
                ApriEditorDiario = n_ApriEditorDiario
            End Get
            Set(ByVal Value As Boolean)
                n_ApriEditorDiario = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As FileError
            Get
                Errore = n_Errore
            End Get
        End Property

        Public Property StatoComunita_Iscritto() As Main.FiltroStatoComunita
            Get
                Organizzazione_Iscritto = n_StatoComunita
            End Get
            Set(ByVal Value As Main.FiltroStatoComunita)
                n_StatoComunita = Value
            End Set
        End Property
#End Region

        Sub New()
            Me.n_Errore = FileError.none
            Me.n_Visualizza_iscritto = Main.ElencoRecord.Normale
            Me.n_StatoComunita = Main.FiltroStatoComunita.Attiva
        End Sub
        Sub New(ByVal nome As String, ByVal dir As String)
            Me.n_name = nome
            Me.n_Directory = dir
            Me.n_Errore = FileError.none
            Me.n_Visualizza_iscritto = Main.ElencoRecord.Normale
            Me.n_StatoComunita = Main.FiltroStatoComunita.Attiva
        End Sub

        Public Function Exist() As Boolean
            Try
                If lm.Comol.Core.File.Exists.Directory(Me.n_Directory) Then
                    If lm.Comol.Core.File.Exists.File(Me.n_Directory & Me.n_name) Then
                        Me.n_Errore = FileError.none
                        Return True
                    Else
                        Me.n_Errore = FileError.fileNotFound
                        Return False
                    End If
                    Return False
                Else
                    lm.Comol.Core.File.Create.Directory(Me.n_Directory)
                    Me.n_Errore = FileError.dirNotFound
                    Return False
                End If
            Catch ex As Exception
                Me.n_Errore = FileError.fileNotFound
                Return False
            End Try
            Return False
        End Function

        Public Function RecuperaImpostazioni() As Boolean
            Dim oDataset As New DataSet

            Try
                If Me.Exist() = False Then
                    Return False
                End If

                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)

                If Not oDataset.Tables(0).Columns.Contains("OrdinamentoPost") Then
                    oDataset.Tables(0).Columns.Add("OrdinamentoPost", System.Type.GetType("System.Int32"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("StatoComunita") Then
                    oDataset.Tables(0).Columns.Add("StatoComunita", System.Type.GetType("System.Int32"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("OrdinamentoDiario") Then
                    oDataset.Tables(0).Columns.Add("OrdinamentoDiario", System.Type.GetType("System.Int32"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("ApriEditorDiario") Then
                    oDataset.Tables(0).Columns.Add("ApriEditorDiario", System.Type.GetType("System.Boolean"))
                End If
                Dim totale, i As Integer
                totale = oDataset.Tables(0).Rows.Count


                Dim oRow As DataRow
                If totale > 0 Then
                    oRow = oDataset.Tables(0).Rows(i)
                    If IsDBNull(oRow.Item("showLogo")) Then
                        Me.n_showLogo = True
                    Else
                        Me.n_showLogo = oRow.Item("showLogo")
                    End If

                    If IsDBNull(oRow.Item("TPCM_ID_iscritto")) Then
                        Me.n_TPCM_ID_iscritto = -1
                    Else
                        Me.n_TPCM_ID_iscritto = oRow.Item("TPCM_ID_iscritto")
                    End If
                    If IsDBNull(oRow.Item("record_iscritto")) Then
                        Me.n_record_iscritto = 0
                    Else
                        Me.n_record_iscritto = oRow.Item("record_iscritto")
                    End If
                    If IsDBNull(oRow.Item("ORGN_ID_iscritto")) Then
                        Me.n_ORGN_ID_iscritto = -1
                    Else
                        Me.n_ORGN_ID_iscritto = oRow.Item("ORGN_ID_iscritto")
                    End If
                    If IsDBNull(oRow.Item("AA_iscritto")) Then
                        Me.n_AA_iscritto = -2
                    Else
                        Me.n_AA_iscritto = oRow.Item("AA_iscritto")
                    End If
                    If IsDBNull(oRow.Item("PRDO_ID_iscritto")) Then
                        Me.n_PRDO_ID_iscritto = -1
                    Else
                        Me.n_PRDO_ID_iscritto = oRow.Item("PRDO_ID_iscritto")
                    End If
                    If IsDBNull(oRow.Item("TPCS_ID_iscritto")) Then
                        Me.n_TPCS_ID_iscritto = -1
                    Else
                        Me.n_TPCS_ID_iscritto = oRow.Item("TPCS_ID_iscritto")
                    End If
                    If IsDBNull(oRow.Item("Visualizza_iscritto")) Then
                        Me.n_Visualizza_iscritto = 3
                    Else
                        Me.n_Visualizza_iscritto = oRow.Item("Visualizza_iscritto")
                    End If


                    If IsDBNull(oRow.Item("showConferma")) Then
                        Me.n_showConferma = False
                    Else
                        Me.n_showConferma = oRow.Item("showConferma")
                    End If

                    If IsDBNull(oRow.Item("TPCM_ID_Ricerca")) Then
                        Me.n_TPCM_ID_Ricerca = -1
                    Else
                        Me.n_TPCM_ID_Ricerca = oRow.Item("TPCM_ID_Ricerca")
                    End If
                    If IsDBNull(oRow.Item("record_Ricerca")) Then
                        Me.n_record_Ricerca = 0
                    Else
                        Me.n_record_Ricerca = oRow.Item("record_Ricerca")
                    End If
                    If IsDBNull(oRow.Item("ORGN_ID_Ricerca")) Then
                        Me.n_ORGN_ID_Ricerca = -1
                    Else
                        Me.n_ORGN_ID_Ricerca = oRow.Item("ORGN_ID_Ricerca")
                    End If
                    If IsDBNull(oRow.Item("AA_Ricerca")) Then
                        Me.n_AA_Ricerca = -2
                    Else
                        Me.n_AA_Ricerca = oRow.Item("AA_Ricerca")
                    End If
                    If IsDBNull(oRow.Item("PRDO_ID_Ricerca")) Then
                        Me.n_PRDO_ID_Ricerca = -1
                    Else
                        Me.n_PRDO_ID_Ricerca = oRow.Item("PRDO_ID_Ricerca")
                    End If
                    If IsDBNull(oRow.Item("TPCS_ID_Ricerca")) Then
                        Me.n_TPCS_ID_Ricerca = -1
                    Else
                        Me.n_TPCS_ID_Ricerca = oRow.Item("TPCS_ID_Ricerca")
                    End If
                    If IsDBNull(oRow.Item("Visualizza_Ricerca")) Then
                        Me.n_Visualizza_Ricerca = 3
                    Else
                        Me.n_Visualizza_Ricerca = oRow.Item("Visualizza_Ricerca")
                    End If

                    If IsDBNull(oRow.Item("showNews")) Then
                        Me.n_showNews = True
                    Else
                        Me.n_showNews = oRow.Item("showNews")
                    End If
                    If IsDBNull(oRow.Item("showNewsAllPage")) Then
                        Me.n_showNewsAllPage = False
                    Else
                        Me.n_showNewsAllPage = oRow.Item("showNewsAllPage")
                    End If
                    If IsDBNull(oRow.Item("velocitaNews")) Then
                        Me.n_velocitaNews = 0
                    Else
                        Me.n_velocitaNews = oRow.Item("velocitaNews")
                    End If
                    If IsDBNull(oRow.Item("OrdinamentoPost")) Then
                        Me.n_OrdinamentoPost = 1
                    Else
                        Me.n_OrdinamentoPost = oRow.Item("OrdinamentoPost")
                    End If
                    If IsDBNull(oRow.Item("OrdinamentoDiario")) Then
                        Me.n_OrdinamentoDiario = 0
                    Else
                        Me.n_OrdinamentoDiario = oRow.Item("OrdinamentoDiario")
                    End If
                    If IsDBNull(oRow.Item("ApriEditorDiario")) Then
                        Me.n_ApriEditorDiario = True
                    Else
                        Me.n_ApriEditorDiario = oRow.Item("ApriEditorDiario")
                    End If
                    If IsDBNull(oRow.Item("StatoComunita")) Then
                        Me.n_StatoComunita = Main.FiltroStatoComunita.Attiva
                    Else
                        Me.n_StatoComunita = oRow.Item("StatoComunita")
                    End If


                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
        Public Function SalvaImpostazioni() As Boolean
            Dim oDataset As New DataSet

            Try
                If Me.Exist() = False Then
                    Me.RigeneraFileXML()
                End If
                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)

                If Not oDataset.Tables(0).Columns.Contains("OrdinamentoPost") Then
                    oDataset.Tables(0).Columns.Add("OrdinamentoPost", System.Type.GetType("System.Int32"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("StatoComunita") Then
                    oDataset.Tables(0).Columns.Add("StatoComunita", System.Type.GetType("System.Int32"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("OrdinamentoDiario") Then
                    oDataset.Tables(0).Columns.Add("OrdinamentoDiario", System.Type.GetType("System.Int32"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("ApriEditorDiario") Then
                    oDataset.Tables(0).Columns.Add("ApriEditorDiario", System.Type.GetType("System.Boolean"))
                End If
                Dim totale, i As Integer
                Dim oRow As DataRow

                totale = oDataset.Tables(0).Rows.Count
                If totale > 0 Then
                    oRow = oDataset.Tables(0).Rows(i)
                Else
                    oRow = oDataset.Tables(0).NewRow
                End If

                oRow.Item("showLogo") = Me.n_showLogo
                oRow.Item("TPCM_ID_iscritto") = Me.n_TPCM_ID_iscritto
                oRow.Item("record_iscritto") = Me.n_record_iscritto
                oRow.Item("ORGN_ID_iscritto") = Me.n_ORGN_ID_iscritto
                oRow.Item("AA_iscritto") = Me.n_AA_iscritto
                oRow.Item("PRDO_ID_iscritto") = Me.n_PRDO_ID_iscritto
                oRow.Item("TPCS_ID_iscritto") = Me.n_TPCS_ID_iscritto
                oRow.Item("Visualizza_iscritto") = Me.n_Visualizza_iscritto

                oRow.Item("showConferma") = Me.n_showConferma

                oRow.Item("TPCM_ID_Ricerca") = Me.n_TPCM_ID_Ricerca
                oRow.Item("record_Ricerca") = Me.n_record_Ricerca
                oRow.Item("ORGN_ID_Ricerca") = Me.n_ORGN_ID_Ricerca
                oRow.Item("AA_Ricerca") = Me.n_AA_Ricerca
                oRow.Item("PRDO_ID_Ricerca") = Me.n_PRDO_ID_Ricerca
                oRow.Item("TPCS_ID_Ricerca") = Me.n_TPCS_ID_Ricerca
                oRow.Item("Visualizza_Ricerca") = Me.n_Visualizza_Ricerca
                oRow.Item("showNews") = Me.n_showNews
                oRow.Item("showNewsAllPage") = Me.n_showNewsAllPage
                oRow.Item("velocitaNews") = Me.n_velocitaNews
                oRow.Item("OrdinamentoPost") = Me.n_OrdinamentoPost
                oRow.Item("StatoComunita") = CType(Me.n_StatoComunita, Main.FiltroStatoComunita)
                oRow.Item("OrdinamentoDiario") = Me.n_OrdinamentoDiario
                oRow.Item("ApriEditorDiario") = Me.n_ApriEditorDiario
                If totale = 0 Then
                    oDataset.Tables(0).Rows.Add(oRow)
                End If
                oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

        Private Sub RigeneraFileXML()
            Dim oDataset As New DataSet
            Try
                oDataset = Me.RigeneraDataset
                oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                Me.n_Errore = FileError.none
            Catch ex As Exception
                Me.n_Errore = FileError.fileNotCreated
            End Try
        End Sub
        Private Function RigeneraDataset() As DataSet
            Dim oDataset As New DataSet

            Try
                oDataset.Tables.Add("impostazioni")
                oDataset.Tables(0).Columns.Add("showLogo", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("TPCM_ID_iscritto", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("record_iscritto", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("ORGN_ID_iscritto", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("AA_iscritto", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("PRDO_ID_iscritto", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("TPCS_ID_iscritto", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("Visualizza_iscritto", System.Type.GetType("System.Int32"))

                oDataset.Tables(0).Columns.Add("showConferma", System.Type.GetType("System.Boolean"))

                oDataset.Tables(0).Columns.Add("TPCM_ID_Ricerca", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("record_Ricerca", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("ORGN_ID_Ricerca", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("AA_Ricerca", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("PRDO_ID_Ricerca", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("TPCS_ID_Ricerca", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("Visualizza_Ricerca", System.Type.GetType("System.Int32"))

                oDataset.Tables(0).Columns.Add("showNews", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("showNewsAllPage", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("velocitaNews", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("OrdinamentoPost", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("StatoComunita", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("OrdinamentoDiario", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("ApriEditorDiario", System.Type.GetType("System.Boolean"))
            Catch ex As Exception

            End Try
            Return oDataset
        End Function

    End Class
End Namespace









