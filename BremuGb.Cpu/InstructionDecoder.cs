
using System;
using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu
{
    public static class InstructionDecoder
    {
        public static IInstruction GetInstructionFromOpcode(byte opcode)
        {
            //decode LDRR
            if ((opcode >> 6) == 0x01 && (opcode & 0x07) != 0x06 && (opcode & 0x38) != 0x30)
                return new LDRR(opcode);

            switch (opcode)
            {
                case 0x00:
                    return new NOP();
                case 0x10:
                    return new STOP();
                case 0x76:
                    return new HALT();
                case 0xC3:
                    return new JPNN();
                case 0xCB:
                    return new PREFIX();
                case 0xF3:
                    return new DI();
                case 0xFB:
                    return new EI();
                default:
                    throw new InvalidOperationException($"Unknown opcode, unable to decode: 0x{opcode:X2}");
            }
        }

        public static IInstruction GetPrefixedInstructionFromOpcode(byte opcode)
        {
            throw new NotImplementedException();
        }
    }
}
