using System;
using NUnit.Framework;
using Moq;

using BremuGb.Memory;
using BremuGb.Cpu.Instructions;

namespace BremuGb.Cpu.Tests
{
    public class ControlFlowInstructionTests
    {
        [Test]
        public void TestCALL()
        {
            ushort sp = 0x3333;
            ushort pc = 0x1122;
            byte lsbData = 0x24;
            byte msbData = 0x42;

            var expectedState = new CpuState();
            expectedState.StackPointer = (ushort)(sp - 2);
            expectedState.ProgramCounter = (ushort)((msbData << 8) | lsbData);

            var actualState = new CpuState();
            actualState.StackPointer = sp;
            actualState.ProgramCounter = pc;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(lsbData);
            memoryMock.Setup(m => m.ReadByte((ushort)(pc + 1))).Returns(msbData);

            var instruction = new CALL();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte((ushort)(sp - 1), (byte)((pc+2) >> 8)), Times.Once);
            memoryMock.Verify(m => m.WriteByte((ushort)(sp - 2), (byte)((pc+2) & 0x00FF)), Times.Once);
        }

        [Test]
        public void TestCALLCC([Values(0xC4, 0xD4, 0xCC, 0xDC)] byte opcode,
                              [Values(0x0, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60,
                                0x70, 0x80, 0x90, 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0)] byte flags)
        {
            ushort sp = 0x3333;
            ushort pc = 0x1122;
            byte lsbData = 0x24;
            byte msbData = 0x42;

            var expectedState = new CpuState();
            expectedState.Registers.F = flags;

            var actualState = new CpuState();
            actualState.StackPointer = sp;
            actualState.ProgramCounter = pc;
            actualState.Registers.F = flags;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(pc)).Returns(lsbData);
            memoryMock.Setup(m => m.ReadByte((ushort)(pc + 1))).Returns(msbData);

            bool conditionMet;
            switch (opcode)
            {
                case 0xC4:
                    conditionMet = (flags & 0x80) == 0;
                    break;
                case 0xD4:
                    conditionMet = (flags & 0x10) == 0;
                    break;
                case 0xCC:
                    conditionMet = (flags & 0x80) == 0x80;
                    break;
                case 0xDC:
                    conditionMet = (flags & 0x10) == 0x10;
                    break;
                default:
                    throw new ArgumentException($"Opcode {opcode:X2} is not a CALLCC instruction");
            }

            //expect jump if condition is met
            if (conditionMet)
            {
                expectedState.ProgramCounter = (ushort)((msbData << 8) | lsbData);
                expectedState.StackPointer = (ushort)(sp - 2);
            }
            else
            {
                expectedState.StackPointer = sp;
                expectedState.ProgramCounter = (ushort)(pc + 2);
            }

            var instruction = new CALLCC(opcode);

            //act
            var cycleCount = 0;
            while (!instruction.IsFetchNecessary())
            {
                instruction.ExecuteCycle(actualState, memoryMock.Object);
                cycleCount++;
            }

            //assert
            if (conditionMet)
            {
                Assert.AreEqual(6, cycleCount, "Unexpected cycle count for CALLCC instruction");
                memoryMock.Verify(m => m.WriteByte((ushort)(sp - 1), (byte)((pc + 2) >> 8)), Times.Once);
                memoryMock.Verify(m => m.WriteByte((ushort)(sp - 2), (byte)((pc + 2) & 0x00FF)), Times.Once);
            }
            else
            {
                Assert.AreEqual(3, cycleCount, "Unexpected cycle count for CALLCC instruction");
                memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
            }

