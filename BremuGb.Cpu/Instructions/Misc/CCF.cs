
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class CCF : InstructionBase
    {
        protected override int InstructionLength => 1;

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            cpuState.Registers.AddSubFlag = false;
            cpuState.Registers.HalfCarryFlag = false;

            cpuState.Registers.CarryFlag = !cpuState.Registers.CarryFlag;

            base.ExecuteCycle(cpuState, mainMemory);
        }
    }
}
