using BremuGb.Memory;
using BremuGb.Cpu.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BremuGb.Cpu.Tests
{
    [TestClass]
    public class InstructionTests
    {
        [TestMethod]
        public void TestLDNN()
        {
            byte opcode = 0x6B;

            //index based mocking?
            var cpuStateMock = new Mock<ICpuState>();
            cpuStateMock.Setup(mock => mock.Registers.E).Returns(0x42);
            cpuStateMock.Setup(mock => mock.Registers.E).Returns(0x42);

            var mainMemoryMock = new Mock<IRandomAccessMemory>();

            IInstruction instruction = new LDRR(opcode);

            //act
            instruction.ExecuteCycle(cpuStateMock.Object, mainMemoryMock.Object);
            

        }
    }
}
