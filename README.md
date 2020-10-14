# OT Login Server

Web Login server is used to help you connect to your server supporting newest protcol version 12.
It was intended to use in development environment, so it may be buggy or not performant enough to suit production environments. Use at your own risk.

Technologies:
  - .Net Core 3.1
  - C#
  - MySql

## How To Compile
It's actually pretty simple.
Right click OTLoginServer project file and select publish, if you wish you can change some options there and just click publish.
That's it :)

## How To Use
It's even simpler, after you succesfully published, you gonna see new directory in your `bin` directory called `publish`.
Simply move your .exe file to your forgottenserver root directory (this one with config.lua in it)
That's it, just run it and enjoy. Make sure you `run` it as `administrator` though.
By default it will listen on `http://localhost:80/login/`, you can change this in Const.cs.

### Bugs
There is always room for improvement, so you if ran into any bugs or so, report them in issues tab, also pull requests are always welcome!
