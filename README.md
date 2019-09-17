# Payment Calculator WPF

[![License](https://img.shields.io/github/license/void-type/Payment_Calculator_WPF.svg?style=flat-square)](https://github.com/void-type/Payment_Calculator_WPF/blob/master/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/PaymentCalculator.Wpf/5.svg?style=flat-square)](https://dev.azure.com/void-type/PaymentCalculator.Wpf/_build/latest?definitionId=5&branchName=master)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/PaymentCalculator.Wpf/5.svg?style=flat-square)](https://dev.azure.com/void-type/PaymentCalculator.Wpf/_build/latest?definitionId=5&branchName=master)
[![ReleaseVersion](https://img.shields.io/github/release/void-type/Payment_Calculator_Wpf.svg?style=flat-square)](https://github.com/void-type/Payment_Calculator_WPF/releases)

Payment Calculator is a loan amortization calculator made with C#.Net and WPF on .Net Core 3.0.

PaymentCalculator only runs on Windows at this time.

To run, download the latest [release](https://github.com/void-type/Payment_Calculator_WPF/releases) and run the PaymentCalculator.exe executable.

Releases 2.2.0 and later are single file applications. The portable version is larger, but only requires the single exe file run. The framework version requires the [.Net Core 3.0 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.0) to be installed.

Releases 2.0.0 to 2.1.X require the [.Net Core 3.0 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.0) to be installed.

Releases prior to 2.0.0 were built with .Net Framework 4.6.1 and should run on Windows 7 and later without any extra installs.

## Features

* Build an amortization schedule for loans and mortgages.
* See loan metrics like total interest paid and total paid over the lifetime of the loan.

This project uses the [VoidCore.Finance](https://github.com/void-type/VoidCore.Finance) library.

## Comments

This application has it's roots in Visual Basic class assignment I had in college. It was an amortization calculator written in VB.NET and WinForms.

I wanted to port that assignment to C#, but there wasn't a C# variant of the Microsoft.VisualBasic.Financial library, and .Net was not open source at the time.

So, I created this project. Within it, the beginnings of VoidCore.Financial took shape via reverse-engineering the financial functions in VB.NET and Excel. I ran into some errata in the VB implementation when I was first developing the library. I believe I worked around them by using decimal types and using some improved logic, so my implementation will not be a perfect emulation.

I can't guarantee it will be bug-free, but there are some unit tests for common use-cases.

## Developers

To work on Payment Calculator, you will need the [.Net Core 3.0 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0).

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
