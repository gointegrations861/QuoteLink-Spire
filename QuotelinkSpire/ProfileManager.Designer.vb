<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProfileManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProfileManager))
        Me.profile1Name = New System.Windows.Forms.Label()
        Me.profile2Name = New System.Windows.Forms.Label()
        Me.Profile1Rename = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Profile3Rename = New System.Windows.Forms.Button()
        Me.profile3Name = New System.Windows.Forms.Label()
        Me.Profile2Rename = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'profile1Name
        '
        Me.profile1Name.AutoSize = True
        Me.profile1Name.Location = New System.Drawing.Point(6, 26)
        Me.profile1Name.Name = "profile1Name"
        Me.profile1Name.Size = New System.Drawing.Size(42, 16)
        Me.profile1Name.TabIndex = 0
        Me.profile1Name.Text = "Label1"
        '
        'profile2Name
        '
        Me.profile2Name.AutoSize = True
        Me.profile2Name.Location = New System.Drawing.Point(6, 56)
        Me.profile2Name.Name = "profile2Name"
        Me.profile2Name.Size = New System.Drawing.Size(42, 16)
        Me.profile2Name.TabIndex = 1
        Me.profile2Name.Text = "Label1"
        '
        'Profile1Rename
        '
        Me.Profile1Rename.Location = New System.Drawing.Point(241, 20)
        Me.Profile1Rename.Name = "Profile1Rename"
        Me.Profile1Rename.Size = New System.Drawing.Size(59, 25)
        Me.Profile1Rename.TabIndex = 2
        Me.Profile1Rename.Text = "Rename"
        Me.Profile1Rename.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Profile3Rename)
        Me.GroupBox1.Controls.Add(Me.profile3Name)
        Me.GroupBox1.Controls.Add(Me.Profile2Rename)
        Me.GroupBox1.Controls.Add(Me.profile1Name)
        Me.GroupBox1.Controls.Add(Me.profile2Name)
        Me.GroupBox1.Controls.Add(Me.Profile1Rename)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(306, 122)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Profiles"
        '
        'Profile3Rename
        '
        Me.Profile3Rename.Location = New System.Drawing.Point(241, 82)
        Me.Profile3Rename.Name = "Profile3Rename"
        Me.Profile3Rename.Size = New System.Drawing.Size(59, 25)
        Me.Profile3Rename.TabIndex = 5
        Me.Profile3Rename.Text = "Rename"
        Me.Profile3Rename.UseVisualStyleBackColor = True
        '
        'profile3Name
        '
        Me.profile3Name.AutoSize = True
        Me.profile3Name.Location = New System.Drawing.Point(6, 87)
        Me.profile3Name.Name = "profile3Name"
        Me.profile3Name.Size = New System.Drawing.Size(42, 16)
        Me.profile3Name.TabIndex = 4
        Me.profile3Name.Text = "Label1"
        '
        'Profile2Rename
        '
        Me.Profile2Rename.Location = New System.Drawing.Point(241, 51)
        Me.Profile2Rename.Name = "Profile2Rename"
        Me.Profile2Rename.Size = New System.Drawing.Size(59, 25)
        Me.Profile2Rename.TabIndex = 3
        Me.Profile2Rename.Text = "Rename"
        Me.Profile2Rename.UseVisualStyleBackColor = True
        '
        'ProfileManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(330, 148)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ProfileManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Profile Manager"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents profile1Name As System.Windows.Forms.Label
    Friend WithEvents profile2Name As System.Windows.Forms.Label
    Friend WithEvents Profile1Rename As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Profile2Rename As System.Windows.Forms.Button
    Friend WithEvents Profile3Rename As System.Windows.Forms.Button
    Friend WithEvents profile3Name As System.Windows.Forms.Label
End Class
