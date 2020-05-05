
using BremuGb.Memory;

namespace BremuGb.Cpu
{
    public class CpuCore : ICpuCore
    {
        private ICpuState _cpuState;

        private IRandomAccessMemory _mainMemory;
        private IInstruction _currentInstruction;

        public CpuCore(IRandomAccessMemory mainMemory, ICpuState cpuState)
        {
            _mainMemory = mainMemory;
            _cpuState = cpuState;

            Reset();
        }

        public void ExecuteCpuCycle()
        {   
            //TODO: Implement HALT/STOP handling

            _currentInstruction.ExecuteCycle(_cpuState, _mainMemory);

            //check if fetch
            if (_currentInstruction.IsFetchNecessary())
            {
                //check for interrupts

                LoadNextInstruction();
            }
        }

        public void Reset()
        {
            _cpuState.Reset();

            //set initial memory registers

            LoadNextInstruction();
        }

        private void LoadNextInstruction()
        {
            var nextOpcode = _mainMemory.ReadByte(_cpuState.ProgramCounter++);

            if (_cpuState.InstructionPrefix)
            {
                _currentInstruction = InstructionDecoder.GetPrefixedInstructionFromOpcode(nextOpcode);
                _cpuState.InstructionPrefix = false;
            }
            else
                _currentInstruction = InstructionDecoder.GetInstructionFromOpcode(nextOpcode);
        }
    }
}
