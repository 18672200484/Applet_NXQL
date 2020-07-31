using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.UploadData.Entities
{
    public class PropertySet
    {
        public virtual String Source { get; set; }
        public virtual String Destination { get; set; }
        public virtual String DesType { get; set; }
        public virtual String DesLength { get; set; }
        public virtual String Description { get; set; }
        public virtual String DesPrimaryKey { get; set; }
        public virtual String Format { get; set; }
    }
}
