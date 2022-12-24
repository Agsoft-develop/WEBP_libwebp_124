
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices

'class library is catched up from : https://github.com/JosePineiro/WebP-wrapper

Public Class libwebp

    Private Shared ReadOnly WEBP_DECODER_ABI_VERSION As Integer = &H209 'Version 1.2.4
    Private Const WEBP_MAX_DIMENSION As Integer = 16383

    'Decode by file name path
    Overloads Function Decode(ByVal pathFileName As String) As Bitmap
        Dim WebP_byte = File.ReadAllBytes(pathFileName)
        Dim bmp As Bitmap = Nothing
        Dim bmp_Data As BitmapData = Nothing
        Dim pinnedWebP = GCHandle.Alloc(WebP_byte, GCHandleType.Pinned)

        Dim imgWidth As Integer = Nothing, imgHeight As Integer = Nothing,
             hasAlpha As Boolean = Nothing, hasAnimation As Boolean = Nothing, format As String = Nothing

        Try
            Get_Info(WebP_byte, imgWidth, imgHeight, hasAlpha, hasAnimation, format)

            If hasAlpha Then
                bmp = New Bitmap(imgWidth, imgHeight, PixelFormat.Format32bppArgb)
            Else
                bmp = New Bitmap(imgWidth, imgHeight, PixelFormat.Format24bppRgb)
            End If
            bmp_Data = bmp.LockBits(New Rectangle(0, 0, imgWidth, imgHeight), ImageLockMode.[WriteOnly], bmp.PixelFormat)
            Dim outputSize As Integer = bmp_Data.Stride * imgHeight
            Dim ptrData As IntPtr = pinnedWebP.AddrOfPinnedObject()
            If bmp.PixelFormat = PixelFormat.Format24bppRgb Then
                WebPDecodeBGRInto(ptrData, WebP_byte.Length, bmp_Data.Scan0, outputSize, bmp_Data.Stride)
            Else
                _WebPDecodeBGRAInto(ptrData, WebP_byte.Length, bmp_Data.Scan0, outputSize, bmp_Data.Stride)
            End If

            Return bmp
        Catch __unusedException1__ As Exception
            Throw
        Finally
            'Unlock the pixels
            If bmp_Data IsNot Nothing Then bmp.UnlockBits(bmp_Data)

            'Free memory
            If pinnedWebP.IsAllocated Then pinnedWebP.Free()
        End Try
    End Function
    'Decode by byte()
    Overloads Function Decode(ByVal data_byte As Byte()) As Bitmap
        Dim bmp As Bitmap = Nothing
        Dim bmp_Data As BitmapData = Nothing
        Dim pinnedWebP = GCHandle.Alloc(data_byte, GCHandleType.Pinned)

        Dim imgWidth As Integer = Nothing, imgHeight As Integer = Nothing,
             hasAlpha As Boolean = Nothing, hasAnimation As Boolean = Nothing, format As String = Nothing

        Try
            Get_Info(data_byte, imgWidth, imgHeight, hasAlpha, hasAnimation, format)

            If hasAlpha Then
                bmp = New Bitmap(imgWidth, imgHeight, PixelFormat.Format32bppArgb)
            Else
                bmp = New Bitmap(imgWidth, imgHeight, PixelFormat.Format24bppRgb)
            End If
            bmp_Data = bmp.LockBits(New Rectangle(0, 0, imgWidth, imgHeight), ImageLockMode.[WriteOnly], bmp.PixelFormat)
            Dim outputSize As Integer = bmp_Data.Stride * imgHeight
            Dim ptrData As IntPtr = pinnedWebP.AddrOfPinnedObject()
            If bmp.PixelFormat = PixelFormat.Format24bppRgb Then
                WebPDecodeBGRInto(ptrData, data_byte.Length, bmp_Data.Scan0, outputSize, bmp_Data.Stride)
            Else
                _WebPDecodeBGRAInto(ptrData, data_byte.Length, bmp_Data.Scan0, outputSize, bmp_Data.Stride)
            End If

            Return bmp
        Catch __unusedException1__ As Exception
            Throw
        Finally
            'Unlock the pixels
            If bmp_Data IsNot Nothing Then bmp.UnlockBits(bmp_Data)

            'Free memory
            If pinnedWebP.IsAllocated Then pinnedWebP.Free()
        End Try
    End Function
    Public Sub Get_Info(ByVal WebP_byte As Byte(), ByRef width As Integer, ByRef height As Integer, ByRef has_alpha As Boolean, ByRef has_animation As Boolean, ByRef format As String)
        Dim result As VP8StatusCode
        Dim pinnedWebP = GCHandle.Alloc(WebP_byte, GCHandleType.Pinned)

        Try
            Dim ptrRawWebP As IntPtr = pinnedWebP.AddrOfPinnedObject()

            Dim features As WebPBitstreamFeatures = New WebPBitstreamFeatures()
            result = _WebPGetFeatures(ptrRawWebP, WebP_byte.Length, features)

            If result <> 0 Then Throw New Exception(result.ToString())

            width = features.Width
            height = features.Height
            If features.Has_alpha = 1 Then
                has_alpha = True
            Else
                has_alpha = False
            End If
            If features.Has_animation = 1 Then
                has_animation = True
            Else
                has_animation = False
            End If
            Select Case features.Format
                Case 1
                    format = "lossy"
                Case 2
                    format = "lossless"
                Case Else
                    format = "undefined"
            End Select
        Catch ex As Exception
            Throw New Exception(ex.Message & vbCrLf & "In WebP.GetInfo")
        Finally
            'Free memory
            If pinnedWebP.IsAllocated Then pinnedWebP.Free()
        End Try
    End Sub
    Friend Shared Sub _WebPDecodeBGRAInto(ByVal data As IntPtr, ByVal data_size As Integer, ByVal output_buffer As IntPtr, ByVal output_buffer_size As Integer, ByVal output_stride As Integer)
        Select Case IntPtr.Size
            Case 4
                If WebPDecodeBGRAInto_x86(data, CType(data_size, UIntPtr), output_buffer, output_buffer_size, output_stride) = Nothing Then Throw New InvalidOperationException("Can not decode WebP")
            Case 8
                If WebPDecodeBGRAInto_x64(data, CType(data_size, UIntPtr), output_buffer, output_buffer_size, output_stride) = Nothing Then Throw New InvalidOperationException("Can not decode WebP")
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Sub

    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPDecodeBGRAInto")>
    Private Shared Function WebPDecodeBGRAInto_x86(
         <[In]()> ByVal data As IntPtr, ByVal data_size As UIntPtr, ByVal output_buffer As IntPtr, ByVal output_buffer_size As Integer, ByVal output_stride As Integer) As IntPtr
    End Function

    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPDecodeBGRAInto")>
    Private Shared Function WebPDecodeBGRAInto_x64(
         <[In]()> ByVal data As IntPtr, ByVal data_size As UIntPtr, ByVal output_buffer As IntPtr, ByVal output_buffer_size As Integer, ByVal output_stride As Integer) As IntPtr
    End Function

    Friend Shared Sub WebPDecodeBGRInto(ByVal data As IntPtr, ByVal data_size As Integer, ByVal output_buffer As IntPtr, ByVal output_buffer_size As Integer, ByVal output_stride As Integer)
        Select Case IntPtr.Size
            Case 4
                If WebPDecodeBGRInto_x86(data, CType(data_size, UIntPtr), output_buffer, output_buffer_size, output_stride) = Nothing Then Throw New InvalidOperationException("Can not decode WebP")
            Case 8
                If WebPDecodeBGRInto_x64(data, CType(data_size, UIntPtr), output_buffer, output_buffer_size, output_stride) = Nothing Then Throw New InvalidOperationException("Can not decode WebP")
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Sub

    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPDecodeBGRInto")>
    Private Shared Function WebPDecodeBGRInto_x86(
            <[In]()> ByVal data As IntPtr, ByVal data_size As UIntPtr, ByVal output_buffer As IntPtr, ByVal output_buffer_size As Integer, ByVal output_stride As Integer) As IntPtr
    End Function

    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPDecodeBGRInto")>
    Private Shared Function WebPDecodeBGRInto_x64(
             <[In]()> ByVal data As IntPtr, ByVal data_size As UIntPtr, ByVal output_buffer As IntPtr, ByVal output_buffer_size As Integer, ByVal output_stride As Integer) As IntPtr
    End Function


    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPGetFeaturesInternal")>
    Private Shared Function WebPGetFeaturesInternal_x86(
            <[In]()> ByVal rawWebP As IntPtr, ByVal data_size As UIntPtr, ByRef features As WebPBitstreamFeatures, ByVal WEBP_DECODER_ABI_VERSION As Integer) As VP8StatusCode
    End Function

    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPGetFeaturesInternal")>
    Private Shared Function WebPGetFeaturesInternal_x64(
            <[In]()> ByVal rawWebP As IntPtr, ByVal data_size As UIntPtr, ByRef features As WebPBitstreamFeatures, ByVal WEBP_DECODER_ABI_VERSION As Integer) As VP8StatusCode
    End Function

    Friend Shared Function _WebPGetFeatures(ByVal rawWebP As IntPtr, ByVal data_size As Integer, ByRef features As WebPBitstreamFeatures) As VP8StatusCode
        Select Case IntPtr.Size
            Case 4
                Return WebPGetFeaturesInternal_x86(rawWebP, CType(data_size, UIntPtr), features, WEBP_DECODER_ABI_VERSION)
            Case 8
                Return WebPGetFeaturesInternal_x64(rawWebP, CType(data_size, UIntPtr), features, WEBP_DECODER_ABI_VERSION)
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Function
    Public Function GetVersion() As String
        Try
            Dim v As UInteger = CUInt(WebPGetDecoderVersion())
            Dim revision = v Mod 256
            Dim minor = (v >> 8) Mod 256
            Dim major = (v >> 16) Mod 256
            Return major & "." & minor & "." & revision
        Catch ex As Exception
            Throw New Exception(ex.Message & vbCrLf & "In WebP.GetVersion")
        End Try
    End Function
    Friend Shared Function WebPGetDecoderVersion() As Integer
        Select Case IntPtr.Size
            Case 4
                Return WebPGetDecoderVersion_x86()
            Case 8
                Return WebPGetDecoderVersion_x64()
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Function
    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPGetDecoderVersion")>
    Private Shared Function WebPGetDecoderVersion_x86() As Integer
    End Function
    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPGetDecoderVersion")>
    Private Shared Function WebPGetDecoderVersion_x64() As Integer
    End Function
    Public Function Encode(ByVal bmp As Bitmap, ByVal Optional quality As Integer = 75) As Byte()
        'test bmp
        If bmp.Width = 0 OrElse bmp.Height = 0 Then Throw New ArgumentException("Bitmap contains no data.", "bmp")
        If bmp.Width > WEBP_MAX_DIMENSION OrElse bmp.Height > WEBP_MAX_DIMENSION Then Throw New NotSupportedException("Bitmap's dimension is too large. Max is " & WEBP_MAX_DIMENSION & "x" + WEBP_MAX_DIMENSION & " pixels.")
        If Not bmp.PixelFormat = PixelFormat.Format24bppRgb And Not bmp.PixelFormat = PixelFormat.Format32bppArgb Then
            Throw New NotSupportedException("Only support Format24bppRgb and Format32bppArgb pixelFormat.")
        End If

        Dim bmpData As BitmapData = Nothing
        Dim unmanagedData = IntPtr.Zero

        Try
            Dim size As Integer

            'Get bmp data
            bmpData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.[ReadOnly], bmp.PixelFormat)

            'Compress the bmp data
            If bmp.PixelFormat = PixelFormat.Format24bppRgb Then
                size = WebPEncodeBGR(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, quality, unmanagedData)
            Else
                size = WebPEncodeBGRA(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, quality, unmanagedData)
            End If
            If size = 0 Then Throw New Exception("Can´t encode WebP")

            'Copy image compress data to output array
            Dim rawWebP = New Byte(size - 1) {}
            Marshal.Copy(unmanagedData, rawWebP, 0, size)

            Return rawWebP
        Catch ex As Exception
            Throw New Exception(ex.Message & vbCrLf & "In WebP.EncodeLossly")
        Finally
            'Unlock the pixels
            If bmpData IsNot Nothing Then bmp.UnlockBits(bmpData)

            'Free memory
            If unmanagedData <> IntPtr.Zero Then WebPFree(unmanagedData)
        End Try
    End Function
    Friend Shared Sub WebPFree(ByVal p As IntPtr)
        Select Case IntPtr.Size
            Case 4
                WebPFree_x86(p)
            Case 8
                WebPFree_x64(p)
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Sub

    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPFree")>
    Private Shared Sub WebPFree_x86(ByVal p As IntPtr)
    End Sub

    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPFree")>
    Private Shared Sub WebPFree_x64(ByVal p As IntPtr)
    End Sub

    Friend Shared Function WebPEncodeBGR(ByVal bgr As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal stride As Integer, ByVal quality_factor As Single, <Out> ByRef output As IntPtr) As Integer
        Select Case IntPtr.Size
            Case 4
                Return WebPEncodeBGR_x86(bgr, width, height, stride, quality_factor, output)
            Case 8
                Return WebPEncodeBGR_x64(bgr, width, height, stride, quality_factor, output)
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Function

    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPEncodeBGR")>
    Private Shared Function WebPEncodeBGR_x86(
        <[In]()> ByVal bgr As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal stride As Integer, ByVal quality_factor As Single, <Out> ByRef output As IntPtr) As Integer
    End Function
    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPEncodeBGR")>
    Private Shared Function WebPEncodeBGR_x64(
        <[In]()> ByVal bgr As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal stride As Integer, ByVal quality_factor As Single, <Out> ByRef output As IntPtr) As Integer
    End Function
    Friend Shared Function WebPEncodeBGRA(ByVal bgra As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal stride As Integer, ByVal quality_factor As Single, <Out> ByRef output As IntPtr) As Integer
        Select Case IntPtr.Size
            Case 4
                Return WebPEncodeBGRA_x86(bgra, width, height, stride, quality_factor, output)
            Case 8
                Return WebPEncodeBGRA_x64(bgra, width, height, stride, quality_factor, output)
            Case Else
                Throw New InvalidOperationException("Invalid platform. Can not find proper function")
        End Select
    End Function

    <DllImport("libwebp_x86.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPEncodeBGRA")>
    Private Shared Function WebPEncodeBGRA_x86(
        <[In]()> ByVal bgra As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal stride As Integer, ByVal quality_factor As Single, <Out> ByRef output As IntPtr) As Integer
    End Function

    <DllImport("libwebp_x64.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="WebPEncodeBGRA")>
    Private Shared Function WebPEncodeBGRA_x64(
        <[In]()> ByVal bgra As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal stride As Integer, ByVal quality_factor As Single, <Out> ByRef output As IntPtr) As Integer
    End Function

    Friend Structure WebPBitstreamFeatures
        Public Width As Integer
        Public Height As Integer
        Public Has_alpha As Integer
        Public Has_animation As Integer
        Public Format As Integer
        <MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst:=5, ArraySubType:=UnmanagedType.U4)>
        Private ReadOnly pad As UInteger()
    End Structure
    Friend Enum VP8StatusCode
        VP8_STATUS_OK = 0
        VP8_STATUS_OUT_OF_MEMORY
        VP8_STATUS_INVALID_PARAM
        VP8_STATUS_BITSTREAM_ERROR
        VP8_STATUS_UNSUPPORTED_FEATURE
        VP8_STATUS_SUSPENDED
        VP8_STATUS_USER_ABORT
        VP8_STATUS_NOT_ENOUGH_DATA
    End Enum

End Class
