
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class DECR : InstructionBase
    {
        protected override int InstructionLength => 1;

        public DECR(byte opcode) : base(opcode)
        {
        }

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            var registerIndex = _opcode >> 3;
            var oldValue = cpuState.Registers[registerIndex]--;

            cpuState.Registers.AddSubFlag = true;
            cpuState.Registers.ZeroFlag = cpuState.Registers[registerIndex] == 0;
            cpuState.Registers.HalfCarryFlag = (oldValue & 0x0F) == 0;

            base.ExecuteCycle(cpuState, mainMemory);
        }
    }
}
