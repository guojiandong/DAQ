using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.InfluxDbClient.Model
{
    public class SoftWareInfo
    {
        private const string DEFAULT = "_Default";
        public SoftWareInfo()
        {
            this.ProjectName = DEFAULT;
            this.LineIndex = 0;
            this.SoftwareName = DEFAULT;
        }

        public SoftWareInfo(string proj, int lineindex, string soft)
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
