﻿namespace BremuGb.Audio.SoundChannels
{
    internal class NoiseChannel : SoundChannelBase
    {
        private int _timer = 0;

        private int _lengthCounter;

        private bool _compareLength;

        private int _envelopeTimer = 0;

        private int _initialVolume = 0;

        private int _currentVolume = 0;

        private bool _envelopeIncrease = false;

        private int _envelopePeriod = 0;

        private int _shiftClockFreq = 0;
        private bool _widthMode;
        private int _dividingRatio;

        private ushort _lfsr;
        private int[] _divisorLookup;

        public NoiseChannel()
        {
            _divisorLookup = new int[8] { 8, 16, 32, 48, 64, 80, 96, 112 };
        }

        public byte Envelope
        {
            get
            {
                return (byte)(_initialVolume << 4 |
                                (_envelopeIncrease ? 1 : 0) << 3 |
                                _envelopePeriod);
            }
            internal set
            {
                _initialVolume = (value & 0xF0) >> 4;
                _envelopeIncrease = (value & 0x8) == 0x8;
                _envelopePeriod = value & 0x7;
            }
        }

        public byte InitConsecutive 
        { 
            get
            {
                if (_compareLength)
                    return 0xFF;
                else
                    return 0xBF;
            }

            internal set
            {
                _compareLength = (value & 0x40) == 0x40;

                if((value & 0x80) == 0x80)
                {
                    //handle trigger
                    _timer = _divisorLookup[_dividingRatio] << _shiftClockFreq;

                    _lfsr = 0x7FFF;

                    if (_lengthCounter == 0)
                        _lengthCounter = 64;

                    _currentVolume = _initialVolume;

                    _envelopeTimer = _envelopePeriod;
                }
            }
        }

        public byte PolynomialCounter 
        { 
            get
            {
                return (byte)(_shiftClockFreq << 4 |
                             (_widthMode ? 1 : 0) << 3 |
                              _dividingRatio);
            }

            internal set
            {
                _shiftClockFreq = value >> 4;
                _widthMode = (value & 0x8) == 0x8;
                _dividingRatio = value & 0x7;
            }
        }

        public byte SoundLength 
        { 
            get
            {
                return 0xFF;
            }

            internal set
            {
                _lengthCounter = value & 0x3F;
            }
        }

        public override void AdvanceMachineCycle()
        {
            if (_timer == 0)
                return;

            _timer--;

            if (_timer == 0)
            {
                //advance LFSR
                var newBit = (_lfsr & 0x1) ^ (_lfsr & 0x2);
                _lfsr = (ushort)(_lfsr >> 1);

                if(newBit == 1)
                {
                    _lfsr = (ushort)(_lfsr | 0x4000);
                    if (_widthMode)
                        _lfsr = (ushort)(_lfsr | 0x0040);
                }
                else
                {
                    _lfsr = (byte)(_lfsr & 0x3FFF);
                    if (_widthMode)
                        _lfsr = (ushort)(_lfsr & 0x7FBF);
                }

                //reload timer
                _timer = _divisorLookup[_dividingRatio] << _shiftClockFreq;
            }
        }

        public override void ClockEnvelope()
        {
            if (_envelopeTimer == 0 || _envelopePeriod == 0)
                return;
            _envelopeTimer--;

            if (_envelopeTimer == 0)
            {
                if (_envelopeIncrease && _currentVolume < 0xF)
                    _currentVolume++;
                else if (!_envelopeIncrease && _currentVolume > 0)
                    _currentVolume--;

                _envelopeTimer = _envelopePeriod;
            }
        }

        public override void ClockLength()
        {
            if (_compareLength && _lengthCounter > 0)
                _lengthCounter--;
        }

        public override byte GetSample()
        {
            if (_lengthCounter == 0 || (Envelope & 0xF8) == 0)
                return 0;

            //get output from LFSR
            return (byte)((_lfsr & 0x1) * _currentVolume * 17);
        }
    }
}
