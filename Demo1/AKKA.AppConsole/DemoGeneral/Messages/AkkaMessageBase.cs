﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.AppConsole
{
    [ToString]
    public class AkkaMessageBase
    {
        public DateTime TimeStamp => DateTime.Now.ToUniversalTime();
    }
}