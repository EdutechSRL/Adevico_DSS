﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated. 
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class GenericMailSender

    '''<summary>
    '''LBtitoloServizio control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBtitoloServizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''WZRmail control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents WZRmail As Global.System.Web.UI.WebControls.Wizard

    '''<summary>
    '''WZScommunity control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents WZScommunity As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''CTRLsearchCommunity control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLsearchCommunity As Global.Comunita_OnLine.UC_SearchCommunityByService

    '''<summary>
    '''WZSaddresses control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents WZSaddresses As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''MLVmenu control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents MLVmenu As Global.System.Web.UI.WebControls.MultiView

    '''<summary>
    '''MLVbacheca control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents MLVbacheca As Global.System.Web.UI.WebControls.MultiView

    '''<summary>
    '''Master property.
    '''</summary>
    '''<remarks>
    '''Auto-generated property.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class