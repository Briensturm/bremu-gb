using System;

using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu
{
    public static class InstructionDecoder
    {
        public static IInstruction GetInstructionFromOpcode(byte opcode)
        {
            if ((opcode >> 6) == 0x01 && (opcode & 0x07) != 0x06 && (opcode & 0x38) != 0x30)
                return new LDR8R8(opcode);

            if ((opcode & 0xC7) == 0x06 && opcode != 0x36)
                return new LDR8D8(opcode);

            if ((opcode & 0xF8) == 0x80 && opcode != 0x86)
                return new ADDAR8(opcode);

            if ((opcode & 0xF8) == 0x88 && opcode != 0x8E)
                return new ADCAR8(opcode);

            if ((opcode & 0xC7) == 0x04 && (opcode & 0x38) != 0x30)
                return new INCR8(opcode);

            if ((opcode & 0xC7) == 0x05 && (opcode & 0x38) != 0x30)
                return new DECR8(opcode);

            if ((opcode & 0xCF) == 0xC5)
                return new PUSH(opcode);

            if ((opcode & 0xCF) == 0xC1)
                return new POP(opcode);

            if ((opcode & 0xCF) == 0x03)
                return new INCR16(opcode);

            if ((opcode & 0xCF) == 0x0B)
                return new DECR16(opcode);

            if ((opcode & 0xC7) == 0xC7)
                return new RST(opcode);

            if ((opcode & 0xE7) == 0xC4)
                return new CALLCC(opcode);

            if ((opcode & 0xE7) == 0xC2)
                return new JPCC(opcode);

            if ((opcode & 0xE7) == 0xC0)
                return new RETCC(opcode);

            if ((opcode & 0xE7) == 0x20)
                return new JRCC(opcode);

            if ((opcode & 0xCF) == 0x01)
                return new LDR16N16(opcode);

            switch (opcode)
            {
                case 0x00:
                    return new NOP();
                case 0x10:
                    return new STOP();
                case 0x18:
                    return new JR();
                case 0x2F:
                    return new CPL();
                case 0x34:
                    return new INC_HL_();
                case 0x35:
                    return new DEC_HL_();
                case 0x36:
                    return new LD_HL_D8();
                case 0x37:
                    return new SCF();
                case 0x3F:
                    return new CCF();
                case 0x76:
                    return new HALT();
                case 0xC3:
                    return new JPD16();
                case 0xC9:
                    return new RET();
                case 0xCB:
                    return new PREFIX();
                case 0xCD:
                    return new CALL();
                case 0xD9:
                    return new RETI();
                case 0xE9:
                    return new JPHL();
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
            if ((opcode & 0xC0) == 0x40 && (opcode & 0x07) != 0x06)
                return new BITNR8(opcode);

            if ((opcode & 0xC0) == 0xC0 && (opcode & 0x07) != 0x06)
                return new SETNR8(opcode);

            if ((opcode & 0xC0) == 0x80 && (opcode & 0x07) != 0x06)
                return new RESNR8(opcode);

            throw new NotImplementedException();
        }
    }
}
