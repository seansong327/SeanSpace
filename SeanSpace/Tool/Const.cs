using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SeanSpace.Tool
{
    public static class Const
    {
        public static string SearchUrl = ConfigurationManager.AppSettings["SearchUrl"];
    }
}