            TestHelper.ValidateCpuState(expectedState, actualState);            
        }

        [Test]
        public void TestRET()
        {
            ushort sp = 0x4242;
            byte lsbData = 0x11;
            byte msbData = 0x22;

            var expectedState = new CpuState();
            expectedState.StackPointer = (ushort)(sp + 2);
            expectedState.ProgramCounter = (ushort)((msbData << 8) | lsbData);

            var actualState = new CpuState();
            actualState.StackPointer = sp;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(sp)).Returns(lsbData);
            memoryMock.Setup(m => m.ReadByte((ushort)(sp+1))).Returns(msbData);

            var instruction = new RET();

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test]
        public void TestRETI()
        {
            ushort sp = 0x4242;
            byte lsbData = 0x11;
            byte msbData = 0x22;

            var expectedState = new CpuState();
            expectedState.StackPointer = (ushort)(sp + 2);
            expectedState.ProgramCounter = (ushort)((msbData << 8) | lsbData);
            expectedState.InterruptMasterEnable = true;

            var actualState = new CpuState();
            actualState.StackPointer = sp;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(sp)).Returns(lsbData);
            memoryMock.Setup(m => m.ReadByte((ushort)(sp + 1))).Returns(msbData);

            var instruction = new RETI();

            //act
            while (!instruction.IsFetchNecessary())
            {
                if(actualState.ImeScheduled)
                {
                    actualState.ImeScheduled = false;
                    actualState.InterruptMasterEnable = true;
                }
                instruction.ExecuteCycle(actualState, memoryMock.Object);
            }

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [Test, Combinatorial]
        public void TestRETCC([Values(0xC0, 0xD0, 0xC8, 0xD8)] byte opcode,
                              [Values(0x0, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60,
                                0x70, 0x80, 0x90, 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0)] byte flags)
        {
            ushort sp = 0x4242;
            byte lsbData = 0x11;
            byte msbData = 0x22;

            var expectedState = new CpuState();
            expectedState.Registers.F = flags;

            var actualState = new CpuState();
            actualState.Registers.F = flags;
            actualState.StackPointer = sp;

            var memoryMock = new Mock<IRandomAccessMemory>();
            memoryMock.Setup(m => m.ReadByte(sp)).Returns(lsbData);
            memoryMock.Setup(m => m.ReadByte((ushort)(sp + 1))).Returns(msbData);

            bool conditionMet;
            switch (opcode)
            {
                case 0xC0:
                    conditionMet = (flags & 0x80) == 0;
                    break;
                case 0xD0:
                    conditionMet = (flags & 0x10) == 0;
                    break;
                case 0xC8:
                    conditionMet = (flags & 0x80) == 0x80;
                    break;
                case 0xD8:
                    conditionMet = (flags & 0x10) == 0x10;
                    break;
                default:
                    throw new ArgumentException($"Opcode {opcode:X2} is not a RETCC instruction");
            }

            //expect jump if condition is met
            if (conditionMet)
            {
                expectedState.ProgramCounter = (ushort)((msbData << 8) | lsbData);
                expectedState.StackPointer = (ushort)(sp + 2);
            }
            else
                expectedState.StackPointer = sp;

            var instruction = new RETCC(opcode);

            //act
            var cycleCount = 0;
            while (!instruction.IsFetchNecessary())
            {
                instruction.ExecuteCycle(actualState, memoryMock.Object);
                cycleCount++;
            }

            //assert
            if (conditionMet)
                Assert.AreEqual(5, cycleCount);
            else
                Assert.AreEqual(2, cycleCount);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
        }

        [TestCase(0xC7)]
        [TestCase(0xD7)]
        [TestCase(0xE7)]
        [TestCase(0xF7)]
        [TestCase(0xCF)]
        [TestCase(0xDF)]
        [TestCase(0xEF)]
        [TestCase(0xFF)]
        public void TestRST(byte opcode)
        {
            ushort resetAddress = (ushort)(opcode & 0x38);

            ushort sp = 0x4242;
            ushort pc = 0x1122;

            var expectedState = new CpuState();
            expectedState.StackPointer = (ushort)(sp - 2);
            expectedState.ProgramCounter = resetAddress;

            var actualState = new CpuState();
            actualState.StackPointer = sp;
            actualState.ProgramCounter = pc;

            var memoryMock = new Mock<IRandomAccessMemory>();

            var instruction = new RST(opcode);

            //act
            while (!instruction.IsFetchNecessary())
                instruction.ExecuteCycle(actualState, memoryMock.Object);

            TestHelper.ValidateCpuState(expectedState, actualState);
            memoryMock.Verify(m => m.WriteByte((ushort)(sp - 1), (byte)(pc >> 8)), Times.Once);
            memoryMock.Verify(m => m.WriteByte((ushort)(sp - 2), (byte)(pc & 0x00FF)), Times.Once);
        }
    }
}
