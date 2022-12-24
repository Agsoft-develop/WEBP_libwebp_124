<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.pb = New System.Windows.Forms.PictureBox()
        Me.bt_decode = New System.Windows.Forms.Button()
        Me.bt_info = New System.Windows.Forms.Button()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        Me.bt_encode = New System.Windows.Forms.Button()
        Me.sfd = New System.Windows.Forms.SaveFileDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.bt_decomp = New System.Windows.Forms.Button()
        Me.bt_comp = New System.Windows.Forms.Button()
        CType(Me.pb, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pb
        '
        Me.pb.Location = New System.Drawing.Point(11, 10)
        Me.pb.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pb.Name = "pb"
        Me.pb.Size = New System.Drawing.Size(662, 412)
        Me.pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pb.TabIndex = 0
        Me.pb.TabStop = False
        '
        'bt_decode
        '
        Me.bt_decode.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bt_decode.Location = New System.Drawing.Point(11, 459)
        Me.bt_decode.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bt_decode.Name = "bt_decode"
        Me.bt_decode.Size = New System.Drawing.Size(235, 38)
        Me.bt_decode.TabIndex = 1
        Me.bt_decode.Text = "Decode (Load .webp)"
        Me.bt_decode.UseVisualStyleBackColor = True
        '
        'bt_info
        '
        Me.bt_info.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bt_info.Location = New System.Drawing.Point(503, 459)
        Me.bt_info.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bt_info.Name = "bt_info"
        Me.bt_info.Size = New System.Drawing.Size(160, 38)
        Me.bt_info.TabIndex = 2
        Me.bt_info.Text = "Image info"
        Me.bt_info.UseVisualStyleBackColor = True
        '
        'ofd
        '
        Me.ofd.FileName = "OpenFileDialog1"
        '
        'bt_encode
        '
        Me.bt_encode.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bt_encode.Location = New System.Drawing.Point(252, 459)
        Me.bt_encode.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bt_encode.Name = "bt_encode"
        Me.bt_encode.Size = New System.Drawing.Size(245, 38)
        Me.bt_encode.TabIndex = 3
        Me.bt_encode.Text = "Encode (Save as .webp)"
        Me.bt_encode.UseVisualStyleBackColor = True
        '
        'sfd
        '
        Me.sfd.Filter = "|.webp"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Maroon
        Me.Label1.Location = New System.Drawing.Point(15, 431)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(228, 22)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "by libweb.dll (version 1.2.4)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Maroon
        Me.Label2.Location = New System.Drawing.Point(15, 506)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(343, 22)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "by convert tools (dwebp.exe) (cwebp.exe)"
        '
        'bt_decomp
        '
        Me.bt_decomp.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bt_decomp.Location = New System.Drawing.Point(12, 540)
        Me.bt_decomp.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bt_decomp.Name = "bt_decomp"
        Me.bt_decomp.Size = New System.Drawing.Size(219, 38)
        Me.bt_decomp.TabIndex = 6
        Me.bt_decomp.Text = "webp to image (dwebp)"
        Me.bt_decomp.UseVisualStyleBackColor = True
        '
        'bt_comp
        '
        Me.bt_comp.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bt_comp.Location = New System.Drawing.Point(267, 540)
        Me.bt_comp.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bt_comp.Name = "bt_comp"
        Me.bt_comp.Size = New System.Drawing.Size(230, 38)
        Me.bt_comp.TabIndex = 7
        Me.bt_comp.Text = "image to webp (cwebp)"
        Me.bt_comp.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(688, 589)
        Me.Controls.Add(Me.bt_comp)
        Me.Controls.Add(Me.bt_decomp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.bt_encode)
        Me.Controls.Add(Me.bt_info)
        Me.Controls.Add(Me.bt_decode)
        Me.Controls.Add(Me.pb)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.pb, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pb As PictureBox
    Friend WithEvents bt_decode As Button
    Friend WithEvents bt_info As Button
    Friend WithEvents ofd As OpenFileDialog
    Friend WithEvents bt_encode As Button
    Friend WithEvents sfd As SaveFileDialog
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents bt_decomp As Button
    Friend WithEvents bt_comp As Button
End Class
