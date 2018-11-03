using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.LogMySql.Model
{
    public class SoftWareInfo
    {
        public SoftWareInfo()
        {
            this.ProjectName = string.Empty;
            this.LineIndex = 0;
            this.SoftwareName = string.Empty;
        }

        public SoftWareInfo(string proj,int lineindex,string soft)
        {
            this.ProjectName = proj;
            this.LineIndex = lineindex;
            this.SoftwareName = soft;
        }
        public SoftWareInfo(SoftWareInfo other)
        {
            this.ProjectName = other.ProjectName;
            this.LineIndex = other.LineIndex;
            this.SoftwareName = other.SoftwareName;
        }

        public string ProjectName { get; set; }
        public int LineIndex { get; set; }
        public string SoftwareName { get; set; }
    }
}
