﻿'------------------------------------------------------------------------------
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


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SlspnStringText() As String
            Get
                Return CType(Me("SlspnStringText"),String)
            End Get
            Set
                Me("SlspnStringText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property DSNNameText() As String
            Get
                Return CType(Me("DSNNameText"),String)
            End Get
            Set
                Me("DSNNameText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property DSNServerText() As String
            Get
                Return CType(Me("DSNServerText"),String)
            End Get
            Set
                Me("DSNServerText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property DescriptionCheck() As Boolean
            Get
                Return CType(Me("DescriptionCheck"),Boolean)
            End Get
            Set
                Me("DescriptionCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property SyncPriceCheck() As Boolean
            Get
                Return CType(Me("SyncPriceCheck"),Boolean)
            End Get
            Set
                Me("SyncPriceCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property SyncCostCheck() As Boolean
            Get
                Return CType(Me("SyncCostCheck"),Boolean)
            End Get
            Set
                Me("SyncCostCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property NewCustomerCombo() As Integer
            Get
                Return CType(Me("NewCustomerCombo"),Integer)
            End Get
            Set
                Me("NewCustomerCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property UpdateItemCombo() As Integer
            Get
                Return CType(Me("UpdateItemCombo"),Integer)
            End Get
            Set
                Me("UpdateItemCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ItemNumberMapCombo() As Integer
            Get
                Return CType(Me("ItemNumberMapCombo"),Integer)
            End Get
            Set
                Me("ItemNumberMapCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property TransTypeCombo() As Integer
            Get
                Return CType(Me("TransTypeCombo"),Integer)
            End Get
            Set
                Me("TransTypeCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property NewItemCombo() As Integer
            Get
                Return CType(Me("NewItemCombo"),Integer)
            End Get
            Set
                Me("NewItemCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property FilterTypeCombo() As Integer
            Get
                Return CType(Me("FilterTypeCombo"),Integer)
            End Get
            Set
                Me("FilterTypeCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property FilterStageCombo() As String
            Get
                Return CType(Me("FilterStageCombo"),String)
            End Get
            Set
                Me("FilterStageCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property InvSyncWarehouseCombo() As String
            Get
                Return CType(Me("InvSyncWarehouseCombo"),String)
            End Get
            Set
                Me("InvSyncWarehouseCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property DefaultWarehouseCombo() As String
            Get
                Return CType(Me("DefaultWarehouseCombo"),String)
            End Get
            Set
                Me("DefaultWarehouseCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property UseSQLBackendCheck() As Boolean
            Get
                Return CType(Me("UseSQLBackendCheck"),Boolean)
            End Get
            Set
                Me("UseSQLBackendCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SQLServerText() As String
            Get
                Return CType(Me("SQLServerText"),String)
            End Get
            Set
                Me("SQLServerText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SQLDatabaseText() As String
            Get
                Return CType(Me("SQLDatabaseText"),String)
            End Get
            Set
                Me("SQLDatabaseText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SQLUsernameText() As String
            Get
                Return CType(Me("SQLUsernameText"),String)
            End Get
            Set
                Me("SQLUsernameText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SQLPasswordText() As String
            Get
                Return CType(Me("SQLPasswordText"),String)
            End Get
            Set
                Me("SQLPasswordText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
        Public Property DefaultProfile() As Integer
            Get
                Return CType(Me("DefaultProfile"),Integer)
            End Get
            Set
                Me("DefaultProfile") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Profile 1")>  _
        Public Property Profile1Name() As String
            Get
                Return CType(Me("Profile1Name"),String)
            End Get
            Set
                Me("Profile1Name") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Profile 2")>  _
        Public Property Profile2Name() As String
            Get
                Return CType(Me("Profile2Name"),String)
            End Get
            Set
                Me("Profile2Name") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property SellPriceLevelCombo() As Integer
            Get
                Return CType(Me("SellPriceLevelCombo"),Integer)
            End Get
            Set
                Me("SellPriceLevelCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property BOMComboBox() As Integer
            Get
                Return CType(Me("BOMComboBox"),Integer)
            End Get
            Set
                Me("BOMComboBox") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Profile 3")>  _
        Public Property Profile3Name() As String
            Get
                Return CType(Me("Profile3Name"),String)
            End Get
            Set
                Me("Profile3Name") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property QWInstallation() As String
            Get
                Return CType(Me("QWInstallation"),String)
            End Get
            Set
                Me("QWInstallation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property SequentialCustCheck() As Boolean
            Get
                Return CType(Me("SequentialCustCheck"),Boolean)
            End Get
            Set
                Me("SequentialCustCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property SuppressSlspnWarningCheck() As Boolean
            Get
                Return CType(Me("SuppressSlspnWarningCheck"),Boolean)
            End Get
            Set
                Me("SuppressSlspnWarningCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property IntroductionNotesCheck() As Boolean
            Get
                Return CType(Me("IntroductionNotesCheck"),Boolean)
            End Get
            Set
                Me("IntroductionNotesCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property ClosingNotesCheck() As Boolean
            Get
                Return CType(Me("ClosingNotesCheck"),Boolean)
            End Get
            Set
                Me("ClosingNotesCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property PurchasingNotesCheck() As Boolean
            Get
                Return CType(Me("PurchasingNotesCheck"),Boolean)
            End Get
            Set
                Me("PurchasingNotesCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property InternalNotesCheck() As Boolean
            Get
                Return CType(Me("InternalNotesCheck"),Boolean)
            End Get
            Set
                Me("InternalNotesCheck") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property DateMapCombo() As Integer
            Get
                Return CType(Me("DateMapCombo"),Integer)
            End Get
            Set
                Me("DateMapCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property CustomerNoMap() As Integer
            Get
                Return CType(Me("CustomerNoMap"),Integer)
            End Get
            Set
                Me("CustomerNoMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("29")>  _
        Public Property CustomerNameMap() As Integer
            Get
                Return CType(Me("CustomerNameMap"),Integer)
            End Get
            Set
                Me("CustomerNameMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("32")>  _
        Public Property CustAddr1Map() As Integer
            Get
                Return CType(Me("CustAddr1Map"),Integer)
            End Get
            Set
                Me("CustAddr1Map") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("33")>  _
        Public Property CustAddr2Map() As Integer
            Get
                Return CType(Me("CustAddr2Map"),Integer)
            End Get
            Set
                Me("CustAddr2Map") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("34")>  _
        Public Property CustAddr3Map() As Integer
            Get
                Return CType(Me("CustAddr3Map"),Integer)
            End Get
            Set
                Me("CustAddr3Map") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("35")>  _
        Public Property CustCityMap() As Integer
            Get
                Return CType(Me("CustCityMap"),Integer)
            End Get
            Set
                Me("CustCityMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("37")>  _
        Public Property CustPostalCodeMap() As Integer
            Get
                Return CType(Me("CustPostalCodeMap"),Integer)
            End Get
            Set
                Me("CustPostalCodeMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("36")>  _
        Public Property CustProvinceMap() As Integer
            Get
                Return CType(Me("CustProvinceMap"),Integer)
            End Get
            Set
                Me("CustProvinceMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("38")>  _
        Public Property CustCountryMap() As Integer
            Get
                Return CType(Me("CustCountryMap"),Integer)
            End Get
            Set
                Me("CustCountryMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("40")>  _
        Public Property CustPhoneMap() As Integer
            Get
                Return CType(Me("CustPhoneMap"),Integer)
            End Get
            Set
                Me("CustPhoneMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("41")>  _
        Public Property CustEmailMap() As Integer
            Get
                Return CType(Me("CustEmailMap"),Integer)
            End Get
            Set
                Me("CustEmailMap") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SpireURLText() As String
            Get
                Return CType(Me("SpireURLText"),String)
            End Get
            Set
                Me("SpireURLText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SpireUsernameText() As String
            Get
                Return CType(Me("SpireUsernameText"),String)
            End Get
            Set
                Me("SpireUsernameText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SpirePasswordText() As String
            Get
                Return CType(Me("SpirePasswordText"),String)
            End Get
            Set
                Me("SpirePasswordText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property DefaultSyncSellPriceLevelCombo() As Integer
            Get
                Return CType(Me("DefaultSyncSellPriceLevelCombo"),Integer)
            End Get
            Set
                Me("DefaultSyncSellPriceLevelCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property TerrSpreadsheetLocation() As String
            Get
                Return CType(Me("TerrSpreadsheetLocation"),String)
            End Get
            Set
                Me("TerrSpreadsheetLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property TerrWorksheet() As String
            Get
                Return CType(Me("TerrWorksheet"),String)
            End Get
            Set
                Me("TerrWorksheet") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property VendorStringText() As String
            Get
                Return CType(Me("VendorStringText"),String)
            End Get
            Set
                Me("VendorStringText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property CustomerNoMapCombo() As Integer
            Get
                Return CType(Me("CustomerNoMapCombo"),Integer)
            End Get
            Set
                Me("CustomerNoMapCombo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property TerritoryMapCombo() As Integer
            Get
                Return CType(Me("TerritoryMapCombo"),Integer)
            End Get
            Set
                Me("TerritoryMapCombo") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.QuotelinkSpire.My.MySettings
            Get
                Return Global.QuotelinkSpire.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
