using System;
using BremuGb.Memory;

namespace BremuGb.Cpu.Instructions
{
    public abstract class InstructionBase : IInstruction
    {
        protected int _remainingCycles;
        protected byte _opcode;
        protected abstract int InstructionLength { get; }

        public InstructionBase(byte opcode = 0x00)
        {
            _remainingCycles = InstructionLength;
            _opcode = opcode;
        }

        public virtual void ExecuteCycle(ICpuState cpuState, IRandomAccessMemory mainMemory)
        {
            _remainingCycles--;
        }

        public bool IsFetchNecessary()
        {
            return _remainingCycles == 0;
        }

        public void Initialize()
        {
            if (InstructionLength < 1)
                throw new InvalidOperationException("Instruction cycle count must be greater than 1");

            _remainingCycles = InstructionLength;
        }

        protected bool IsConditionMet(ICpuState cpuState)
        {
            switch(_opcode & 0x1C)
            {
                case 0x00:
                    return !cpuState.Registers.ZeroFlag;
                case 0x01:
                    return cpuState.Registers.ZeroFlag;
                case 0x10:
                    return !cpuState.Registers.CarryFlag;
                case 0x11:
                    return cpuState.Registers.ZeroFlag;
                default:
                    throw new InvalidOperationException($"Unexpected behavior for conditional opcode 0x{_opcode:X2}");
            }
        }
    }
}
