using NUnit.Framework;
using Moq;

using BremuGb.Memory;
using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu.Tests
{
    public class ArithmeticInstructionTests
    {
        [TestCase(0xA0)]
        [TestCase(0xA1)]
        [TestCase(0xA2)]
        [TestCase(0xA3)]
        [TestCase(0xA4)]
        [TestCase(0xA5)]
        [TestCase(0xA7)]
        public void Test_ANDAR8(byte opcode)
        {
            int registerIndex = opcode & 0x07;

            byte a = 0b01010101;
            byte data = 0b11110000;
            byte result = 0b01010000;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.A = a;
            actualState.Registers[registerIndex] = data;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.A = result;
            expectedState.Registers[registerIndex] = data;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new ANDAR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_ANDAD8()
        {
            byte a = 0b01010101;
            byte data = 0b11110000;
            byte result = 0b01010000;

            ushort pc = 0x4242;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.A = a;
            actualState.ProgramCounter = pc;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.A = result;
            expectedState.ProgramCounter = (ushort)(actualState.ProgramCounter + 1);

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(data);

            var instruction = new ANDAD8();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_ANDA_HL_()
        {
            byte a      = 0b01010101;
            byte data   = 0b11110000;
            byte result = 0b01010000;

            ushort hl = 0x4242;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.A = a;
            actualState.Registers.HL = hl;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.A = result;
            expectedState.Registers.HL = hl;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(hl)).Returns(data);

            var instruction = new ANDA_HL_();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0xB0)]
        [TestCase(0xB1)]
        [TestCase(0xB2)]
        [TestCase(0xB3)]
        [TestCase(0xB4)]
        [TestCase(0xB5)]
        [TestCase(0xB7)]
        public void Test_ORAR8(byte opcode)
        {
            int registerIndex = opcode & 0x07;

            byte a = 0b01010101;
            byte data = 0b11110000;
            byte result = 0b11110101;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.A = a;
            actualState.Registers[registerIndex] = data;

            var expectedState = new CpuState();            
            expectedState.Registers.A = result;
            expectedState.Registers[registerIndex] = data;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new ORAR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_ORAD8()
        {
            byte a = 0b01010101;
            byte data = 0b11110000;
            byte result = 0b11110101;

            ushort pc = 0x4242;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.A = a;
            actualState.ProgramCounter = pc;

            var expectedState = new CpuState();
            expectedState.Registers.A = result;
            expectedState.ProgramCounter = (ushort)(actualState.ProgramCounter + 1);

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(data);

            var instruction = new ORAD8();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_ORA_HL_()
        {
            byte a      = 0b01010101;
            byte data   = 0b11110000;
            byte result = 0b11110101;

            ushort hl = 0x4242;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.A = a;
            actualState.Registers.HL = hl;

            var expectedState = new CpuState();            
            expectedState.Registers.A = result;
            expectedState.Registers.HL = hl;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(hl)).Returns(data);

            var instruction = new ORA_HL_();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0xA8)]
        [TestCase(0xA9)]
        [TestCase(0xAA)]
        [TestCase(0xAB)]
        [TestCase(0xAC)]
        [TestCase(0xAD)]
        [TestCase(0xAF)]
        public void Test_XORAR8(byte opcode)
        {
            int registerIndex = opcode & 0x07;

            byte a      = 0b01010101;
            byte data   = 0b11110000;
            byte result = 0b10100101;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.A = a;
            actualState.Registers[registerIndex] = data;

            var expectedState = new CpuState();
            expectedState.Registers.A = result;
            expectedState.Registers[registerIndex] = data;

            //handle case XORA A
            if (opcode == 0xAF)
            {
                actualState.Registers.ZeroFlag = false;

                expectedState.Registers.A = 0;
                expectedState.Registers.ZeroFlag = true;
            }

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new XORAR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_XORAD8()
        {
            byte a = 0b01010101;
            byte data = 0b11110000;
            byte result = 0b10100101;

            ushort pc = 0x4242;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.A = a;
            actualState.ProgramCounter = pc;

            var expectedState = new CpuState();
            expectedState.Registers.A = result;
            expectedState.ProgramCounter = (ushort)(actualState.ProgramCounter + 1);

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(data);

            var instruction = new XORAD8();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_XORA_HL_()
        {
            byte a      = 0b01010101;
            byte data   = 0b11110000;
            byte result = 0b10100101;

            ushort hl = 0x4242;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = true;
            actualState.Registers.ZeroFlag = true;
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.A = a;
            actualState.Registers.HL = hl;

            var expectedState = new CpuState();            
            expectedState.Registers.A = result;
            expectedState.Registers.HL = hl;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(hl)).Returns(data);

            var instruction = new XORA_HL_();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0x80)]
        [TestCase(0x81)]
        [TestCase(0x82)]
        [TestCase(0x83)]
        [TestCase(0x84)]
        [TestCase(0x85)]
        [TestCase(0x87)]
        public void Test_ADDAR8(byte opcode)
        {
            byte a = 0xF1;
            byte data = 0x0F;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.CarryFlag = true;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            
            actualState.Registers.A = a;

            //case A + A
            if(opcode == 0x87)
                expectedState.Registers.A = (byte)(a + a);                
            else
            {
                actualState.Registers[opcode & 0x07] = data;

                expectedState.Registers[opcode & 0x07] = data;
                expectedState.Registers.A = (byte)(a + data);
                expectedState.Registers.ZeroFlag = true;
            }

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new ADDAR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test, Combinatorial]
        public void Test_ADCAR8([Values(0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8F)] byte opcode,
                              [Values(0, 1)] int carryFlag)
        {
            byte a = 0xF1;
            byte data = 0x0F;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.CarryFlag = true;

            var actualState = new CpuState();
            actualState.Registers.SubtractionFlag = true;
            actualState.Registers.CarryFlag = carryFlag == 1;

            actualState.Registers.A = a;

            //case A + A
            if (opcode == 0x8F)
                expectedState.Registers.A = (byte)(a + a + carryFlag);
            else
            {
                actualState.Registers[opcode & 0x07] = data;

                expectedState.Registers[opcode & 0x07] = data;
                expectedState.Registers.A = (byte)(a + data + carryFlag);
                expectedState.Registers.ZeroFlag = ((byte)(a + data) + (byte)carryFlag) == 0;
            }

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new ADCAR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_INC_HL_()
        {
            ushort address = 0x4242;
            byte data = 0xFF;

            var expectedState = new CpuState();
            expectedState.Registers.HL = address;
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.ZeroFlag = true;

            var actualState = new CpuState();
            actualState.Registers.HL = address;
            actualState.Registers.SubtractionFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(address)).Returns(data);

            var instruction = new INC_HL_();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(address, (byte)(data + 1)), Times.Once());
        }

        [Test]
        public void Test_DEC_HL_()
        {
            ushort address = 0x4242;
            byte data = 0x00;

            var expectedState = new CpuState();
            expectedState.Registers.HL = address;
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.SubtractionFlag = true;

            var actualState = new CpuState();
            actualState.Registers.HL = address;
            actualState.Registers.ZeroFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(address)).Returns(data);

            var instruction = new DEC_HL_();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(address, (byte)(data - 1)), Times.Once());
        }

        [TestCase(0x05)]
        [TestCase(0x15)]
        [TestCase(0x25)]
        [TestCase(0x0D)]
        [TestCase(0x1D)]
        [TestCase(0x2D)]
        [TestCase(0x3D)]
        public void Test_DECR8(byte opcode)
        {
            var registerIndex = opcode >> 3;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.SubtractionFlag = true;
            expectedState.Registers[registerIndex]--;

            var actualState = new CpuState();
            actualState.Registers.ZeroFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new DECR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0x04)]
        [TestCase(0x14)]
        [TestCase(0x24)]
        [TestCase(0x0C)]
        [TestCase(0x1C)]
        [TestCase(0x2C)]
        [TestCase(0x3C)]
        public void Test_INCR8(byte opcode)
        {
            var data = 0xFF;
            var registerIndex = opcode >> 3;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers[registerIndex] = (byte)(data + 1);
            expectedState.Registers.ZeroFlag = true;

            var actualState = new CpuState();
            actualState.Registers[registerIndex] = (ushort)data;
            actualState.Registers.SubtractionFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new INCR8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0x0B)]
        [TestCase(0x1B)]
        [TestCase(0x2B)]
        [TestCase(0x3B)]
        public void Test_DECR16(byte opcode)
        {
            var actualState = new CpuState();
            actualState.Registers.BC = 0x11FF;
            actualState.Registers.DE = 0x22FF;
            actualState.Registers.HL = 0x33FF;
            actualState.StackPointer = 0x44FF;

            var expectedState = new CpuState();
            expectedState.Registers.BC = actualState.Registers.BC;
            expectedState.Registers.DE = actualState.Registers.DE;
            expectedState.Registers.HL = actualState.Registers.HL;
            expectedState.StackPointer = actualState.StackPointer;

            switch (opcode)
            {
                case 0x0B:
                    expectedState.Registers.BC--;
                    break;
                case 0x1B:
                    expectedState.Registers.DE--;
                    break;
                case 0x2B:
                    expectedState.Registers.HL--;
                    break;
                case 0x3B:
                    expectedState.StackPointer--;
                    break;
            }

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new DECR16(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0x03)]
        [TestCase(0x13)]
        [TestCase(0x23)]
        [TestCase(0x33)]
        public void Test_INCR16(byte opcode)
        {
            var actualState = new CpuState();
            actualState.Registers.BC = 0x11FF;
            actualState.Registers.DE = 0x22FF;
            actualState.Registers.HL = 0x33FF;
            actualState.StackPointer = 0x44FF;

            var expectedState = new CpuState();
            expectedState.Registers.BC = actualState.Registers.BC;
            expectedState.Registers.DE = actualState.Registers.DE;
            expectedState.Registers.HL = actualState.Registers.HL;
            expectedState.StackPointer = actualState.StackPointer;

            switch (opcode)
            {
                case 0x03:
                    expectedState.Registers.BC++;
                    break;
                case 0x13:
                    expectedState.Registers.DE++;
                    break;
                case 0x23:
                    expectedState.Registers.HL++;
                    break;
                case 0x33:
                    expectedState.StackPointer++;
                    break;
            }

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new INCR16(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }
    }
}