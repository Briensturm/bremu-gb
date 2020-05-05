using System;

namespace BremuGb.Cpu
{
    public class CpuRegisters
    {
        public byte A { get; set; }
        public byte F { get; set; }
        public byte B { get; set; }
        public byte C { get; set; }
        public byte D { get; set; }
        public byte E { get; set; }
        public byte H { get; set; }
        public byte L { get; set; }
        public ushort HL
        {
            get
            {
                return (ushort)(H << 8 | L);
            }
            set
            {
                H = (byte)(HL >> 8);
                L = (byte)(HL & 0x0011);
            }
        }

        public ushort this[int index]
        {
            get
            {
                return index switch
                {
                    0 => B,
                    1 => C,
                    2 => D,
                    3 => E,
                    4 => H,
                    5 => L,
                    6 => HL,
                    7 => A,
                    _ => throw new IndexOutOfRangeException("Cpu does not have a register with index " + index),
                };
            }
            set
            {
                switch(index)
                {
                    case 0:
                        B = (byte)value;
                        break;
                    case 1:
                        C = (byte)value;
                        break;
                    case 2:
                        D = (byte)value;
                        break;
                    case 3:
                        E = (byte)value;
                        break;
                    case 4:
                        H = (byte)value;
                        break;
                    case 5:
                        L = (byte)value;
                        break;
                    case 6:
                        HL = value;
                        break;
                    case 7:
                        A = (byte)value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Cpu does not have a register with index " + index);

                }
            }
        }

        public void Reset()
        {
            A = 0x01;
            F = 0xB0;
            B = 0x00;
            C = 0x13;
            D = 0x00;
            E = 0xD8;
            H = 0x01;
            L = 0x4D;
        }
    }
}
