@{
	RootModule = 'MyFeederPS.dll'
	ModuleVersion = '0.2'
	GUID = 'fc4dd3fa-582f-45ed-9c01-de73f245e3cc'
	Author = 'Roger Brown'
	CompanyName = 'rhubarb-geek-nz'
	Copyright = '(c) Roger Brown. All rights reserved.'
	FunctionsToExport = @()
	CmdletsToExport = @(
		'Open-MyFeederPS',
		'Get-MyFeederPS',
		'Close-MyFeederPS',
		'Connect-MyFeederPS',
		'Disconnect-MyFeederPS',
		'Read-MyFeederPS',
		'Send-MyFeederPS'
	)
	VariablesToExport = '*'
	AliasesToExport = @()
	PrivateData = @{
    	PSData = @{
	    }
	}
}
