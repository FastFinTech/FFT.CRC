// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace FFT.CRC
{
    /// <summary>
    /// Provides access to low-level CRC32 calculation methods.
    /// </summary>
    public static class CRC32Calculator
    {
        /// <summary>
        /// The starting value of a CRC32 calculation.
        /// </summary>
        public const uint SEED = 0xFFFFFFFF;

        private static readonly uint[] table;

        static CRC32Calculator()
        {
            const uint POLYNOMIAL = 0xEDB88320;
            table = new uint[256];
            for (var i = 0U; i < 256U; i++)
            {
                var value = i;
                for (var j = 0; j < 8; j++)
                {
                    if ((value & 1) == 1)
                    {
                        value = (value >> 1) ^ POLYNOMIAL;
                    }
                    else
                    {
                        value >>= 1;
                    }
                }
                table[i] = value;
            }
        }

        /// <summary>
        /// Updates the running CRC32 calculation <paramref name="value"/>.
        /// Call this method each time new data arrives, passing in the new <paramref name="bytes"/>.
        /// You are responsible to maintain your own copy of <paramref name="value"/> until the
        /// calculation process is complete.
        /// </summary>
        /// <param name="value">The running CRC32 calculation value which will be updated.</param>
        /// <param name="bytes">The new data segment to "append" to the CRC32 calculation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Add(ref uint value, ReadOnlySpan<byte> bytes)
        {
            // *************************************************************
            //        SIMPLE BUT SLOWER VERSION
            // *************************************************************
            //foreach (var b in bytes)
            //    value = (value >> 8) ^ table[(value ^ b) & 0xFF];
            // *************************************************************

            // Use the unsafe pointers to eliminate array bounds checking and creation of an enumerator
            // From benchmark testing, we find it saves 2 microseconds for 1024 CRC calculations composed of 3 spans of 3 bytes each.
            var length = bytes.Length;
            fixed (uint* t = table)
            fixed (byte* b = bytes)
                for (var i = 0; i < length; i++)
                    value = (value >> 8) ^ t[(value ^ b[i]) & 0xFF];
        }

        /// <summary>
        /// After adding all the data you have received, call this method to obtain
        /// the final CRC32 calculation result.
        /// </summary>
        /// <param name="value">The running CRC32 calculation value.</param>
        /// <returns>The finalized CRC32 calculation result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Finalize(uint value)
            => value ^ SEED;
    }
}
