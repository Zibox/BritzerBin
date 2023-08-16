Add-Type -TypeDefinition @"
using System;
using System.Management.Automation;

public static class FunctionExtractor
{
    public static string ExtractFunctionFromCallerSession(string functionName)
    {
        using (var powershell = PowerShell.Create(RunspaceMode.CurrentRunspace))
        {
            powershell.AddScript($"$function:{functionName}.ScriptBlock.ToString()");
            var result = powershell.Invoke();
            if (result.Count > 0)
            {
                return result[0].ToString();
            }
        }
        return null;
    }
}
"@ -Language CSharp

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
$ps3.AddScript('while($true){Get-Greeting | Out-File -Path "c:\temp\rstest.txt;start-sleep 5}')
$ps3.RunspacePool = $pool
$ps3.BeginInvoke()


