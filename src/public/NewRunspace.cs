using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

namespace BritzerBin
{
    [Cmdlet(VerbsCommon.New, "Runspace")]
    [OutputType(typeof(Runspace))]
    public class NewRunspace : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public InitialSessionState InitialSessionState { get; set; } = InitialSessionState.CreateDefault();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            Runspace runspace = RunspaceFactory.CreateRunspace(this.Host,InitialSessionState);
            WriteObject(runspace);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}