Public Class ISKnob
    Dim knobImg As Bitmap = New Bitmap(My.Resources.knbY)
    Dim knobWidth As Integer = knobImg.Width
    Dim knobHeight As Integer = knobImg.Width
    Dim imgPosPoint As Point = New Point(-knobImg.Width, -knobImg.Width)
    Dim realValue As Double
    Dim realMax As Double = 10
    Dim realMin As Double = 0
    Dim lastValue As Double
    Dim newValue As Double
    Dim newImgPos As Double
    Dim shownValue As Double = SetValue
    Dim shownMax As Double = 10
    Dim shownMin As Double = 1
    Dim knobismoving As Boolean = False
    Dim mouseClickY As Integer
    Dim repvals As Boolean = False
	Dim savedPos As Double

    Property image() As Bitmap
        Get
            image = knobImg
        End Get
        Set(ByVal value As Bitmap)
            knobImg = value
            knobWidth = knobImg.Width
            knobHeight = knobImg.Width
            imgPosPoint = New Point(-knobImg.Width, savedPos)
        End Set
    End Property

    Property SetValue() As Double
        Get
            SetValue = shownValue
        End Get
        Set(ByVal value As Double)
            If value > shownMax Then value = shownMax
            If value < shownMin Then value = shownMin
            setInternalValue(value)
            shownValue = value
            lastValue = (((value - shownMin) * (realMax - realMin)) / (shownMax - shownMin)) + realMin
        End Set
    End Property

    Property SetMax() As Integer
        Get
            SetMax = shownMax
        End Get
        Set(value As Integer)
            shownMax = value
            If shownValue > shownMax Then shownValue = shownMax
            If shownValue < shownMin Then shownValue = shownMin
            setInternalValue(shownValue)
        End Set
    End Property

    Property SetMin() As Integer
        Get
            SetMin = shownMin
        End Get
        Set(value As Integer)
            shownMin = value
            If shownValue > shownMax Then shownValue = shownMax
            If shownValue < shownMin Then shownValue = shownMin
            setInternalValue(shownValue)
        End Set
    End Property

    Property ReportValues() As Boolean
        Get
            ReportValues = repvals
        End Get
        Set(value As Boolean)
            repvals = value
        End Set
    End Property

    Private Sub setInternalValue(ByVal value As Double)
        newValue = (((value - shownMin) * (realMax - realMin)) / (shownMax - shownMin)) + realMin
        Dim newImgPos As Integer = Math.Round(((-newValue * ((knobImg.Height - knobWidth) / (realMax - realMin))) - knobWidth) / knobWidth) * knobWidth
        realValue = newValue
        imgPosPoint.Y = newImgPos
        savedPos = imgPosPoint.Y
        Ref()
    End Sub

    Private Sub knob_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If knobismoving = False Then
            mouseClickY = e.Y
            knobismoving = True
        End If
        Ref()
    End Sub

    Private Sub knob_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If knobismoving = True Then
            Dim diff As Integer = mouseClickY - e.Y
            Dim newVal As Double = lastValue + diff / 10
            If newVal < realMin Then newVal = realMin
            If newVal > realMax Then newVal = realMax
            Dim newImgPos As Integer = Math.Round(((-newVal * ((knobImg.Height - knobWidth) / (realMax - realMin))) - knobWidth) / knobWidth) * knobWidth
            realValue = newVal
            shownValue = (((newVal - realMin) * (shownMax - shownMin)) / (realMax - realMin)) + shownMin
            imgPosPoint.Y = newImgPos
            savedPos = imgPosPoint.Y
        End If
        Ref()
    End Sub

    Private Sub UserControl11_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        knobismoving = False
        lastValue = realValue
        If repvals Then
            Report()
        End If
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Me.Height = knobWidth
        Me.Width = knobHeight
        e.Graphics.ResetTransform()
        e.Graphics.TranslateTransform(knobWidth, knobHeight)
        e.Graphics.DrawImage(knobImg, imgPosPoint)
    End Sub

    Public Sub Ref()
        Me.Refresh()
        EnableDoubleBuffering()
    End Sub

    Sub EnableDoubleBuffering()
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        Me.UpdateStyles()
    End Sub

    Public Sub Report()
        Console.WriteLine("realvalue: " & realValue & " // shownvalue: " & shownValue)
    End Sub

End Class
