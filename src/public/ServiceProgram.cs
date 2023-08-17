using System;
using System.ServiceProcess;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PowerShellService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new PowerShellService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }

    public class PowerShellService : ServiceBase
    {
        private Runspace _runspace;
        private Thread _scriptThread;
        public PowerShellService()
        {
            ServiceName = "PowerShellService";
        }

        protected override void OnStart(string[] args)
        {
            _scriptThread = new Thread(RunspaceService);
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();

            using (PowerShell ps = PowerShell.Create())
            {
                ps.Runspace = _runspace;

                // Embed the PowerShell script within the verbatim string
                string script = @"
                $PSVersionTable | Out-File -path 'c:\temp\test.txt' -Append
                while ($true) {
                    'lol sup' | Out-File -Path 'c:\temp\test.txt' -Append
                    start-sleep 10
                }
                ";

                ps.AddScript(script);
                ps.Invoke();
            }
        }
        private void RunspaceService()
        {
            using
        }
        protected override void OnStop()
        {
            _runspace.Close();
            _runspace.Dispose();
        }
    }
}
