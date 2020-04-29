using System;
using System.Collections.Generic;
using System.Text;

namespace bremugb.cpu.Instructions
{
    public interface IInstruction
    {
        public int GetCycleCount { get; }
    }
}
