# MyFeederPS

Read a `SmartCard` using `PowerShell`

Build using

```
dotnet build MyFeederPS.csproj --configuration Release
```

Install by copying contents of `bin/Release/netstandard2.1` into a new `MyFeederPS` directory on the [PSModulePath](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_psmodulepath)

Run the `test.ps1` to confirm it works.

```

NAME                                 ATR                                          ID               BAL
----                                 ---                                          --               ---
Generic Usb Smart Card Reader 0      3B6900FF4A434F503331563232                   1010000001782812 $14.73
Microsoft IFD 0
Microsoft UICC ISO Reader 3f873476 0 3B9F96801FC78031E07234238ABBD22324108D450342

```
