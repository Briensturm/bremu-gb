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
        public void DecodeNOP()
        {
            byte opcode = 0x00;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<NOP>(instruction);
        }

        [Test]
        public void DecodeINCHL()
        {
            byte opcode = 0x34;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<INCHL>(instruction);
        }

        [TestCase(0x04)]
        [TestCase(0x14)]
        [TestCase(0x24)]
        [TestCase(0x0C)]
        [TestCase(0x1C)]
        [TestCase(0x2C)]
        [TestCase(0x3C)]
        public void DecodeINCR(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<INCR>(instruction);
        }

        [TestCase(0x03)]
        [TestCase(0x13)]
        [TestCase(0x23)]
        [TestCase(0x33)]
        public void DecodeINCRR(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<INCRR>(instruction);
        }

        [Test]
        public void DecodeDECHL()
        {
            byte opcode = 0x35;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<DECHL>(instruction);
        }

        [TestCase(0x05)]
        [TestCase(0x15)]
        [TestCase(0x25)]
        [TestCase(0x0D)]
        [TestCase(0x1D)]
        [TestCase(0x2D)]
        [TestCase(0x3D)]
        public void DecodeDECR(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<DECR>(instruction);
        }

        [TestCase(0x0B)]
        [TestCase(0x1B)]
        [TestCase(0x2B)]
        [TestCase(0x3B)]
        public void DecodeDECRR(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<DECRR>(instruction);
        }

        [Test]
        public void DecodeCCF()
        {
            byte opcode = 0x3F;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<CCF>(instruction);
        }

        [Test]
        public void DecodeCPL()
        {
            byte opcode = 0x2F;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<CPL>(instruction);
        }

        [Test]
        public void DecodeDI()
        {
            byte opcode = 0xF3;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<DI>(instruction);
        }

        [Test]
        public void DecodeEI()
        {
            byte opcode = 0xFB;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<EI>(instruction);
        }

        [Test]
        public void DecodeHALT()
        {
            byte opcode = 0x76;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<HALT>(instruction);
        }

        [Test]
        public void DecodePREFIX()
        {
            byte opcode = 0xCB;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<PREFIX>(instruction);
        }

        [Test]
        public void DecodeSTOP()
        {
            byte opcode = 0x10;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<STOP>(instruction);
        }

        [Test]
        public void DecodeSCF()
        {
            byte opcode = 0x37;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<SCF>(instruction);
        }

        [Test]
        public void DecodeCALL()
        {
            byte opcode = 0xCD;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<CALL>(instruction);
        }

        [Test]
        public void DecodeJR()
        {
            byte opcode = 0x18;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<JR>(instruction);
        }

        [Test]
        public void DecodeJPHL()
        {
            byte opcode = 0xE9;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<JPHL>(instruction);
        }

        [Test]
        public void DecodeRETI()
        {
            byte opcode = 0xD9;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<RETI>(instruction);
        }

        [Test]
        public void DecodeRET()
        {
            byte opcode = 0xC9;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<RET>(instruction);
        }

        [TestCase(0xC0)]
        [TestCase(0xD0)]
        [TestCase(0xC8)]
        [TestCase(0xD8)]
        public void DecodeRETCC(byte opcode)
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
        public void DecodeRST(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<RST>(instruction);
        }

        [Test]
        public void DecodeJPNN()
        {
            byte opcode = 0xC3;

            var instruction = InstructionDecoder.GetInstructionFromOpcode(opcode);

            Assert.IsInstanceOf<JPNN>(instruction);
        }

        [TestCase(0xC2)]
        [TestCase(0xD2)]
        [TestCase(0xCA)]
        [TestCase(0xDA)]
        public void DecodeJPCC(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<JPCC>(instruction);
        }

        [TestCase(0x20)]
        [TestCase(0x30)]
        [TestCase(0x28)]
        [TestCase(0x38)]
        public void DecodeJRCC(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<JRCC>(instruction);
        }

        [TestCase(0xC1)]
        [TestCase(0xD1)]
        [TestCase(0xE1)]
        [TestCase(0xF1)]
        public void DecodePOP(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<POP>(instruction);
        }

        [TestCase(0xC5)]
        [TestCase(0xD5)]
        [TestCase(0xE5)]
        [TestCase(0xF5)]
        public void DecodePUSH(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<PUSH>(instruction);
        }

        [TestCase(0xC4)]
        [TestCase(0xD4)]
        [TestCase(0xCC)]
        [TestCase(0xDC)]
        public void DecodeCALLCC(byte opcode)
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
        public void DecodeLDRR(byte opcode)
        {
            var instruction = InstructionDecoder.GetInstructionFromOpcode((byte)opcode);

            Assert.IsInstanceOf<LDRR>(instruction);
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
