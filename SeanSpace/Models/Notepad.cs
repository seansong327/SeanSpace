using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeanSpace.Models
{
    public class Notepad
    {
        public long Sid { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}