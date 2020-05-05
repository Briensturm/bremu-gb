
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public class JPNN : InstructionBase
    {
        private ushort _jumpAddress;

        protected override int InstructionLength => 3;

        public override void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            switch(_remainingCycles)
            {
                case 3:
                    //read jump address lsb
                    _jumpAddress = mainMemory.ReadByte(cpuState.ProgramCounter++);
                    break;
                case 2:
                    //read jump address msb
                    _jumpAddress |= (ushort)(mainMemory.ReadByte(cpuState.ProgramCounter++) << 8);
                    break;
                case 1:
                    //do the jump
                    cpuState.ProgramCounter = _jumpAddress;
                    break;
            }

            base.ExecuteCycle(cpuState, mainMemory);
        }
    }
}
