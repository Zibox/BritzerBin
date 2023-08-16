using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Host;

namespace BritzerBin
{
    [Cmdlet(VerbsCommon.New,"RunspacePool")]
    [OutputType(typeof(RunspacePool))]
    public class NewRunspacePool : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public InitialSessionState InitialSessionState { get; set; } = InitialSessionState.CreateDefault();

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true)]
        [ValidateRange(1,int.MaxValue)]
        public int MaxThreads { get; set; }
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            RunspacePool pool = RunspaceFactory.CreateRunspacePool(1,MaxThreads,InitialSessionState, this.Host);
            WriteObject(pool);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}