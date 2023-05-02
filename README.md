# BroadLib
A library which manipulates numbers of large size - anywhere from 8 bytes to 16GB (upper limit not tested yet) . 

It works as a balanced numeral system with base of 2^64.

<h3>Dependency library</h3>

In order for this build to work, the <i>BroadOPs.dll</i> file needs to be downloaded from http://izi.wtf/BroadOPs and then placed into the root solution directory (with <i>Copy to output path</i> option enabled).  Of course, there are other ways of implementing the library, like for instance copying the BroadOPs.dll file into the system32 folder if you're planning to use it in many projects.
This is because BroadOPs.dll has a special licence to it.

<h3>Types</h3> 

<b>Broad</b> - a chained Int64 array with exponent representing value in the form of int64value^((2^64)^Exponent)
at creation, you can specify number of digits reserved for non-integral part of the number. 

<b>BFloat160</b> - a 160-bit floating point which consists of 128-bit mantissa (two signed QWords) and 32-bit exponent (a signed DWord).
A <i>Double</i> must be converted to <i>BFloat</i> before it can be added to a <i>Broad</i>.  This conversion is done automatically.

<h3>Broad type usage and methods</h3>

You can initialize a Broad with:
- the value of a <i>Double</i> or add a <i>Double</i> to a Broad. <code>Dim xl as New Broad(DblPrecisionNr As Double)</code>
- the value of another Broad. <code>Dim xl as New Broad(xl1 as Broad)</code>
- Zero value. Specify <code>New(TotalBufferSize, nDigitsInPart)</code>. Both arguments are 32-bit integers and do not represent size in bytes, but in 8-byte size. Part size must be greater or equal to 0 and less than TotalBufferSize.

Addition/Subtraction Operation
You may use the "+" and "-" operators to add two Broad variables, something like <code>c = a + b</code>
Also a BFloat, a Double or an Int64 can be added to a Broad type.

Multiplication/Division 

Multiplication and division by an Int64 is supported.


While operators +, -, * and / offer a convenient way, performance-wise, the <b>Add</b>,<b>Subtract</b>,<b>Multiply</b> and <b>Divide</b> methods are faster as they don't require extra memory to be allocated.  The calculations are performed directly on the existing digits.
Operators are not fully implemented yet, however, the abovementioned procedures for basic calculations should work fine.

The code in "Broad.vb" and "starter.vb" projects are quite self-explanatory and you can easily tweak it according to your needs.
Where ever you see <i>Exponent</i> or <i>exp64</i> in code, this means (2<sup>64</sup>)<sup>Exponent</sup> 

ReadOnly Property <b>BufferSize</b> - Returns total number of digits

Property <b>PartSize</b> - Returns number of 64-bit digits used for non-integral part of number. Changing this value is a lightweight operation, as the buffer remains the same.  Must be less than total buffer size.

Property <b>Buffer</b> As Long - Access the digit in buffer by index

 <b>Round</b> method - Adjusts PartSize to the given number.  Offers zero rounding error due to the balanced numeral system.

<b>LTrim</b>(ByRef xl As Broad) - Strips leading zeroes
...

<h3>Broad type description</h3>

Numbers are represented either as integers (when <i>PartSize</I>=0), i.e. 0 1 2 3 0 0 (assuming that every digit is a 64-bit integer) or an integer+digits after decimal point. Minimum count of digits used in the integral part of the number is 1. So a very small number would have, for example, these 64-bit digits written in the memory buffer:  0 . 0 0 0 0 1 2 3 4
The most significant digit must always be zero, the size adjustments are automatically made prior to the mathematical operations if the AutoSize property is set to True (as by default).
