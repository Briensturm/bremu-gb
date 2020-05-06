﻿
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class DECRR : InstructionBase
    {
        private byte _registerBits;
        protected override int InstructionLength => 2;

        public DECRR(byte opcode) : base(opcode)
        {
            _registerBits = (byte)((opcode >> 4) & 0x03);
        }

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            switch(_remainingCycles)
            {
                case 1:
                    switch(_registerBits)
                    {
                        case 0x00:
                            cpuState.Registers.BC--;
                            break;
                        case 0x01:
                            cpuState.Registers.DE--;
                            break;
                        case 0x10:
                            cpuState.Registers.HL--;
                            break;
                        case 0x11:
                            cpuState.StackPointer--;
                            break;
                    }
                    break;
            }
            base.ExecuteCycle(cpuState, mainMemory);
        }
    }
}
