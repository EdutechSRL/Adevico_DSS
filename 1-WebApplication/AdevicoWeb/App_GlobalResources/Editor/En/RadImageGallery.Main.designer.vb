'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace Resources.RadImageGallery
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option or rebuild the Visual Studio project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "15.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class Main
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Resources.RadImageGallery.Main", Global.System.Reflection.[Assembly].Load("App_GlobalResources"))
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Close.
        '''</summary>
        Friend Shared ReadOnly Property CloseButtonText() As String
            Get
                Return ResourceManager.GetString("CloseButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Enter FullScreen.
        '''</summary>
        Friend Shared ReadOnly Property EnterFullScreenButtonText() As String
            Get
                Return ResourceManager.GetString("EnterFullScreenButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Exit FullScreen.
        '''</summary>
        Friend Shared ReadOnly Property ExitFullScreenButtonText() As String
            Get
                Return ResourceManager.GetString("ExitFullScreenButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Hide Thumbnails.
        '''</summary>
        Friend Shared ReadOnly Property HideThumbnailsButtonText() As String
            Get
                Return ResourceManager.GetString("HideThumbnailsButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Item {0} of {1}.
        '''</summary>
        Friend Shared ReadOnly Property ItemsCounterFormat() As String
            Get
                Return ResourceManager.GetString("ItemsCounterFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} / {1}.
        '''</summary>
        Friend Shared ReadOnly Property MobileItemsCounterFormat() As String
            Get
                Return ResourceManager.GetString("MobileItemsCounterFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Next Image.
        '''</summary>
        Friend Shared ReadOnly Property NextImageButtonText() As String
            Get
                Return ResourceManager.GetString("NextImageButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Page {0} of {1}.
        '''</summary>
        Friend Shared ReadOnly Property PagerTextFormat() As String
            Get
                Return ResourceManager.GetString("PagerTextFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Pause Slideshow.
        '''</summary>
        Friend Shared ReadOnly Property PauseButtonText() As String
            Get
                Return ResourceManager.GetString("PauseButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Play Slideshow.
        '''</summary>
        Friend Shared ReadOnly Property PlayButtonText() As String
            Get
                Return ResourceManager.GetString("PlayButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Previous Image.
        '''</summary>
        Friend Shared ReadOnly Property PrevImageButtonText() As String
            Get
                Return ResourceManager.GetString("PrevImageButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Please do not remove this key..
        '''</summary>
        Friend Shared ReadOnly Property ReservedResource() As String
            Get
                Return ResourceManager.GetString("ReservedResource", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Scroll Next.
        '''</summary>
        Friend Shared ReadOnly Property ScrollNextButtonText() As String
            Get
                Return ResourceManager.GetString("ScrollNextButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Scroll Prev.
        '''</summary>
        Friend Shared ReadOnly Property ScrollPrevButtonText() As String
            Get
                Return ResourceManager.GetString("ScrollPrevButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Show Thumbnails.
        '''</summary>
        Friend Shared ReadOnly Property ShowThumbnailsButtonText() As String
            Get
                Return ResourceManager.GetString("ShowThumbnailsButtonText", resourceCulture)
            End Get
        End Property
    End Class
End Namespace
