# BroadLib
A library which manipulates numbers of large size - 8 bytes to 16GB (upper limit not tested yet) . 

It works as a balanced numeral system with base of 2^64.

Types: 

<b>Broad</b> - a chained Int64 array with exponent representing value in the form of int64value^((2^64)^Exponent)
at creation, you can specify number of digits reserved for non-integral part of the number. 
 Changing this value is a lightweight operation, as the buffer remains the same.

<b>BFloat160</b> - a 160-bit floating point which consists of 128-bit mantissa (two signed QWords) and 32-bit exponent (a signed DWord) 

<h3>Prerequisites<h3>

