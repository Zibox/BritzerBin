using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace BritzerBin
{
    [Cmdlet(VerbsCommon.New,"PwshService")]
    [OutputType(typeof(void))]
    public class NewPwshService
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    }
}