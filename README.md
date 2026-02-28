# iMS_Studio
iMS Studio .NET Application for interactive Image, Compensation and Tone Buffer Creation and Playback

## Build Process

1. Clone repository:

`git clone https://github.com/Isomet-Corporation/iMS_Studio.git`

2. Checkout `imslib-csharp` and `ims_hw_server` alongside and build both in Release mode:

`git clone --recurse-submodules https://github.com/Isomet-Corporation/imslib-csharp.git`

`git clone --recurse-submodules https://github.com/Isomet-Corporation/ims_hw_server.git`

Ensure both repos are sited alongside this one at the same depth:

```
<workspace>
    |
    |-<iMS_Studio>
    |-<imslib-csharp>
    |-<ims_hw_server>
```

That way the relative paths in the .csproj will line up correctly

3. Open iMS_Studio.sln in Visual Studio and load the Package Manager console:

Tools -> NuGet Package Manager -> Package Manager Console

4. Install Packages

`Update-Package -Reinstall`

5. Check in reference tab that all packages have downloaded and built (no warning symbols) or redo step 4.

6. Build Solution  