using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFileCleanUp
{
    class ScanTypeJson
    {
        public long Version { get; set; }
        public List<ModeView.ScanTypeModeView> ScanTypes { get; set; } = new List<ModeView.ScanTypeModeView>();
    }
}
