# FFT.CRC

A fast, allocation-free implementation for incrementally calculating a CRC32 value on a stream of data slices as each slice becomes available.

[![Source code](https://img.shields.io/static/v1?label=github&message=main&logo=github&color=blue)](https://github.com/FastFinTech/FFT.CRC "View the source code")
[![NuGet package](https://img.shields.io/nuget/v/FFT.CRC.svg)](https://nuget.org/packages/FFT.CRC "View the nuget package")

## Features

1. [`CRC32Builder`](xref:FFT.CRC.CRC32Builder): Allows you to add data one slice at a time as it becomes available, keeping a running total of the CRC32 calculation. Once all data has arrived, the final CRC32 value is provided.

1. [`CRC32Calculator`](xref:FFT.CRC.CRC32Calculator): Exposes low-level methods for making your own CRC32 calculations without the `CRC32Builder` feature.

# Quickstarts

# [CRC32Builder](#tab/CRC32Builder)

[See reference](xref:FFT.CRC.CRC32Builder)

```csharp
using System;
using FFT.CRC;
// get an INITIALIZED builder struct
var crcBuilder = CRC32Builder.InitializedValue;
// add each data segment
foreach(ReadOnlySpan<byte> segment in data)
  crcBuilder.Add(segment);
// get the final calculation result
var crcValue = crcBuilder.Value;
// optionally reset the builder ready for new data
crcBuilder.Initialize();
```

# [CRC32Calculator](#tab/CRC32Calculator)

[See reference](xref:FFT.CRC.CRC32Calculator)

```csharp
using System;
using FFT.CRC;
// initialize the CRC calculation value
var crcValue = CRC32Calculator.SEED;
// add each data segment, updating our crcValue variable
foreach(ReadOnlySpan<byte> segment in data)
  CRC32Calculator.Add(ref crcValue, segment);
// get the finalized calculation value.
crcValue = CRC32Calculator.Finalize(crcValue);
```

***