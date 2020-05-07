using NUnit.Framework;

namespace BremuGb.Cpu.Tests
{
    internal static class TestHelper
    {
        internal static void ValidateCpuState(ICpuState expectedState, ICpuState actualState)
        {
            Assert.AreEqual(expectedState.StackPointer, actualState.StackPointer);
            Assert.AreEqual(expectedState.ProgramCounter, actualState.ProgramCounter);

            Assert.AreEqual(expectedState.Registers.BC, actualState.Registers.BC);
            Assert.AreEqual(expectedState.Registers.HL, actualState.Registers.HL);
            Assert.AreEqual(expectedState.Registers.DE, actualState.Registers.DE);
            Assert.AreEqual(expectedState.Registers.A, actualState.Registers.A);
            Assert.AreEqual(expectedState.Registers.F, actualState.Registers.F);

            Assert.AreEqual(expectedState.HaltMode, actualState.HaltMode);
            Assert.AreEqual(expectedState.StopMode, actualState.StopMode);
            Assert.AreEqual(expectedState.InstructionPrefix, actualState.InstructionPrefix);
            Assert.AreEqual(expectedState.InterruptMasterEnable, actualState.InterruptMasterEnable);
            Assert.AreEqual(expectedState.ImeScheduled, actualState.ImeScheduled);
        }
    }
}
