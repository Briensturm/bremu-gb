using System;
using System.Collections.Generic;
using System.Text;

namespace bremugb.cpu.Instructions
{
    public class TestInstruction : IInstruction
    {
        public int GetCycleCount => 2;
    }
}
