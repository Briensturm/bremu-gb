using System;

namespace bremugb.cpu.Instructions
{
    public static class InstructionDecoder
    {
        public static IInstruction GetInstructionFromOpcode(byte opcode)
        {
            switch(opcode)
            {
                case 0x000:
                    return new TestInstruction();
                default:
                    throw new InvalidOperationException("Unable to decode unknown opcode: " + opcode);
            }
        }
    }
}
