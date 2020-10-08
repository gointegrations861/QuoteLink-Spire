Public Class ProfileManager
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        My.Settings.SettingsKey = "Global"
        My.Settings.Reload()
        profile1Name.Text = My.Settings.Profile1Name
        profile2Name.Text = My.Settings.Profile2Name
        profile3Name.Text = My.Settings.Profile3Name

    End Sub

    Private Sub ProfileRename_Click(sender As Object, e As EventArgs) Handles Profile1Rename.Click, Profile2Rename.Click, Profile3Rename.Click
        Dim item = CType(sender, Button)
        Dim profile = item.Name.Substring(7, 1)
        Dim renameDialog = New RenameProfile

        renameDialog.ShowDialog()

        If renameDialog.okClicked Then
            If profile = "1" Then
                profile1Name.Text = renameDialog.newProfileName.Text
                My.Settings.Profile1Name = renameDialog.newProfileName.Text
            ElseIf profile = "2" Then
                profile2Name.Text = renameDialog.newProfileName.Text
                My.Settings.Profile2Name = renameDialog.newProfileName.Text
            Else
                profile3Name.Text = renameDialog.newProfileName.Text
                My.Settings.Profile3Name = renameDialog.newProfileName.Text
            End If
            My.Settings.Save()
        End If

    End Sub
End Class