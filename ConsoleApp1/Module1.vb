Imports BroadLib


Module Module1

    Const bottom64& = &H8000000000000000
    Const top64& = &H7FFFFFFFFFFFFFFF


    Dim a As New Broad(10050, 5300)
    'Dim a As New Broad(50000, 49990)
    'Dim a As New Broad(4000, 3997)

    Dim b As New Broad(5, 2)
    'Dim b As New Broad(a)
    Dim r As Broad

    Sub Main()
        Dim x& = &H8000000000000000
        Dim y&
        Dim tb As BFloat160  ' broad.Balanced128

        'On Error Resume Next

        b(1) = top64
        b(2) = top64
        b(-2) = 22



        b.Round(5)
        b.Round(2)
        b.Round(1)
        b.Round(0)


        a(0) = 1 ' top64


        Broad.displayMode = Broad.displayModeType.inHex


        'Dim bflt As New BFloat160(d) ' ^ (-1)) ' 1032) '.333333333333) '.getFromFloat64(1024) '-0.75)
        'sss = BFloat160.GetBFloat(CDbl(2 ^ (-64))) ' ^ (-1)) ' 1032) '.333333333333) '.getFromFloat64(1024) '-0.75)
        ' a(3) = 123456
        'printValue(bflt, "b")
        printValue(a, "A1")
        'a.Add(CDbl(1 / 300000))
        'a.Add(bflt)

        For i = 0 To 6100 '000

            a.Multiply(5000001) ' 2739654
            'a = a + b ' <-- YOU MAY USE THIS, HOWEVER
            'a.Add(b) IS FASTER, SAME GOES FOR THE "*" OPERATOR VS. MULTIPLY FUNCTION

        Next

        printValue(a, "A2")

        For i = 0 To 6100 '000
            'a = a - b
            a.Divide(5000001)
            'a.Add(sss)
            ' a.Subtract(CDbl(1 / 3))
            'a = a / 5000001
            'a = a * (1 / 0.7) '2739654
        Next
        printValue(a, "A3")
        'showValues()


        Console.ReadKey()


    End Sub



    Sub printValue(ByRef b As BFloat160, Optional name$ = "BFloat")
        Console.WriteLine(name + " = " + b.ToString)
        Console.WriteLine("")

    End Sub

    Sub printValue(ByRef b As Broad, Optional name$ = "")
        Console.WriteLine(name + " = ")
        Console.WriteLine(b)
        Console.WriteLine("")


    End Sub

    Sub showValues()

        Console.WriteLine(a.ToString)
        Console.WriteLine(" B = ")
        Console.WriteLine(b)
        Console.WriteLine("")

        Console.WriteLine(" R = ")
        Console.WriteLine(r)
        Console.WriteLine("")
        Console.WriteLine("")

    End Sub


End Module
