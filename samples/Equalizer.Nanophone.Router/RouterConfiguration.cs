using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Equalizer.Nanophone.Router
{
    public class RouterConfiguration
    {
        public Router Router { get; set; }
    }

    public class Router
    {
        public string Prefix { get; set; }
    }
}