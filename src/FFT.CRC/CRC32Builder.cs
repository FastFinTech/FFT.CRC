// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace FFT.CRC
{
    /// <summary>
    /// <para>
    /// Use this struct to incrementally calculate the running CRC32 value
    /// of a stream of data as each new <see cref="Span{T}"/> becomes available.
    /// When finished calculating a CRC32 value, you can <see cref="Initialize"/> this
    /// instance and reuse it.
    /// </para>
    /// <para>
    /// This is a MUTATING struct, so make sure you pass it into methods using BYREF when you need mutations to affect calling code.
    /// </para>
    /// <para>
    /// This stuct MUST BE INITIALIZED before use with the <see cref="Initialize"/> method.
    /// Alternatively, you can obtain an already-initialized value with the static <see cref="CreateInitialized"/> method.
    /// </para>
    /// </summary>
    public struct CRC32Builder
    {
        private uint value;

        /// <summary>
        /// Resets internal values ready to calculate a new CRC32 value from the beginning of a new stream of data.
        /// Never use a <see cref="CRC32Builder"/> that has not been initialized via this method or
        /// obtained via the static <see cref="CreateInitialized"/> method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize()
        {
            this.value = CRC32Calculator.SEED;
        }

        /// <summary>
        /// Updates <see cref="Value"/> with the new data.
        /// You may call this method as many times as you like until all the data has arrived.
        /// </summary>
        /// <param name="bytes">The new data to "append" to the CRC32 calculation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ReadOnlySpan<byte> bytes)
        {
            CRC32Calculator.Add(ref this.value, bytes);
        }

        /// <summary>
        /// Retrieves the CRC32 value calculated for the data that has been added so far.
        /// After retrieving this value, you can continue adding more data and again retrive a new value,
        /// or you can <see cref="Initialize"/> the builder to start a new calculation from scratch.
        /// </summary>
        public uint Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return CRC32Calculator.Finalize(this.value);
            }
        }

        /// <summary>
        /// Creates an initialized <see cref="CRC32Builder"/> ready to have data added via the <see cref="Add(Span{T})"/> method.
        /// </summary>
        public static CRC32Builder CreateInitialized() => new CRC32Builder
        {
            value = CRC32Calculator.SEED,
        };
    }
}
