Imports System.Runtime.InteropServices

Public Class UserInstance
    Implements IDisposable

    Private handle As IntPtr = IntPtr.Zero
    Private disposed As Boolean = False

    Sub New(hWin As IntPtr)
        ' TODO: Complete member initialization 
        Dim myPropertyName As String = "USERNAME." & System.Environment.UserName
        handle = hWin
        SetProp(handle, myPropertyName, hWin.ToInt32)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        Dim myPropertyName As String = "USERNAME." & System.Environment.UserName
        If Not disposed Then

            'remove the property
            RemoveProp(handle, myPropertyName)
            disposed = True
        End If
    End Sub

    Public Shared Function IsSameUser(hWin As IntPtr) As Boolean
        If hWin.ToInt32 = 0 Then

            'If the main window has not been started yet
            'then the user clicked too many times.
            Return True

        Else

            Dim myPropertyName As String = "USERNAME." & System.Environment.UserName
            'If the property does not exist then this 
            'is a different user
            Dim ptr As Int32 = GetProp(hWin, myPropertyName)
            Return (ptr <> 0)
        End If
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function GetProp(ByVal hwnd As IntPtr, ByVal lpString As String) As Int32
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SetProp(ByVal hwnd As IntPtr, ByVal lpString As String, ByVal hData As Int32) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function RemoveProp(ByVal hwnd As IntPtr, ByVal lpString As String) As Int32
    End Function
    'Public Declare Function GetProp Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String) As Long
    'Public Declare Function SetProp Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String, ByVal hData As Int32) As Long
    'Public Declare Function RemoveProp Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String) As Long


End Class
