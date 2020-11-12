﻿// ------------------------------------------------------------------------------
//  <copyright company="Microsoft Corporation">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace Ycs
{
    public sealed class UintOptRleDecoder : AbstractStreamDecoder<uint>
    {
        private uint _state;
        private uint _count;

        public UintOptRleDecoder(Stream input, bool leaveOpen = false)
            : base(input, leaveOpen)
        {
            Console.WriteLine($"uint decoder input, {input.Length}: {string.Join(",", (input as MemoryStream).ToArray())}");
            // Do nothing.
        }

        public override uint Read()
        {
            if (_count == 0)
            {
                var v = Reader.ReadVarInt();

                // If the sign is negative, we read the count too; otherwise, count is 1.
                bool isNegative = v.sign < 0;
                if (isNegative)
                {
                    _state = (uint)(-v.value);
                    _count = Reader.ReadVarUint() + 2;
                }
                else
                {
                    _state = (uint)v.value;
                    _count = 1;
                }
            }

            _count--;
            return _state;
        }
    }
}