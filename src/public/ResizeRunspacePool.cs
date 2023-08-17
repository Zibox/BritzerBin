using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace BritzerBin
{
    [Cmdlet(VerbsCommon.Resize,"RunspacePool")]
    [OutputType(typeof(void))]
    public class ResizeRunspacePool : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateRange(2,32)]
        public int MaxThreads { get; set; }
        [Parameter(Mandatory = true, Position = 1)]
        public RunspacePool? RunspacePool {get;set;}
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            if (RunspacePool == null)
            {
                throw new PSArgumentNullException();
            }
            else
            {
                base.ProcessRecord();
                RunspacePool.SetMaxRunspaces(MaxThreads);
                return;
            }
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}