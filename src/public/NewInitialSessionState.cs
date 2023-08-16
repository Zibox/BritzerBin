using System;
using System.Threading;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace BritzerBin
{
    [Cmdlet(VerbsCommon.New,"InitialSessionState")]
    [OutputType(typeof(InitialSessionState))]
    public class NewInitialSessionState : PSCmdlet
    {
        
        [Parameter(Mandatory = false, Position = 0, ValueFromPipelineByPropertyName = true)]
        public string[]? Variable { get; set; }

        [Parameter(Mandatory = false, Position = 1, ValueFromPipelineByPropertyName = true)]
        public string[]? Function { get; set; }

        [Parameter(Mandatory = false, Position = 2, ValueFromPipelineByPropertyName = true)]
        public string[]? Assembly {get;set;}

        [Parameter(Mandatory = false, Position = 3, ValueFromPipelineByPropertyName = true)]
        public string[]? Module {get;set;}
        
        [Parameter(Mandatory = false,Position = 4,ValueFromPipelineByPropertyName = true)]
        public string[]? StartupScript {get;set;}
         
        [Parameter(Mandatory = false,Position = 5,ValueFromPipelineByPropertyName = true)]
        public PSThreadOptions ThreadOptions {get;set;} = PSThreadOptions.Default;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            InitialSessionState returnIss = InitialSessionState.CreateDefault();
            returnIss.ThreadOptions = ThreadOptions;
            if (Variable != null){
                PSVariableIntrinsics vs = SessionState.PSVariable;
                foreach (string _var in Variable)
                {
                    PSVariable psVar = vs.Get(_var);
                    if (psVar != null){
                        returnIss.Variables.Add(new SessionStateVariableEntry(_var, psVar.Value, null));
                    }
                }
            }
            if (Function != null )
            {
                foreach (string function in Function){
                    string? definition = ExtractFunctionDefinition(function);
                    if (definition != null){
                        returnIss.Commands.Add(new SessionStateFunctionEntry(function, definition));
                    }
                }
            }
            if (Assembly != null)
            {
                foreach (string assembly in Assembly){
                    returnIss.Assemblies.Add(new SessionStateAssemblyEntry(assembly));
                }
            }
            if (Module != null)
            {
                returnIss.ImportPSModule(Module);
            }
            if (StartupScript != null)
            {
                foreach(string startupScript in StartupScript)
                {
                    returnIss.StartupScripts.Add(startupScript);
                }
            }
            WriteObject(returnIss);
        }
        protected override void EndProcessing()
        {
            base.StopProcessing();
        }
        private static string? ExtractFunctionDefinition(string functionName)
        {
            using (var powershell = PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                powershell.AddScript($"(${{function:{functionName}}}).ToString()");
                var result = powershell.Invoke();
                if (result.Count > 0)
                {
                    return result[0].ToString();
                }
                return null;
            }
        }
    }
}
