using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Tests.Helpers
{
    //
    // Summary:
    //     Attribute that is applied to a method to indicate that it is a fact that should
    //     be run by the test runner. It can also be extended to support a customized definition
    //     of a test method.
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    //[XunitTestCaseDiscoverer("Xunit.Sdk.FactDiscoverer", "xunit.execution.{Platform}")]
    public class FactAttribute : Attribute
    {
        public FactAttribute()
        {

        }

        //
        // Summary:
        //     Gets the name of the test to be used when the test is skipped. Defaults to null,
        //     which will cause the fully qualified test name to be used.
        public virtual string DisplayName { get; set; }
        //
        // Summary:
        //     Marks the test so that it will not be run, and gets or sets the skip reason
        public virtual string Skip { get; set; }
    }
}
