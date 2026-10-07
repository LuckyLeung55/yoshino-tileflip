Public Class Form1
    '声明Random的实例
    Private random As New Random

    '图形符号
    'Private icons =
    '    New List(Of String) From {"!", "!", "N", "N", ",", ",", "k", "k",
    '                                "b", "b", "v", "v", "w", "w", "z", "z"}

    Private icons =
        New List(Of String) From {"img1", "img1", "img2", "img2", "img3", "img3", "img4", "img4",
                                    "img5", "img5", "img6", "img6", "img7", "img7", "img8", "img8"}

    ' 根据标识取图片
    Private Function GetImageByKey(key As String) As Image
        Dim path As String = Application.StartupPath & "\image\" & key & ".png"
        If System.IO.File.Exists(path) Then

            Return Image.FromStream(New System.IO.MemoryStream(System.IO.File.ReadAllBytes(path)))
        Else
            Return Nothing
        End If
    End Function

    '随机显示符号
    Private Sub AssignIconsToSquares()

        icons = New List(Of String) From {
            "img1", "img1", "img2", "img2", "img3", "img3", "img4", "img4",
            "img5", "img5", "img6", "img6", "img7", "img7", "img8", "img8"
        }

        For Each control In TableLayoutPanel1.Controls
            Dim iconLabel = TryCast(control, Label)
            If iconLabel IsNot Nothing Then
                Dim randomNumber = random.Next(icons.Count)

                Dim key = icons(randomNumber)

                iconLabel.Tag = "hidden|" & key
                iconLabel.Text = "" ' 隐藏文字
                iconLabel.Image = GetImageByKey("back")

                icons.RemoveAt(randomNumber)
            End If
        Next

    End Sub

    '执行随机显示符号
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AssignIconsToSquares()
    End Sub

    '定义第一第二次变量名
    Private firstClicked As Label = Nothing


    Private secondClicked As Label = Nothing


    '敲击响应
    Private Sub label_Click(ByVal sender As Object, ByVal e As EventArgs) _
Handles Label1.Click, Label2.Click, Label3.Click, Label4.Click,
        Label5.Click, Label6.Click, Label7.Click, Label8.Click,
        Label9.Click, Label10.Click, Label11.Click, Label12.Click,
        Label13.Click, Label14.Click, Label15.Click, Label16.Click

        If Timer1.Enabled Then Exit Sub

        Dim clickedLabel = TryCast(sender, Label)
        If clickedLabel Is Nothing Then Exit Sub

        ' 【关键修改】如果 Tag 是以 "flipped" 开头的，说明已经翻开，忽略点击
        Dim tagStr = clickedLabel.Tag.ToString()
        If tagStr.StartsWith("flipped|") Then Exit Sub

        ' 提取卡牌的真实 key
        Dim realKey = tagStr.Replace("hidden|", "").Replace("flipped|", "")

        ' 翻开卡牌
        clickedLabel.Tag = "flipped|" & realKey
        clickedLabel.Image = GetImageByKey(realKey)

        ' 第一次点击
        If firstClicked Is Nothing Then
            firstClicked = clickedLabel
            Exit Sub
        End If

        ' 第二次点击
        secondClicked = clickedLabel

        If firstClicked.Tag.ToString().Replace("flipped|", "") = secondClicked.Tag.ToString().Replace("flipped|", "") Then
            firstClicked = Nothing
            secondClicked = Nothing
            CheckForWinner()
            Exit Sub
        End If

        Timer1.Start()
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()

        ' 重新盖住，改回 hidden 状态
        firstClicked.Tag = "hidden|" & firstClicked.Tag.ToString().Replace("flipped|", "")
        firstClicked.Image = GetImageByKey("back")

        secondClicked.Tag = "hidden|" & secondClicked.Tag.ToString().Replace("flipped|", "")
        secondClicked.Image = GetImageByKey("back")

        firstClicked = Nothing
        secondClicked = Nothing
    End Sub

    Private Sub CheckForWinner()
        For Each control In TableLayoutPanel1.Controls
            Dim iconLabel = TryCast(control, Label)
            If iconLabel IsNot Nothing AndAlso iconLabel.Tag.ToString().StartsWith("hidden|") Then
                Exit Sub ' 只要还有处于 hidden 状态的卡片，就还没赢
            End If
        Next

        MessageBox.Show("恭喜你挑战成功!", "来自七罪大人的祝福")
        Close()
    End Sub

End Class