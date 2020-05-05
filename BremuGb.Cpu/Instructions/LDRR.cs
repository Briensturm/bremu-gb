﻿
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class LDRR : InstructionBase
    {
        protected override int InstructionLength => 1;

        public LDRR(byte opcode) : base(opcode)
        {
        }

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            //register decoding
            var sourceIndex = _opcode & 0x07;
            var targetIndex = (_opcode >> 3) & 0x07;

            //load
            cpuState.Registers[targetIndex] = cpuState.Registers[sourceIndex];

            base.ExecuteCycle(cpuState, mainMemory);
        }
    }
}
