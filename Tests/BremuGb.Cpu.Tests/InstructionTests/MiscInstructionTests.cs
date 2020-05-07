using NUnit.Framework;
using Moq;

using BremuGb.Memory;
using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu.Tests
{
    public class MiscInstructionTests
    {
        [Test]
        public void TestNOP()
        {
            var expectedState = new CpuState();
            var actualState = new CpuState();
            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new NOP();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestSCF()
        {
            var expectedState = new CpuState();
            expectedState.Registers.CarryFlag = true;

            var actualState = new CpuState();
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.AddSubFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new SCF();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestHALT()
        {
            var expectedState = new CpuState();
            expectedState.HaltMode = true;

            var actualState = new CpuState();
            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new HALT();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestSTOP()
        {
            var expectedState = new CpuState();
            expectedState.StopMode = true;

            var actualState = new CpuState();
            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new STOP();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestPREFIX()
        {
            var expectedState = new CpuState();
            expectedState.InstructionPrefix = true;

            var actualState = new CpuState();
            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new PREFIX();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestDI()
        {
            var expectedState = new CpuState();
            var actualState = new CpuState
            {
                InterruptMasterEnable = true,
                ImeScheduled = true
            };

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new DI();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestEI()
        {
            var expectedState = new CpuState
            {
                ImeScheduled = true
            };

            var actualState = new CpuState();

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new EI();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestCCF()
        {
            var expectedState = new CpuState();
            expectedState.Registers.CarryFlag = !expectedState.Registers.CarryFlag;

            var actualState = new CpuState();
            actualState.Registers.HalfCarryFlag = true;
            actualState.Registers.AddSubFlag = true;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new CCF();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestCPL()
        {
            var expectedState = new CpuState();
            expectedState.Registers.A = 0x42;
            expectedState.Registers.HalfCarryFlag = true;
            expectedState.Registers.AddSubFlag = true;

            var actualState = new CpuState();
            actualState.Registers.A = (byte)(~expectedState.Registers.A);

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new CPL();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }
    }
}