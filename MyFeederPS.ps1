#
#  Copyright 2023, Roger Brown
#
#  This file is part of Roger Brown's Toolkit.
#
#  This program is free software: you can redistribute it and/or modify it
#  under the terms of the GNU General Public License as published by the
#  Free Software Foundation, either version 3 of the License, or (at your
#  option) any later version.
# 
#  This program is distributed in the hope that it will be useful, but WITHOUT
#  ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
#  FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
#  more details.
#
#  You should have received a copy of the GNU General Public License
#  along with this program.  If not, see <http://www.gnu.org/licenses/>
#
# $Id: MyFeederPS.ps1 58 2023-03-07 23:55:28Z rhubarb-geek-nz $
#

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

$READER = Open-MyFeederPS

$LIST = Get-MyFeederPS -Reader $READER
$RESULT = @()

[byte[]] $SELECT=0,164,4,0,7,212,16,0,0,3,0,1
[byte[]] $READ=0,178,1,36,26

foreach ($NAME in $LIST) 
{ 
	$ATR = $null
	$ID = $null
	$BAL = $null
	
	try
	{
		Connect-MyFeederPS -Reader $READER -Name $NAME

		try
		{
			$ATR = Read-MyFeederPS -Reader $READER
			$ATR = [String]::Join("", ($ATR | % { "{0:X2}" -f $_}))

			$RES = Send-MyFeederPS -Reader $READER -Apdu $SELECT

			if ( $RES.Length -gt 2 )
			{
				$ID = [String]::Join("", ($RES[8..15] | % { "{0:X2}" -f $_}))

				$BAL = 0

				$RES = Send-MyFeederPS -Reader $READER -Apdu $READ

				if ( $RES.Length -gt 2 )
				{
					foreach ($B in $RES[2..5])
					{
						$BAL=($BAL * 256)+$B
					}
				}

				$BAL = ($BAL/100).ToString("c")
			}
			else
			{
				$ID = [String]::Join("", ($RES | % { "{0:X2}" -f $_}))
			}	
		}
		finally
		{
			Disconnect-MyFeederPS -Reader $READER
		}
	}
	catch
	{
	}

	$RESULT += [PSCustomObject]@{
		NAME=$NAME
		ATR=$ATR
		ID=$ID
		BAL=$BAL
	}
}

Close-MyFeederPS -Reader $READER

$RESULT | Format-Table
