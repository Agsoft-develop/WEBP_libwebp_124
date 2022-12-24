Imports System.Security.Cryptography
Imports System.IO
Imports WEBP_libwebp_124.libwebp
Imports System.Net.Mail

'class library is catched up from : https://github.com/JosePineiro/WebP-wrapper
Public Class Form1
    Public _filename As String
    Private Sub bt_decode_Click(sender As Object, e As EventArgs) Handles bt_decode.Click
        ofd.Filter = "WEBP Files | *.webp"
        If ofd.ShowDialog = DialogResult.OK Then
            Dim get_format As String
            _filename = ofd.FileName
            Dim file_info = New IO.FileInfo(_filename)
            get_format = file_info.Extension
            get_format = get_format.ToLower
            Dim byt As Byte() = File.ReadAllBytes(_filename)
            If get_format = ".webp" Then
                Dim webp_decode As New libwebp
                'pb.Image = webp_decode.Decode(_filename)  'decode by file name
                pb.Image = webp_decode.Decode(byt)
            Else
                pb.Image = Image.FromFile(_filename)
            End If
        End If

    End Sub

    Private Sub bt_info_Click(sender As Object, e As EventArgs) Handles bt_info.Click

        If pb.Image IsNot Nothing Then
            Dim file_info = New IO.FileInfo(_filename)
            Dim get_format As String
            get_format = file_info.Extension
            get_format = get_format.ToLower

            Dim imgWidth As Integer = Nothing, imgHeight As Integer = Nothing

            If get_format = ".webp" Then
                Dim getinfo As New libwebp
                Dim hasAlpha As Boolean = Nothing, hasAnimation As Boolean = Nothing, format As String = Nothing
                Dim webp_byte As Byte() = File.ReadAllBytes(_filename)
                getinfo.Get_Info(webp_byte, imgWidth, imgHeight, hasAlpha, hasAnimation, format)

                MsgBox("Image format: " + get_format + imgWidth.ToString + " WXH " + imgHeight.ToString + ", Alpha: " +
                hasAlpha.ToString + ", Animation: " + hasAnimation.ToString + ", Format: " + format.ToString, MsgBoxStyle.Information)
            Else
                imgWidth = pb.Image.Width
                imgHeight = pb.Image.Height
                MsgBox("Image format: " + get_format + imgWidth.ToString + " WXH " + imgHeight.ToString)
            End If

        Else
            MsgBox("Picture box is empty")
        End If



    End Sub

    Private Sub bt_encode_Click(sender As Object, e As EventArgs) Handles bt_encode.Click
        If pb.Image IsNot Nothing Then
            If sfd.ShowDialog = DialogResult.OK Then
                Dim webp_encode As New libwebp
                Dim bt() As Byte
                bt = webp_encode.Encode(pb.Image, 100)
                File.WriteAllBytes(sfd.FileName, bt)
                MsgBox("Down")
            End If
        Else
            MsgBox("Picture box is empty")
        End If

    End Sub

    Function CompressTO_webp(ByVal origin As String, ByVal destination As String, Optional quality As String = "75")
        ' www.ryancooper.com/resources/SaveasWebP.aspx
        '  Compress an image file to a WebP file by cwebp tools
        ' https://developers.google.com/speed/webp/docs/using
        Dim strReturn = "RETURNED NULL"
        Dim args = "cwebp " & origin & " -q " & quality & " -o " & destination & " "
        Dim startinfo = New ProcessStartInfo()
        startinfo.FileName = System.Environment.CurrentDirectory + "\cwebp.exe"
        startinfo.Arguments = args
        startinfo.RedirectStandardOutput = True
        startinfo.CreateNoWindow = False
        startinfo.UseShellExecute = False

        Using proc As Process = New Process()
            proc.StartInfo = startinfo
            proc.Start()
            strReturn = proc.StandardOutput.ReadToEnd()
        End Using

        Return strReturn
    End Function

    Function DecopressTO_bitmap(origin, destination)

        ' www.ryancooper.com/resources/SaveasWebP.aspx
        ' Decompress a WebP file to an image file by dwebp tools
        ' https://developers.google.com/speed/webp/docs/using
        Dim strReturn = "RETURNED NULL"
        Dim args = "dwebp " & origin & " -o " & destination & " "
        Dim startinfo = New ProcessStartInfo()
        startinfo.FileName = System.Environment.CurrentDirectory + "\dwebp.exe"
        startinfo.Arguments = args
        startinfo.RedirectStandardOutput = True
        startinfo.CreateNoWindow = False
        startinfo.UseShellExecute = False

        Using proc As Process = New Process()
            proc.StartInfo = startinfo
            proc.Start()
            strReturn = proc.StandardOutput.ReadToEnd()
        End Using
        Return strReturn
    End Function



    Private Sub bt_comp_Click(sender As Object, e As EventArgs) Handles bt_comp.Click
        ofd.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png;"
        If ofd.ShowDialog = DialogResult.OK Then
            Dim load As String = Nothing, save As String = Nothing
            sfd.Filter = "WEBP Files | *.webp"
            If sfd.ShowDialog = DialogResult.OK Then
                save = sfd.FileName
                load = ofd.FileName
                CompressTO_webp(load, save)
            End If
        End If

    End Sub

    Private Sub bt_decomp_Click(sender As Object, e As EventArgs) Handles bt_decomp.Click
        ofd.Filter = "(.WEBP Files) | *.webp"
        If ofd.ShowDialog = DialogResult.OK Then
            Dim load As String = Nothing, save As String = Nothing
            sfd.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (*.jpeg*)|*.jpeg|Png Image (.png)|*.png"
            If sfd.ShowDialog = DialogResult.OK Then
                load = ofd.FileName
                save = sfd.FileName
                DecopressTO_bitmap(load, save)
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IntPtr.Size = 4 Then
            Me.Text = "System : x86"
        ElseIf IntPtr.Size = 8 Then
            Me.Text = "System : x64"
        End If


    End Sub
End Class
