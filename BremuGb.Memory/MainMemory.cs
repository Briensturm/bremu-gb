
using System;
using System.Collections.Generic;

namespace BremuGb.Memory
{
    public class MainMemory : IRandomAccessMemory
    {
        private Dictionary<ushort, IMemoryAccessDelegate> _writeDelegates = new Dictionary<ushort, IMemoryAccessDelegate>();
        private Dictionary<ushort, IMemoryAccessDelegate> _readDelegates = new Dictionary<ushort, IMemoryAccessDelegate>();

        private Dictionary<ushort, byte> _undelegatedMemory = new Dictionary<ushort, byte>();

        //remove later
        public MainMemory()
        {
            //sample program
            _undelegatedMemory.Add(0x0100, 0x00);
            _undelegatedMemory.Add(0x0101, 0x48);
            _undelegatedMemory.Add(0x0102, 0xC3);
            _undelegatedMemory.Add(0x0103, 0x00);
            _undelegatedMemory.Add(0x0104, 0x01);
        }

        public byte ReadByte(ushort address)
        {
            if(_readDelegates.TryGetValue(address, out IMemoryAccessDelegate readDelegate))
                return readDelegate.DelegateMemoryRead(address);

            if (_undelegatedMemory.TryGetValue(address, out byte data))
                return data;

            throw new InvalidOperationException($"Unknown memory address 0x{address:X4}");
        }

        public void WriteByte(ushort address, byte data)
        {
            if (_writeDelegates.TryGetValue(address, out IMemoryAccessDelegate writeDelegate))
                writeDelegate.DelegateMemoryRead(address);

            _undelegatedMemory[address] = data;
        }

        public void RegisterMemoryAccessDelegateWrite(ushort address, IMemoryAccessDelegate memDelegate)
        {
            if (_writeDelegates.ContainsKey(address))
                throw new InvalidOperationException("Cannot register multiple memory delegates per address");

            _writeDelegates.Add(address, memDelegate);
        }

        public void RegisterMemoryAccessDelegateRead(ushort address, IMemoryAccessDelegate memDelegate)
        {
            if (_readDelegates.ContainsKey(address))
                throw new InvalidOperationException("Cannot register multiple memory delegates per address");

            _readDelegates.Add(address, memDelegate);
        }

        public void RegisterMemoryAccessDelegateWriteRange(ushort addressLo, ushort addressHi, IMemoryAccessDelegate memDelegate)
        {
            if (memDelegate == null)
                throw new ArgumentNullException(nameof(memDelegate));
            if (addressLo > addressHi)
                throw new InvalidOperationException($"Invalid address range: {addressLo:X4} - {addressHi:X4}");

            for (ushort address = addressLo; address <= addressHi; address++)
                RegisterMemoryAccessDelegateWrite(address, memDelegate);
        }

        public void RegisterMemoryAccessDelegateReadRange(ushort addressLo, ushort addressHi, IMemoryAccessDelegate memDelegate)
        {
            if (memDelegate == null)
                throw new ArgumentNullException(nameof(memDelegate));
            if (addressLo > addressHi)
                throw new InvalidOperationException($"Invalid address range: {addressLo:X4} - {addressHi:X4}");

            for (ushort address = addressLo; address <= addressHi; address++)
                RegisterMemoryAccessDelegateRead(address, memDelegate);
        }
    }
}
