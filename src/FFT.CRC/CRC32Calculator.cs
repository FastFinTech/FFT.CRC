// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace FFT.CRC
{
    /// <summary>
    /// Use this class to perform your own CRC32 calculations without the benefit of the <see cref="CRC32Builder"/>
    /// or the <see cref="CRC32Builder"/>. You will need to maintain your own copy of "value" as you process data.
    /// </summary>
    public static class CRC32Calculator
    {
        /// <summary>
        /// The starting value of a CRC32 calculation.
        /// This should be the value passed to the <see cref="Add(ref uint, ReadOnlySpan{byte})"/> method for the first calculation.
        /// </summary>
        public const uint SEED = 0xFFFFFFFF;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static uint[] table;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Updates <paramref name="value"/> with the new data contained in <paramref name="bytes"/>.
        /// You may call this method as many times as you like until all the data has arrived.
        /// You are responsible to maintain your own copy of <paramref name="value"/> until the
        /// calculation is complete.
        /// </summary>
        /// <param name="value">The running CRC32 calculation value which will be updated.</param>
        /// <param name="bytes">The new data to "append" to the CRC32 calculation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref uint value, ReadOnlySpan<byte> bytes)
        {
            foreach (var b in bytes)
                value = (value >> 8) ^ table[(value ^ b) & 0xFF];
        }

        /// <summary>
        /// Updates <paramref name="value"/> with the new data contained in <paramref name="bytes"/>.
        /// You may call this method as many times as you like until all the data has arrived.
        /// You are responsible to maintain your own copy of <paramref name="value"/> until the
        /// calculation is complete.
        /// </summary>
        /// <param name="value">The running CRC32 calculation value which will be updated.</param>
        /// <param name="bytes">The new data to "append" to the CRC32 calculation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Add(uint value, ReadOnlySpan<byte> bytes)
        {
            foreach (var b in bytes)
                value = (value >> 8) ^ table[(value ^ b) & 0xFF];
            return value;
        }

        /// <summary>
        /// Performs the final calculation after all data has been added.
        /// </summary>
        /// <param name="value">
        /// When passed in, contains the CRC32 calculation value so far.
        /// When the method returns, contains the finalized CRC32 calculation value.
        /// Once finalized, the value can NOT be used for adding more data to the CRC32 calculation.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Finalize(ref uint value)
        {
            value ^= SEED;
        }

        /// <summary>
        /// Performs the final calculation after all data has been added.
        /// </summary>
        /// <param name="value">
        /// When passed in, contains the CRC32 calculation value so far.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Finalize(uint value)
        {
            return value ^ SEED;
        }

        /// <summary>
        /// Calculates the CRC32 checksum of <paramref name="entireData"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Calculate(ReadOnlySequence<byte> entireData)
        {
            var value = SEED;
            foreach (var segment in entireData)
                Add(ref value, segment.Span);
            return Finalize(value);
        }

        [ModuleInitializer]
        internal static void InitializeTable()
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
    }
}
