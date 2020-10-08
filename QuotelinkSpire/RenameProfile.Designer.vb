<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RenameProfile
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RenameProfile))
        Me.newProfileName = New System.Windows.Forms.TextBox()
        Me.okRenameButton = New System.Windows.Forms.Button()
        Me.cancelRenameButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'newProfileName
        '
        Me.newProfileName.Location = New System.Drawing.Point(12, 13)
        Me.newProfileName.Name = "newProfileName"
        Me.newProfileName.Size = New System.Drawing.Size(260, 22)
        Me.newProfileName.TabIndex = 0
        '
        'okRenameButton
        '
        Me.okRenameButton.Location = New System.Drawing.Point(116, 44)
        Me.okRenameButton.Name = "okRenameButton"
        Me.okRenameButton.Size = New System.Drawing.Size(75, 25)
        Me.okRenameButton.TabIndex = 1
        Me.okRenameButton.Text = "Ok"
        Me.okRenameButton.UseVisualStyleBackColor = True
        '
        'cancelRenameButton
        '
        Me.cancelRenameButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelRenameButton.Location = New System.Drawing.Point(197, 44)
        Me.cancelRenameButton.Name = "cancelRenameButton"
        Me.cancelRenameButton.Size = New System.Drawing.Size(75, 25)
        Me.cancelRenameButton.TabIndex = 2
        Me.cancelRenameButton.Text = "Cancel"
        Me.cancelRenameButton.UseVisualStyleBackColor = True
        '
        'RenameProfile
        '
        Me.AcceptButton = Me.okRenameButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cancelRenameButton
        Me.ClientSize = New System.Drawing.Size(284, 81)
        Me.Controls.Add(Me.cancelRenameButton)
        Me.Controls.Add(Me.okRenameButton)
        Me.Controls.Add(Me.newProfileName)
        Me.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RenameProfile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rename Profile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents newProfileName As System.Windows.Forms.TextBox
    Friend WithEvents okRenameButton As System.Windows.Forms.Button
    Friend WithEvents cancelRenameButton As System.Windows.Forms.Button
End Class
