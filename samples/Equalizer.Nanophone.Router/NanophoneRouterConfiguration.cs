﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Equalizer.Nanophone.Router
{
    public class NanophoneRouterConfiguration
    {
        public Router Router { get; set; }
        public Consul Consul { get; set; }
    }

    public class Router
    {
        public string Prefix { get; set; }
    }

    public class Consul
    {
        public bool IgnoreCriticalServices { get; set; }
    }
}