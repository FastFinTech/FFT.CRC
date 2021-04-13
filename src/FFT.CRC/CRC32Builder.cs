// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace FFT.CRC
{
    /// <summary>
    /// Use this struct to incrementally calculate the running CRC32 value of a
    /// stream of data as each new data segment becomes available.
    /// </summary>
    /// <remarks>
    /// This struct MUST BE INITIALIZED before use with the <see
    /// cref="Initialize"/> method, or you can obtain an already-initialized
    /// value with the static <see cref="InitializedValue"/> property.
    /// </remarks>
    public struct CRC32Builder
    {
        private uint value;

        /// <summary>
        /// Gets an initialized <see cref="CRC32Builder"/>.
        /// </summary>
        public static CRC32Builder InitializedValue { get; } = new CRC32Builder
        {
            value = CRC32Calculator.SEED,
        };

        /// <summary>
        /// Resets internal values ready to calculate a new CRC32 value from the
        /// beginning of a new stream of data. Always initialize a <see
        /// cref="CRC32Builder"/> with this method before use, or obtain an
        /// initial value via the static <see cref="InitializedValue"/>
        /// property.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize()
            => this.value = CRC32Calculator.SEED;

        /// <summary>
        /// Updates <see cref="Value"/> with the new data. You may call this
        /// method as many times as you like until all the data has arrived.
        /// </summary>
        /// <param name="bytes">The new data segment to "append" to the CRC32
        /// calculation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ReadOnlySpan<byte> bytes)
            => CRC32Calculator.Add(ref this.value, bytes);

        /// <summary>
        /// Gets a finalized CRC32 value for the data that has been added so
        /// far.
        /// </summary>
        public uint Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => CRC32Calculator.Finalize(this.value);
        }
    }
}
