Imports BroadLib


Module test

    Const bottom64& = &H8000000000000000
    Const top64& = &H7FFFFFFFFFFFFFFF


    Dim a As New Broad(10050, 5300)

    Dim b As New Broad(5, 2)
    'Dim b As New Broad(a)
    Dim r As Broad

    Sub Main()
        Dim x& = &H8000000000000000

        b(1) = top64 : b(2) = top64 : b(-2) = 22 'assign some values to b. Can be used for testing Add/Subtract

        a(0) = 1 ' set value of a to 1 
        'the statement a(3) = 230 would mean value of a = 230 * (2 ^ 64) ^ 3)

        Broad.displayMode = Broad.displayModeType.inHex 'comment out this line to get the values in decimal


        'Dim bflt As New BFloat160(1/3)
        printValue(a, "A1")


        'Do multiply operation 6101 time 
        For i = 0 To 6100

            a.Multiply(5000001)
            'a = a + b ' <-- YOU MAY USE THIS, HOWEVER
            'a.Add(b) IS FASTER, SAME GOES FOR THE "*" OPERATOR VS. MULTIPLY FUNCTION

        Next

        printValue(a, "A2")

        For i = 0 To 6100 '000
            'a = a - b
            a.Divide(5000001)
            ' a.Subtract(CDbl(1 / 3))
        Next

        printValue(a, "A3") 'should print the initial value of 'a'
        'you can test it also with dividing first and then doing the multiplication

        'showValues()

        Console.WriteLine(" OK ")
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
