using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string PicID { get; set; }
    }
}