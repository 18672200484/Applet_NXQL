using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CMCS.DapperDber.Attrs;

namespace CMCS.Common.Entities.Sys
{
    [Serializable]
    public class EntityBase2
    {
        public EntityBase2()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
        }

        [DapperPrimaryKey]
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
