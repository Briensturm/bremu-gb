
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class PUSH : InstructionBase
    {
        private byte _registerBits;
        private bool _writeDataLoaded;

        private byte _msbData;
        private byte _lsbData;

        protected override int InstructionLength => 4;

        public PUSH(byte opcode) : base(opcode)
        {
            _registerBits = (byte)((opcode >> 4) & 0x03);
        }

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            if (!_writeDataLoaded)
                LoadWriteData(cpuState);

            switch(_remainingCycles)
            {
                case 4:
                    cpuState.StackPointer--;
                    break;
                case 3:
                    mainMemory.WriteByte(cpuState.StackPointer--, _msbData);
                    break;
                case 2:
                    mainMemory.WriteByte(cpuState.StackPointer--, _lsbData);
                    break;
            }

            base.ExecuteCycle(cpuState, mainMemory);
        }

        private void LoadWriteData(ICpuState cpuState)
        {
            switch(_registerBits)
            {
                case 0x00:
                    _msbData = cpuState.Registers.B;
                    _lsbData = cpuState.Registers.C;
                    break;
                case 0x01:
                    _msbData = cpuState.Registers.D;
                    _lsbData = cpuState.Registers.E;
                    break;
                case 0x10:
                    _msbData = cpuState.Registers.H;
                    _lsbData = cpuState.Registers.L;
                    break;
                case 0x11:
                    _msbData = cpuState.Registers.A;
                    _lsbData = cpuState.Registers.F;
                    break;
            }

            _writeDataLoaded = true;
        }
    }
}
