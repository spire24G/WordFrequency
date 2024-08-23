$varPathExe = "../../WordFrequencyApp/bin/Debug/net8.0/WordFrequencyApp.exe"
$varPathInput = "../input"
$varPathOutput = "../output"


$MonFolder = Get-ChildItem -Path $varPathInput -File | Where-Object {$_.Name -match 'txt$'} 
foreach ($MyFile in $MonFolder)
{
	& $varPathExe $($MyFile.FullName) "$varPathOutput/$($MyFile.Basename)result.txt"
}


