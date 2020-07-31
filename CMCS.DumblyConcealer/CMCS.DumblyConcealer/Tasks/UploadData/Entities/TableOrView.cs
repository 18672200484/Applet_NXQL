using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.UploadData.Entities
{
    public class TableOrView
    {
        public virtual String Source { get; set; }
        public virtual String Destination { get; set; }
        public virtual String Type { get; set; }
        public virtual String Description { get; set; }
        public virtual String IsSoftDelete { get; set; }
        public virtual String IsHaveSyncTime { get; set; }
        public virtual String TimeIntervalProperty { get; set; }
        public virtual String TreeParentId { get; set; }
        public virtual String IsUse { get; set; }
        public virtual List<PropertySet> PropertySetDetails { get; set; }
    }
}
