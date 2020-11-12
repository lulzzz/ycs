﻿// ------------------------------------------------------------------------------
//  <copyright company="Microsoft Corporation">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace Ycs
{
    public sealed class IntDiffOptRleDecoder : AbstractStreamDecoder<int>
    {
        private int _state;
        private uint _count;
        private int _diff;

        public IntDiffOptRleDecoder(Stream input, bool leaveOpen = false)
            : base(input, leaveOpen)
        {
            // Do nothing.
        }

        public override int Read()
        {
            if (_count == 0)
            {
                int diff = Reader.ReadVarInt().value;

                // If the first bit is set, we read more data.
                bool hasCount = (diff & Bit.Bit1) > 0;

                if (diff < 0)
                {
                    _diff = -((-diff) >> 1);
                }
                else
                {
                    _diff = diff >> 1;
                }

                _count = hasCount ? Reader.ReadVarUint() + 2 : 1;
            }

            _state += _diff;
            _count--;

            return _state;
        }
    }
}