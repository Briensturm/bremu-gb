
using BremuGb.Memory;

namespace bremugb.cpu
{
    public class CpuCore
    {
        private IRandomAccessMemory mainMemory;

        private ushort programCounter;
        private ushort stackPointer;

        private bool interruptMasterEnable;

        private byte _CurrentOpcode;

        public CpuCore(IRandomAccessMemory mainMemory)
        {
            this.mainMemory = mainMemory;
        }
    }
}
