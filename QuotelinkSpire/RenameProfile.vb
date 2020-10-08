Public Class RenameProfile
    Public okClicked As Boolean = False
    Private Sub okButton_Click(sender As Object, e As EventArgs) Handles okRenameButton.Click
        okClicked = True
        Me.Close()
    End Sub

    Private Sub cancelRenameButton_Click(sender As Object, e As EventArgs) Handles cancelRenameButton.Click
        Me.Close()
    End Sub
End Class