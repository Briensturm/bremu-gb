using NUnit.Framework;
using Moq;

using BremuGb.Memory;
using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu.Tests
{
    public class ArithmeticInstructionTests
    {
        [Test]
        public void TestINCHL()
        {
            ushort address = 0x4242;
            byte data = 0xFF;

            var expectedState = new CpuState();
            expectedState.Registers.HL = address;
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.ZeroFlag = true;

            var actualState = new CpuState();
            actualState.Registers.HL = address;
            actualState.Registers.AddSubFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(address)).Returns(data);

            var instruction = new INCHL();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(address, (byte)(data + 1)), Times.Once());
        }

        [Test]
        public void TestDECHL()
        {
            ushort address = 0x4242;
            byte data = 0x00;

            var expectedState = new CpuState();
            expectedState.Registers.HL = address;
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.AddSubFlag = true;

            var actualState = new CpuState();
            actualState.Registers.HL = address;
            actualState.Registers.ZeroFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(address)).Returns(data);

            var instruction = new DECHL();

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
        public void TestDECR(byte opcode)
        {
            var registerIndex = opcode >> 3;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.AddSubFlag = true;
            expectedState.Registers[registerIndex]--;

            var actualState = new CpuState();
            actualState.Registers.ZeroFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new DECR(opcode);

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
        public void TestINCR(byte opcode)
        {
            var data = 0xFF;
            var registerIndex = opcode >> 3;

            var expectedState = new CpuState();
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers[registerIndex] = (byte)(data + 1);
            expectedState.Registers.ZeroFlag = true;

            var actualState = new CpuState();
            actualState.Registers[registerIndex] = (ushort)data;
            actualState.Registers.AddSubFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new INCR(opcode);

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
        public void TestDECRR(byte opcode)
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

            var instruction = new DECRR(opcode);

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
        public void TestINCRR(byte opcode)
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

            var instruction = new INCRR(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }
    }
}