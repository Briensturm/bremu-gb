using NUnit.Framework;
using Moq;

using BremuGb.Memory;
using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu.Tests
{
    public class LoadInstructionTests
    {
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
        public void Test_LDR8R8(byte opcode)
        {
            byte data = 0x42;

            var sourceIndex = opcode & 0x07;
            var targetIndex = (opcode >> 3) & 0x07;

            var expectedState = new CpuState();
            expectedState.Registers[sourceIndex] = data;
            expectedState.Registers[targetIndex] = data;

            var actualState = new CpuState();
            actualState.Registers[sourceIndex] = data;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new LDR8R8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            //assert
            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0x01)]
        [TestCase(0x11)]
        [TestCase(0x21)]
        [TestCase(0x31)]
        public void Test_LDR16N16(byte opcode)
        {
            ushort pc = 0x1122;
            byte lsbData = 0x24;
            byte msbData = 0x42;

            var actualState = new CpuState();
            actualState.ProgramCounter = pc;

            var expectedState = new CpuState();
            expectedState.ProgramCounter = (ushort)(actualState.ProgramCounter + 2); 
            
            switch(opcode)
            {
                case 0x01:
                    expectedState.Registers.BC = (ushort)((msbData << 8) | lsbData);
                    break;
                case 0x11:
                    expectedState.Registers.DE = (ushort)((msbData << 8) | lsbData);
                    break;
                case 0x21:
                    expectedState.Registers.HL = (ushort)((msbData << 8) | lsbData);
                    break;
                case 0x31:
                    expectedState.StackPointer = (ushort)((msbData << 8) | lsbData);
                    break;
            }

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(lsbData);
            memoryMock.Setup(m => m.ReadByte((ushort)(pc + 1))).Returns(msbData);

            var instruction = new LDR16N16(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void Test_LD_HL_D8()
        {
            ushort pc = 0x1234;
            ushort hl = 0x4321;
            byte data = 0x42;

            var actualState = new CpuState();
            actualState.ProgramCounter = pc;
            actualState.Registers.HL = hl;

            var expectedState = new CpuState();
            expectedState.ProgramCounter = (ushort)(actualState.ProgramCounter + 1);
            expectedState.Registers.HL = hl;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(data);

            var instruction = new LD_HL_D8();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(hl, data), Times.Once);
        }

        [TestCase(0x06)]
        [TestCase(0x16)]
        [TestCase(0x26)]
        [TestCase(0x0E)]
        [TestCase(0x1E)]
        [TestCase(0x2E)]
        [TestCase(0x3E)]
        public void Test_LDR8D8(byte opcode)
        {
            ushort pc = 0x1234;
            byte data = 0x42;

            var registerIndex = opcode >> 3;

            var actualState = new CpuState();
            actualState.ProgramCounter = pc;

            var expectedState = new CpuState();
            expectedState.ProgramCounter = (ushort)(actualState.ProgramCounter + 1);
            expectedState.Registers[registerIndex] = data;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(data);

            var instruction = new LDR8D8(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }
    }
}