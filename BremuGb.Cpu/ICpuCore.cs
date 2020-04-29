using System;
using System.Collections.Generic;
using System.Text;

namespace bremugb.cpu
{
    interface ICpuCore
    {
        public void Initialize();
        public void Run();
        public void Clock();
    }
}
