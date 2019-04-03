# Payment Calculator WPF

Payment Calculator is a loan amortization calculator made with C#.Net and WPF on .Net Core 3.0.

## Features

This project also contains the VoidType financial library, which is similar to the Microsoft.VisualBasic.Financial class library. It improves upon the VB.Net library by utilizing decimal number types for greater accuracy. There are double type overloads to remain API-compatible with the VB.Net Financial class. There is also a wrapper and interface that can be utilized in other projects to convert the static class for dependency injection.

Once compiled, the single, portable executable is around 750KB.

## Comments

This application has it's roots in Visual Basic class assignment I had in college. I wanted to port that assignment to C#, but there wasn't a compatible financial library available for C# at the time. Also, .Net was not open source.

I replicated the Microsoft.VisualBasic.Financial class based on knowledge from my finance classes and reverse-engineering financial functions in Excel and VB.Net. I ran into some errata in the VB implementation when I was first developing the library. I believe I worked around them by using decimal types and using some improved logic, so my implementation will not be a perfect emulation.

Refer to the license for appropriate use of the application or it's source code. I can't guarantee it will be bug-free, but there are some unit tests for common use-cases.
