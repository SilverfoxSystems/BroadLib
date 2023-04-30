'IF YOU INTEND TO COMPILE FROM SCRATCH, MAKE SURE THAT YOU DISABLE "INTEGER OVERFLOW CHECKS" UNDER SETTINGS/Compile/Advanced Compile options

Option Explicit Off
Imports System.Runtime.InteropServices


Public Class Broad
	'(scoped) Dim LowestDigitInUse%, HighestDigitInUse%
	Friend buff() As Long, del% ', BufferSize%
	Public Shared maxDigitsInString% = 32
	' Dim szLimit% ' = -1
	Friend Shared AutoSize As AutoSizeMode ' Boolean = True
	Friend Shared IgnoreLimit As Boolean = True

	<DllImport("broadOPs.dll", EntryPoint:="cmpsqProc")> Private Shared Function _
 cmpsq%(ByVal cnt%, ByRef a&, ByRef b&)
	End Function
	<DllImport("broadOPs.dll", EntryPoint:="findTopDigit")> Private Shared Function _
 findTopDigit%(ByVal cnt%, ByRef a&)

	End Function
	<DllImport("broadOPs.dll", EntryPoint:="Add128")> Private Shared Function _
 add128%(ByRef owordLowPtr&)
	End Function
	<DllImport("broadOPs.dll", EntryPoint:="AddQW")> Private Shared Function _
 add64%(ByVal Value&)
	End Function

	<DllImport("broadOPs.dll", EntryPoint:="divInt64")> Private Shared _
  Function divInt64%(ByVal val&, ByRef refOut&, ByVal rngPak&)
		'Function divInt64%(ByVal val&, ByRef refOut&, ByRef ref1&, ByVal rngPak&)
		'Function divInt64%(ByVal val&, ByRef refOut&, ByRef ref1&, ByVal range%, ByVal ostRng%)
	End Function

	<DllImport("broadOPs.dll", EntryPoint:="mulInt64")> Private Shared _
  Function mulInt64%(ByVal val&, ByRef refOut&, ByVal range%)
		'Function mulInt64%(ByVal val&, ByRef refOut&, ByRef ref1&, ByVal range%)


	End Function

	<DllImport("broadOPs.dll", EntryPoint:="AddXL")> Private Shared _
 Function AddXL%(ByRef ref1&, ByRef ref2&, dsz%, sz2&) ', Optional byteOffs% = 0)


	End Function
	<DllImport("broadOPs.dll", EntryPoint:="SubXL")> Private Shared _
 Function SubXL%(ByRef ref1&, ByRef ref2&, dsz%, sz2&) ', Optional byteOffs% = 0)


	End Function
	Public Enum AutoSizeMode
		None
		Low
		High
		Both
	End Enum

	Enum CompState
		isBigger = 1
		isEqual = 0
		isLower = -1
	End Enum

	Enum PrecisionDef
		AsDividend = -1
	End Enum


	Public Shared QuotientPrecision As PrecisionDef = -1


	Public Sub Divide(ByVal Divisor&, Optional ByVal DesiredPrecision% = -1) ' As Broad  ' As Boolean
		Select Case Divisor
			Case 1
			Case -1
				' Return Me * (-1)
				Me.Multiply(-1)
			Case 0
				Throw New Exception("Division by zero (broad)")

		End Select



		ddel% = 0
		rngPk& = 0

		Select Case DesiredPrecision
			Case -1
				ddel = 0
			Case Else
				ddel% = DesiredPrecision - del ' xl0.PartSize
		End Select ' If

		l% = BufferSize + ddel

		' If ddel >= 0 Then
		nDel% = del + ddel

		If ddel >= 0 Then
			rngPk = rngPk Or ddel
			rngPk <<= 32
			rngPk = rngPk Or BufferSize
		Else
			rngPk = l ' xl0.BufferSize + ddel


		End If




		divInt64(Divisor, buff(UBound(buff)), rngPk)


	End Sub




	Public Sub Multiply(m&) ' As Broad  ', Optional startOffset% = 0, Optional length% = -1) As broad

		If buff(UBound(buff)) Then
			If AutoSize.High And AutoSize Then
				ReDim Preserve buff(UBound(buff) + 1)
			ElseIf Not IgnoreLimit Then
				Throw New Exception("Topmost digit is non-zero.")

			End If

		End If
		mulInt64(m, buff(0), buff.Length)

	End Sub

	Public Function MultiplyBFlt160(ByRef bf As BFloat160) As Broad  ', Optional startOffset% = 0, Optional length% = -1) As broad
		'value& bo bf160
		Dim xl As Broad '(cel)
		'  ReadPtr0% = 0
		outOffsIX% = 0
		'If bf.hiQword Then
		With bf
			cel% = BufferSize - del
			nDel% = - .exp64
			If .lowQword Then nDel += 1


			If nDel > 0 Then
				'	If nDel > del Then


				'   End If
				'cel2% = cel - nDel
				'-cel -= (nDel - 1)
				'-If cel <= 0 Then
				'-cel = 1
				'-End If
				'	outOffsIX = nDel - del
				'nDel += del
				If del > nDel Then nDel = del
				'total1% = nDel + cel
				xl = New Broad(cel + nDel, nDel)
				'outoffs=0

			ElseIf cel <= .exp64 Then

				outOffsIX = .exp64
				'total2% = BufferSize + .exp64 ' + del
				'total2% = cel + .exp64 + del
				xl = New Broad(BufferSize + .exp64, del)
				'xl = New broad(outOffsIX + 1, del)
				'xl = New broad(del + .exp64 + 1, del)

			Else
				outOffsIX = .exp64

				xl = New Broad(BufferSize + .exp64, del)
				'	xl = New broad(Me)

			End If



			If .lowQword Then
				'-mulInt64(.lowQword, xl.buff(outOffsIX), buff(0), buff.Length)
				If .hiQword Then
					Dim xl1 As New Broad(xl.BufferSize, xl.PartSize)

					'-mulInt64(.hiQword, xl1.buff(outOffsIX + 1), buff(0), buff.Length)
					xl.Add(xl1)
				End If
			ElseIf .hiQword Then
				'-mulInt64(.hiQword, xl.buff(outOffsIX), buff(0), buff.Length)
				' Else
				'xl = New broad()
			End If

		End With



		'  End If
		Return xl

	End Function


	Public Shared Function Compare%(ByRef xl0 As Broad, ByRef xl1 As Broad) ' As Boolean

		r% = 0
		p0% = Find1stNZ(xl0) : p1% = Find1stNZ(xl1)
		'pd0% = p0 - del : pd1% = p1 - xl1.PartSize
		pd0% = p0 - xl0.PartSize : pd1% = p1 - xl1.PartSize

		If pd0 > pd1 AndAlso xl0.buff(0) Then
			r = resolveFor(xl0.buff(p0))
		ElseIf pd0 < pd1 AndAlso xl1.buff(0) Then
			r = -resolveFor(xl1(p1))
		Else '===jenako exp
			ddel% = xl1.PartSize - xl0.PartSize

			n% = 1

			' Dim xl As broad

			If ddel < 0 Then
				n += p0
				xl = xl0
				ddel = -ddel


				r = cmpsq(n, xl0.buff(p0), xl1.buff(p1))

				If r = 0 Then
					If ddel Then
						r = -resolveFor(xl0.buff(findTopDigit(ddel, xl0.buff(ddel - 1))))

						'resolvefor(xl.buff(Find1stNZ(xl.buffddel))))
						'	Else

						' Return 0
						'Exit Function
					End If

				End If



			Else

				'xl = xl1
				n += p1

				r = cmpsq(n, xl0.buff(p0), xl1.buff(p1))

				If r = 0 Then
					If ddel Then
						r = resolveFor(xl1.buff(findTopDigit(ddel, xl1.buff(ddel - 1))))

					End If

				End If
			End If

		End If

		Return -r
	End Function



	Shared Function resolveFor%(i&)

		If i > 0 Then
			Return 1
		ElseIf i < 0 Then
			Return -1
		Else
			Return 0
		End If
	End Function


	Public Shared Function Find1stNZ%(ByRef xl As Broad, Optional startIX% = -1) 'ByRef xl As broad)

		If startIX = -1 Then
			Return findTopDigit(xl.BufferSize, xl.buff(UBound(xl.buff)))
		Else
			Return findTopDigit(startIX + 1, xl.buff(startIX))
		End If

	End Function




	''' <summary>
	''' Initialize a new xxl based on
	''' </summary>
	''' <param name="xl"></param>
	Public Sub New(ByRef xl As Broad)
		With xl

			buff = .buff.Clone
			'  BufferSize = .BufferSize
			del = .del

		End With
	End Sub


	Public Sub New(ByVal SizeInQWords%, ByVal Optional szPart As Integer = 0, Optional SizeLimit% = -1, Optional AutoIncrSize As AutoSizeMode = AutoSizeMode.Both)
		del = szPart

		ReDim buff(SizeInQWords - 1)
		AutoSize = AutoIncrSize
	End Sub
	Public Sub Add(Nr As Long, Exp%)
		'if exp<cel

	End Sub
	'Private lastHiPtr As ULong

	Public Shared Function SubX%(ByRef xl0 As Broad, ByRef xl1Ptr&, cnt1%, AtOffset%) ' As broad
		'Public  Function Add(ByRef xl1Ptr&, cnt1%, AtOffset%) As broad

		r% = 0

		With xl0
			''start% = del - .del
			topPtr1% = AtOffset + cnt1

			If AtOffset >= 0 Then

				' Dim expandHigh As Boolean = AutoSize And AutoSizeMode.High

				If topPtr1 > .BufferSize Then
					If (AutoSize And AutoSizeMode.High) Then
						ReDim Preserve .buff(topPtr1 - 1)
					Else
						Throw New Exception("Subtraction out of scope of the destination operand.")
					End If

				End If

				If CBool(.buff(UBound(.buff))) Then 'AndAlso 

					If (AutoSize And AutoSizeMode.High) Then
						ReDim Preserve .buff(.BufferSize)
					ElseIf Not IgnoreLimit Then
						Throw New Exception("Upper digit is non-zero (result operand).")
					End If
				End If

				r = SubXL(.buff(AtOffset), xl1Ptr, .BufferSize, cnt1)


				Return r 'New broad(Me)
				'Return Me


			Else
				'... incr del
				If (AutoSize And AutoSizeMode.Low) Then
					'ReDim Preserve buff(topPtr1 - 1)

					n% = -AtOffset + .BufferSize ' UBound(buff)

					Dim xl As New Broad(n)
					'buff.
					.buff.CopyTo(xl.buff, -AtOffset)
					.buff = xl.buff

					r = SubXL(.buff(0), xl1Ptr, .BufferSize, cnt1)


					'buff.Prepend()
					Return r ' xl ' Me

				Else
					Throw New Exception("Subtraction out of scope of the result operand.")
				End If


			End If
		End With


	End Function

	Public Shared Function AddX%(ByRef xl0 As Broad, ByRef xl1Ptr&, cnt1%, AtOffset%) ' As broad
		'Public  Function Add(ByRef xl1Ptr&, cnt1%, AtOffset%) As broad

		r% = 0

		With xl0
			''start% = del - .del
			topPtr1% = AtOffset + cnt1

			If AtOffset >= 0 Then

				'	 Dim expandHigh As Boolean = AutoSize And AutoSizeMode.High

				If topPtr1 > .BufferSize Then
					If (AutoSize And AutoSizeMode.High) Then
						ReDim Preserve .buff(topPtr1 - 1)
					Else
						Throw New Exception("Addition out of scope of the result operand.")
					End If

				End If

				If CBool(.buff(UBound(.buff))) Then 'AndAlso 

					If (AutoSize And AutoSizeMode.High) Then
						ReDim Preserve .buff(.BufferSize)
					ElseIf Not IgnoreLimit Then
						Throw New Exception("Upper digit is non-zero (result operand).")
					End If
				End If

				r = AddXL(.buff(AtOffset), xl1Ptr, .BufferSize, cnt1)


				Return r 'New broad(Me)
				'Return Me


			Else
				'... incr del
				If (AutoSize And AutoSizeMode.Low) Then
					'ReDim Preserve buff(topPtr1 - 1)

					n% = -AtOffset + .BufferSize ' UBound(buff)

					Dim xl As New Broad(n)
					'buff.
					.buff.CopyTo(xl.buff, -AtOffset)
					.buff = xl.buff

					r = AddXL(.buff(0), xl1Ptr, .BufferSize, cnt1)


					'buff.Prepend()
					Return r ' xl ' Me

				Else
					Throw New Exception("Addition out of scope of the result operand.")
				End If


			End If
		End With


	End Function
	'Public Function Add(ByRef xl As broad) As broad
	Public Function Subtract%(ByRef xl As Broad) ' As broad
		r% = SubX(Me, xl.buff(0), xl.BufferSize, del - xl.del)
		Return r

	End Function


	'End Function
	Public Function Add%(ByRef xl As Broad) ' As broad
		r% = AddX(Me, xl.buff(0), xl.BufferSize, del - xl.del)
		Return r
	End Function


	'  Public Sub Resize(szTotal%, szPart%)
	Public Sub Round(Precision%)

		If Precision > del Then
			'	Dim xl As New Broad(BufferSize, del)
			ddel% = Precision - del
			ReDim Preserve buff(UBound(buff) + ddel)
			For i% = UBound(buff) - ddel To 0 Step -1
				buff(i + ddel) = buff(i)
				buff(i) = 0
			Next
			del = Precision

		ElseIf Precision < del Then

			ddel% = Precision - del
			For i% = -ddel To UBound(buff) 'Step -1
				buff(i + ddel) = buff(i)
				buff(i) = 0
			Next


			ReDim Preserve buff(UBound(buff) + ddel)
			del = Precision

		End If


		'buff.
		'  .buff.CopyTo(xl.buff, -AtOffset)
		' .buff = xl.buff

	End Sub
	Friend Function AlterPartSize%(NewSize%, Optional preserveValue As Boolean = True)

	End Function
	Friend Function ResizeBuffHigh%(NewSize%, Optional preserveValue As Boolean = True)
		'Friend Function Resize%(NewSize%, Optional preserveValue As Boolean = True)

	End Function
	Friend Function ResizeLow%(NewSize%, Optional preserveValue As Boolean = True)

	End Function

	Public Function Add%(ByRef BFloat As BFloat160) ' As broad

		Return AddX(Me, BFloat.lowQword, 2, del + BFloat.exp64)


	End Function


	''' <summary>
	''' Adds a signed Long number type at specified exponent
	''' </summary>
	''' <param name="Nr">The Float64 value to be added</param>
	''' <returns> If number returned is non-negative, then it represents number of places left until the broad's upper bound is reached, 0 means that the last digit was modified. If result is negative: -1 when overflow with positive remainder and -2 when overflow with negative remainder. </returns>
	Public Sub Add(Nr#) ' As Broad


		Dim b As New BFloat160(Nr)
		AddX(Me, b.lowQword, 2, del + b.exp64)

	End Sub

	Public Shared Widening Operator CType(a#) As Broad

	End Operator
	Public Shared Narrowing Operator CType(xl As Broad) As Double


	End Operator
	'Public Shared Operator <>(a As Broad, b#) As Boolean

	'End Operator

	'Public Shared Operator =(a As Broad, b#) As Boolean
	' MsgBox("Not yet implemented.")
	'End Operator



	Public Shared Operator <>(a As Broad, b As Broad) As Boolean
		Return CBool(Compare(a, b))

	End Operator

	Public Shared Operator =(a As Broad, b As Broad) As Boolean
		Return Not CBool(Compare(a, b)) '(Compare(a, b) = 0)

	End Operator

	Public Shared Operator <=(a As Broad, b As Broad) As Boolean
		Return (Compare(a, b) <= 0)

	End Operator
	Public Shared Operator >=(a As Broad, b As Broad) As Boolean
		Return (Compare(a, b) >= 0)

	End Operator


	Public Shared Operator <(a As Broad, b As Broad) As Boolean
		Return (Compare(a, b) < 0)

	End Operator
	Public Shared Operator >(a As Broad, b As Broad) As Boolean

		Return (Compare(a, b) > 0)

	End Operator

	Public Shared Operator <(a As Broad, f#) As Boolean

	End Operator
	Public Shared Operator >(xl As Broad, f#) As Boolean

	End Operator

	Public Shared Operator /(a As Broad, LngInt&) As Broad
		'Return a.Divide(b, QuotientPrecision)
		Dim xl As New Broad(a)
		xl.Divide(LngInt)
		Return xl
	End Operator


	Public Shared Operator *(a As Broad, LngInt&) As Broad
		Dim xl As New Broad(a)
		xl.Multiply(LngInt)
		Return xl
		'Return a.Multiply()
	End Operator
	Public Shared Operator *(a As Broad, d#) As Broad
		'Return a.Multiply(BFloat160.GetBFloat(d))
	End Operator



	Public Shared Operator *(a As Broad, b As BFloat160) As Broad
		' Return a.Multiply(b)
	End Operator
	Public Shared Operator +(a As Broad, b As BFloat160) As Broad
		Dim r As New Broad(a)
		r.Add(b)
		Return r
		'AddX(r, b.lowQword, 2, -b.exp64)
	End Operator
	Public Shared Operator -(a As Broad, b As Broad) As Broad
		Dim r As New Broad(a)
		r.Subtract(b)
		Return r

	End Operator
	Public Shared Operator +(a As Broad, b As Broad) As Broad
		Dim r As New Broad(a)
		r.Add(b)
		Return r

	End Operator

	'Public Shared Operator <<(a As Broad, n%) As Broad
	Public Shared Operator >>(a As Broad, n%) As Broad

	End Operator


	Public Property Precision%()
		Get
			Return del
		End Get
		Set(value%)
			Round(value)
		End Set
	End Property

	Public Property Buffer&(Indx%)
		Get
			Return buff(Indx)
		End Get
		Set(value&)
			buff(Indx) = value
		End Set
	End Property ' Length%
	'Public ReadOnly Property BufferSize% ' Length%
	Public ReadOnly Property BufferSize% ' Length%
		Get
			Return buff.Length
		End Get
	End Property


	''' <summary>
	''' Decreasing this value by 1 mathematically equates to multiplication by 2 ^ 64. Cannot be negative.
	''' </summary>
	''' <returns>Number of digits reserved for non-integral part of the number</returns>
	Public Property PartSize%
		'Public Property Precision%
		'Public Property PoweredBy%
		Get
			'TotalSize(
			Return del
		End Get
		Set(value%)
			If value >= 0 Then

				If value < BufferSize Then
					del = value
				Else
					Throw New Exception("Value must be lower than buffer size.")
				End If

			Else
				Throw New Exception("Value cannot be negative.")

			End If


		End Set
	End Property

	'Public Function SetSize%(Optional ExpandLeft As Boolean = True)

	'End Function
	Default Public Property DigitAt(Exponent%) As Long

		Get
			Return buff(del + Exponent)
		End Get
		Set(value As Long)
			buff(del + Exponent) = value
		End Set

	End Property



	Public Property TotalSize%() 'Optional FromBottom As Boolean )

		Get

			'Dim xl As broad
			'   f = xl(5)

			Return buff.Length
		End Get
		Set(value%)
			ReDim Preserve buff(value - 1)

		End Set


		'  End Set
	End Property


	Public Shared Function LTrim(ByRef xl As Broad) As Broad
		With xl
			ReDim Preserve .buff(findTopDigit(.BufferSize, .buff(UBound(.buff))))
		End With

	End Function
	Public Shared Function RTrim(ByRef xl As Broad, Optional nSkippedInitialDigits% = 0) As Broad

	End Function

	Public Shared Function Trim(ByRef xl As Broad) As Broad
		RTrim(xl)
		LTrim(xl)

	End Function

	' = False


	Public Shared displayMode As displayModeType

	Public Enum displayModeType
		inDecimal
		inHex

	End Enum

	Public Overrides Function ToString$()

		s$ = "" : i% = 0 : xx% = 0
		bf1& = 0
		f% = findTopDigit(BufferSize, buff(UBound(buff)))

		'   endd% = UBound(buff) - (2 * maxDigitsInString)
		endd% = f - (2 * maxDigitsInString)
		If endd < 0 Then endd = 0

		n% = 0
		If displayMode = displayModeType.inDecimal Then
			For i% = f To endd Step -1
				'For i% = UBound(buff) To endd Step -1
				xx% = i - del
				bf1 = buff(i)
				If bf1 Then
					If bf1 > 0 Then
						If xx >= 0 Then

							s += " +" + buff(i).ToString + "e+" + (xx).ToString + vbCrLf
						Else
							s += " +" + buff(i).ToString + "e" + (xx).ToString + vbCrLf
						End If
					Else
						If xx >= 0 Then

							s += " " + buff(i).ToString + "e+" + (xx).ToString + vbCrLf
						Else
							s += " " + buff(i).ToString + "e" + (xx).ToString + vbCrLf
						End If

					End If
					n += 1
					If n > maxDigitsInString Then
						s += " + ..."
						Exit For
					End If

				End If
			Next


		Else


			For i% = f To endd Step -1
				'For i% = UBound(buff) To endd Step -1
				bf1 = buff(i)
				If bf1 Then
					If bf1 > 0 Then
						s += "(" + (i - del).ToString + ")" + vbTab + Hex(buff(i)) + vbCrLf '" E" + (i - del).ToString + vbCrLf
					Else
						s += "(" + (i - del).ToString + ")" + vbTab + Hex(buff(i)) + vbCrLf ' + " E" + (i - del).ToString + vbCrLf

					End If


				End If
			Next
		End If

		'   If i >= endd Then
		'   s += " + ..."

		'   End If


		'  MsgBox(Hex(-1))

		If Len(s) Then

			Return s$
		Else
			Return "0"
		End If


	End Function



End Class

'End Class

<StructLayout(LayoutKind.Sequential)>
Public Structure BFloat160 ' BFloat160 ' Balanced128

	Dim exp64%
	Dim Reserved% 'unused
	Dim lowQword&
	Dim hiQword&

	Public Sub New(d#)
		With GetBFloat(d)
			exp64 = .exp64
			lowQword& = .lowQword&
			hiQword = .hiQword
		End With
	End Sub

	'Operator ctype()
	'Public Shared Widening Operator CType(a#) As broad

	'End Operator

	Public Overrides Function ToString() As String
		If Broad.displayMode = Broad.displayModeType.inDecimal Then

			Return hiQword.ToString + " " & lowQword & "e" & exp64
		Else
			Return "(" & exp64 & ") " & Hex(hiQword) + " " & Hex(lowQword)


		End If
	End Function

	Public Shared Function GetBFloat(ByVal Dbl#) As BFloat160 ' Balanced128
		'Public Shared Function getFromFloat64(ByVal dbl#) As BFloat160 ' Balanced128

		Dim a As New BFloat160 ' BFloat160 ' Balanced128

		'With a

		'.exp64 = getBigMoves(dbl)
		ConvertFloat64(Dbl, a)

		'  End With

		Return a


		' l;Return ConvertFloat64(dbl)


	End Function

	<DllImport("broadOPs.dll", EntryPoint:="convertDblTo128")> Private Shared Function _
 ConvertFloat64%(ByRef a#, ByRef BFloat160 As BFloat160)
		'ConvertFloat64%(ByVal a#, ByRef BFloat160 As BFloat160)
	End Function

End Structure


