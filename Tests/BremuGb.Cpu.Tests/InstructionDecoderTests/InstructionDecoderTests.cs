using System;
using NUnit.Framework;

using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu.Tests
{
    public class InstructionDecoderTests
    {
        [Test]
        public void All_non_forbidden_instructions_are_implemented()
        {
            for (ushort opcode = 0x00; opcode <= 0xFF; opcode++)
            {
                if (opcode == 0xD3 || opcode == 0xE3 || opcode == 0xE4 || opcode == 0xF4
                    || opcode == 0xDB || opcode == 0xEB || opcode == 0xEC || opcode == 0xFC
                    || opcode == 0xDD || opcode == 0xED || opcode == 0xFD)
                    continue;

                Assert.DoesNotThrow(() => InstructionDecoder.GetInstructionFromOpcode((byte)opcode));
            }          
        }

        [Test]
        public void Decode_NOP()
        {
            byte opcode = 0x00;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<NOP>(instruction);
        }

        [Test]
        public void Decode_LD_HL_D8()
        {
            byte opcode = 0x36;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<LD_HL_D8>(instruction);
        }

        [TestCase(0x80)]
        [TestCase(0x81)]
        [TestCase(0x82)]
        [TestCase(0x83)]
        [TestCase(0x84)]
        [TestCase(0x85)]
        [TestCase(0x87)]
        public void Decode_ADDAR8(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<ADDAR8>(instruction);
        }

        [TestCase(0x88)]
        [TestCase(0x89)]
        [TestCase(0x8A)]
        [TestCase(0x8B)]
        [TestCase(0x8C)]
        [TestCase(0x8D)]
        [TestCase(0x8F)]
        public void Decode_ADCAR8(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<ADCAR8>(instruction);
        }

        [TestCase(0x06)]
        [TestCase(0x16)]
        [TestCase(0x26)]
        [TestCase(0x0E)]
        [TestCase(0x1E)]
        [TestCase(0x2E)]
        [TestCase(0x3E)]
        public void Decode_LDR8D8(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<LDR8D8>(instruction);
        }

        [Test]
        public void Decode_INC_HL_()
        {
            byte opcode = 0x34;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<INC_HL_>(instruction);
        }

        [TestCase(0x04)]
        [TestCase(0x14)]
        [TestCase(0x24)]
        [TestCase(0x0C)]
        [TestCase(0x1C)]
        [TestCase(0x2C)]
        [TestCase(0x3C)]
        public void Decode_INCR8(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<INCR8>(instruction);
        }

        [TestCase(0x03)]
        [TestCase(0x13)]
        [TestCase(0x23)]
        [TestCase(0x33)]
        public void Decode_INCR16(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<INCR16>(instruction);
        }

        [Test]
        public void Decode_DEC_HL_()
        {
            byte opcode = 0x35;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<DEC_HL_>(instruction);
        }

        [TestCase(0x05)]
        [TestCase(0x15)]
        [TestCase(0x25)]
        [TestCase(0x0D)]
        [TestCase(0x1D)]
        [TestCase(0x2D)]
        [TestCase(0x3D)]
        public void Decode_DECR8(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<DECR8>(instruction);
        }

        [TestCase(0x0B)]
        [TestCase(0x1B)]
        [TestCase(0x2B)]
        [TestCase(0x3B)]
        public void Decode_DECR16(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<DECR16>(instruction);
        }

        [Test]
        public void Decode_CCF()
        {
            byte opcode = 0x3F;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<CCF>(instruction);
        }

        [Test]
        public void Decode_CPL()
        {
            byte opcode = 0x2F;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<CPL>(instruction);
        }

        [Test]
        public void Decode_DI()
        {
            byte opcode = 0xF3;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<DI>(instruction);
        }

        [Test]
        public void Decode_EI()
        {
            byte opcode = 0xFB;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<EI>(instruction);
        }

        [Test]
        public void Decode_HALT()
        {
            byte opcode = 0x76;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<HALT>(instruction);
        }

        [Test]
        public void Decode_PREFIX()
        {
            byte opcode = 0xCB;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<PREFIX>(instruction);
        }

        [Test]
        public void Decode_STOP()
        {
            byte opcode = 0x10;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<STOP>(instruction);
        }

        [Test]
        public void Decode_SCF()
        {
            byte opcode = 0x37;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<SCF>(instruction);
        }

        [Test]
        public void Decode_CALL()
        {
            byte opcode = 0xCD;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<CALL>(instruction);
        }

        [Test]
        public void Decode_JR()
        {
            byte opcode = 0x18;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<JR>(instruction);
        }

        [Test]
        public void Decode_JPHL()
        {
            byte opcode = 0xE9;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<JPHL>(instruction);
        }

        [Test]
        public void Decode_RETI()
        {
            byte opcode = 0xD9;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<RETI>(instruction);
        }

        [Test]
        public void Decode_RET()
        {
            byte opcode = 0xC9;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<RET>(instruction);
        }

        [TestCase(0xC0)]
        [TestCase(0xD0)]
        [TestCase(0xC8)]
        [TestCase(0xD8)]
        public void Decode_RETCC(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<RETCC>(instruction);
        }

        [TestCase(0xC7)]
        [TestCase(0xD7)]
        [TestCase(0xE7)]
        [TestCase(0xF7)]
        [TestCase(0xCF)]
        [TestCase(0xDF)]
        [TestCase(0xEF)]
        [TestCase(0xFF)]
        public void Decode_RST(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<RST>(instruction);
        }

        [Test]
        public void Decode_JPNN()
        {
            byte opcode = 0xC3;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<JPD16>(instruction);
        }

        [TestCase(0xC2)]
        [TestCase(0xD2)]
        [TestCase(0xCA)]
        [TestCase(0xDA)]
        public void Decode_JPCC(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<JPCC>(instruction);
        }

        [TestCase(0x20)]
        [TestCase(0x30)]
        [TestCase(0x28)]
        [TestCase(0x38)]
        public void Decode_JRCC(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<JRCC>(instruction);
        }

        [TestCase(0xC1)]
        [TestCase(0xD1)]
        [TestCase(0xE1)]
        [TestCase(0xF1)]
        public void Decode_POP(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<POP>(instruction);
        }

        [TestCase(0xC5)]
        [TestCase(0xD5)]
        [TestCase(0xE5)]
        [TestCase(0xF5)]
        public void Decode_PUSH(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<PUSH>(instruction);
        }

        [TestCase(0xC4)]
        [TestCase(0xD4)]
        [TestCase(0xCC)]
        [TestCase(0xDC)]
        public void Decode_CALLCC(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<CALLCC>(instruction);
        }

        [TestCase(0x40)]
        [TestCase(0x50)]
        [TestCase(0x60)]
        [TestCase(0x41)]
        [TestCase(0x51)]
        [TestCase(0x61)]
        [TestCase(0x42)]
        [TestCase(0x52)]
        [TestCase(0x62)]
        [TestCase(0x43)]
        [TestCase(0x53)]
        [TestCase(0x63)]
        [TestCase(0x44)]
        [TestCase(0x54)]
        [TestCase(0x64)]
        [TestCase(0x45)]
        [TestCase(0x55)]
        [TestCase(0x65)]
        [TestCase(0x47)]
        [TestCase(0x57)]
        [TestCase(0x67)]
        [TestCase(0x48)]
        [TestCase(0x58)]
        [TestCase(0x68)]
        [TestCase(0x78)]
        [TestCase(0x49)]
        [TestCase(0x59)]
        [TestCase(0x69)]
        [TestCase(0x79)]
        [TestCase(0x4A)]
        [TestCase(0x5A)]
        [TestCase(0x6A)]
        [TestCase(0x7A)]
        [TestCase(0x4B)]
        [TestCase(0x5B)]
        [TestCase(0x6B)]
        [TestCase(0x7B)]
        [TestCase(0x4C)]
        [TestCase(0x5C)]
        [TestCase(0x6C)]
        [TestCase(0x7C)]
        [TestCase(0x4D)]
        [TestCase(0x5D)]
        [TestCase(0x6D)]
        [TestCase(0x7D)]
        [TestCase(0x4F)]
        [TestCase(0x5F)]
        [TestCase(0x6F)]
        [TestCase(0x7F)]
        public void Decode_LDR8R8(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<LDR8R8>(instruction);
        }

        [TestCase(0x01)]
        [TestCase(0x11)]
        [TestCase(0x21)]
        [TestCase(0x31)]
        public void Decode_LDR16N16(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<LDR16N16>(instruction);
        }

        [TestCase(0xD3)]
        [TestCase(0xE3)]
        [TestCase(0xE4)]
        [TestCase(0xF4)]
        [TestCase(0xDB)]
        [TestCase(0xEB)]
        [TestCase(0xEC)]
        [TestCase(0xFC)]
        [TestCase(0xDD)]
        [TestCase(0xED)]
        [TestCase(0xFD)]
        public void Decoding_forbidden_instructions_throws_exception(byte opcode)
        {
            Assert.Catch<InvalidOperationException>(() => InstructionDecoder.GetInstructionFromOpcode((byte)opcode));
        }
    }
}
