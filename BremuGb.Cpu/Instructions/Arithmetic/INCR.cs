
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class INCR : InstructionBase
    {
        protected override int InstructionLength => 1;

        public INCR(byte opcode) : base(opcode)
        {
        }

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            var registerIndex = _opcode >> 3;
            cpuState.Registers[registerIndex]++;

            cpuState.Registers.AddSubFlag = false;
            cpuState.Registers.ZeroFlag = cpuState.Registers[registerIndex] == 0;
            cpuState.Registers.HalfCarryFlag = (cpuState.Registers[registerIndex] & 0x0F) == 0;

            base.ExecuteCycle(cpuState, mainMemory);
        }
    }
}
