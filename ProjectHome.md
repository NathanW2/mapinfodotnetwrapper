# The Mapinfo .NET OLE Wrapper #
**The Mapinfo .NET OLE Wrapper** project aims to act as a .NET SDK for Mapinfo Professional.

<font color='red' size='5'><b>I have suspended development on this project. I have written about the reasons on my blog at: <a href='http://woostuff.wordpress.com/2010/12/15/mapinfo-net-wrapper-the-end/'>http://woostuff.wordpress.com/2010/12/15/mapinfo-net-wrapper-the-end/</a> </b></font>


After the release of version 1.0, I realized that some of the aspects of the wrapper were not really designed the best way and that version 1.0 was a bit of a rush job.  I am actively working on Version 2.0 which will undergo a lot more beta testing and designing.

I am currently writing the documentation for version 2.0 as I go in order to get feedback and to make sure version 2.0 is as good as possible.

Current documentation for version 2.0:
  * http://code.google.com/p/mapinfodotnetwrapper/wiki/EntityTypes

Any feedback on the design of version 2.0 would be great.


## A bit of background ##
I started on this project to aid my efforts when I was working with Mapinfo OLE and .NET together.  Mapinfo's COM object only exposes two commands _Do_ and _Eval_ which only accept and return strings.  After coding a few applications that used Mapinfo COM I realised that a lot of errors where being made with incorrectly formated strings and/or spelling mistakes which due to Mapinfo's error handling where very hard to track down and debug, and so I began work on the Mapinfo .NET OLE Wrapper.

As Pitney Bowes Mapinfo Professional 9.5 has the ability to call .NET methods from a Mapbasic program which has complied using Mapbasic 9.5, and .NET becomes increasing popular with the Mapinfo developer community, this wrapper will hopefully provide a type safe .NET SDK for those new and old to the Mapinfo automation world.

This project also aims to aid people who can't afford/or do not wish to purchase Mapxtreme but who would rather stay using Mapinfo but still achieve automation and development of Mapinfo in .Net without to much fuss.

**Check out the [quick start](QuickStart.md) for a quick run down.**

## Features ##
  * Provides **type safe** access to common Mapinfo OLE commands.
  * Can be used with **any version** of Mapinfo.
  * Allows users to easily automate Mapinfo without sending hundreds of string commands.
  * Makes your code look cleaner, no more String.Format just to add values to a Mapbasic command.
  * Allows for easier debugging of Mapinfo OLE applications.
  * Now has basic implementation of a LINQ provider for Mapinfo:

## Prerequisites ##
> ### To build ###
  * Microsoft Visual Studio 2008
  * .NET Framework 3.5

## More Info ##
  * Some code examples with and without the wrapper can be found on the [examples page](Examples.md).