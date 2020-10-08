<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewItemPrompt
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
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.newItemLabel = New System.Windows.Forms.Label()
        Me.newItemCancelButton = New System.Windows.Forms.Button()
        Me.newItemGenerateButton = New System.Windows.Forms.Button()
        Me.newItemNonStockedButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(308, 124)
        Me.ShapeContainer1.TabIndex = 0
        Me.ShapeContainer1.TabStop = False
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom
        Me.RectangleShape1.FillColor = System.Drawing.SystemColors.ButtonHighlight
        Me.RectangleShape1.FillGradientColor = System.Drawing.Color.White
        Me.RectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(307, 72)
        '
        'newItemLabel
        '
        Me.newItemLabel.AutoSize = True
        Me.newItemLabel.BackColor = System.Drawing.SystemColors.Window
        Me.newItemLabel.Location = New System.Drawing.Point(12, 25)
        Me.newItemLabel.MaximumSize = New System.Drawing.Size(284, 0)
        Me.newItemLabel.Name = "newItemLabel"
        Me.newItemLabel.Size = New System.Drawing.Size(59, 16)
        Me.newItemLabel.TabIndex = 1
        Me.newItemLabel.Text = "New Item"
        '
        'newItemCancelButton
        '
        Me.newItemCancelButton.Location = New System.Drawing.Point(221, 87)
        Me.newItemCancelButton.Name = "newItemCancelButton"
        Me.newItemCancelButton.Size = New System.Drawing.Size(75, 25)
        Me.newItemCancelButton.TabIndex = 2
        Me.newItemCancelButton.Text = "Cancel"
        Me.newItemCancelButton.UseVisualStyleBackColor = True
        '
        'newItemGenerateButton
        '
        Me.newItemGenerateButton.Location = New System.Drawing.Point(140, 87)
        Me.newItemGenerateButton.Name = "newItemGenerateButton"
        Me.newItemGenerateButton.Size = New System.Drawing.Size(75, 25)
        Me.newItemGenerateButton.TabIndex = 3
        Me.newItemGenerateButton.Text = "Generate"
        Me.newItemGenerateButton.UseVisualStyleBackColor = True
        '
        'newItemNonStockedButton
        '
        Me.newItemNonStockedButton.Location = New System.Drawing.Point(48, 87)
        Me.newItemNonStockedButton.Name = "newItemNonStockedButton"
        Me.newItemNonStockedButton.Size = New System.Drawing.Size(86, 25)
        Me.newItemNonStockedButton.TabIndex = 4
        Me.newItemNonStockedButton.Text = "Non-Stocked"
        Me.newItemNonStockedButton.UseVisualStyleBackColor = True
        '
        'NewItemPrompt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(308, 124)
        Me.Controls.Add(Me.newItemNonStockedButton)
        Me.Controls.Add(Me.newItemGenerateButton)
        Me.Controls.Add(Me.newItemCancelButton)
        Me.Controls.Add(Me.newItemLabel)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "NewItemPrompt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Does Not Exist"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents newItemLabel As System.Windows.Forms.Label
    Friend WithEvents newItemCancelButton As System.Windows.Forms.Button
    Friend WithEvents newItemGenerateButton As System.Windows.Forms.Button
    Friend WithEvents newItemNonStockedButton As System.Windows.Forms.Button
End Class
