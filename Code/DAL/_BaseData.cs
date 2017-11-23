using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neolab.Code.DAL
{
    public abstract class BaseData
    {
        public BaseData()
        {

        }
        
        public bool Disabled { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertUserID { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateUserID { get; set; }
    }
}