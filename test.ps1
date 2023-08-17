function Get-Greeting {
    [CmdletBinding()]
    param(

    )
    begin {

    }
    process {
        return "Hello, world!"
    }
    end {

    }
}

$testSb = {
    Get-Greeting | Out-File -Path "c:\temp\rstest.txt" -append
    "testStr" | out-file -path "c:\temp\rstest.txt" -append
    $testVar | out-file -path "c:\temp\rstest.txt" -append
}
$loopSb = {
    while ($true) {
        Get-Greeting | Out-File -Path "c:\temp\rstest.txt" -Append
        Start-Sleep 15
    }
}
$moduleSb = {
    Import-Module "W:\source\BritzerBin\src\obj\Debug\net7.0\BritzerBin.dll"
    $pool = New-RunspacePool -MaxThreads 4
    Write-Output $pool
    $pool | Out-File "c:\temp\rstest.txt" -append
}
Import-Module "W:\source\BritzerBin\src\obj\Debug\net7.0\BritzerBin.dll"
$testVar = 'TestValue123'
$test = New-InitialSessionState -Variable 'testVar' -Function 'Get-Greeting'
$pool = New-RunspacePool -InitialSessionState $test -MaxThreads 10
$pool.Open()
$ps = [PowerShell]::Create()
$ps.AddScript($testSb)
$ps.RunspacePool = $pool
$ps.Invoke()
$ps3 = [powershell]::Create()
$ps3.AddScript($loopSb)
$ps3.RunspacePool = $pool
$ps3.BeginInvoke()
$ps4 = [Powershell]::Create()
$ps4.AddScript($moduleSb)
$ps4.RunspacePool = $pool
$ps4.BeginInvoke()