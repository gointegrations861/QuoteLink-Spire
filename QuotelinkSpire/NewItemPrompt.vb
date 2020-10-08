Public Class NewItemPrompt

    Public selection As Integer = 0
    Private Sub NewItemPrompt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub newItemCancelButton_Click(sender As Object, e As EventArgs) Handles newItemCancelButton.Click
        Me.Close()
    End Sub

    Private Sub newItemGenerateButton_Click(sender As Object, e As EventArgs) Handles newItemGenerateButton.Click
        selection = 1
        Me.Close()
    End Sub

    Private Sub newItemNonStockedButton_Click(sender As Object, e As EventArgs) Handles newItemNonStockedButton.Click
        selection = 2
        Me.Close()
    End Sub
End Class