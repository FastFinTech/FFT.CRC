# FFT.CRC32

[![NuGet package](https://img.shields.io/nuget/v/FFT.CRC.svg)](https://nuget.org/packages/FFT.CRC)

A fast, allocation-free implementation for incrementally calculating a CRC32 value on a stream of data slices as each slice becomes available.

[Full Documentation](https://fastfintech.github.io/FFT.CRC)

## Features

1. [`CRC32Builder`](#crc32builder): Allows you to add data one slice at a time as it becomes available, keeping a running total of the CRC32 calculation. Once all data has arrived, the final CRC32 value is provided.

1. [`CRC32Calculator`](#crc32calculator): Exposes low-level methods for making your own CRC32 calculations without the `CRC32Builder` feature.

