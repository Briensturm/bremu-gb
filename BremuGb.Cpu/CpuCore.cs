using BremuGb.Memory;

namespace bremugb.cpu
{
    public class CpuCore
    {
        private IRandomAccessMemory _MainMemory;

        private ushort _ProgramCounter;
        private ushort _StackPointer;

        private byte _CurrentOpcode;

        public CpuCore(IRandomAccessMemory mainMemory)
        {
            _MainMemory = mainMemory;
        }
    }
}
