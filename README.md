# FFT.CRC32

[![NuGet package](https://img.shields.io/nuget/v/FFT.CRC.svg)](https://nuget.org/packages/FFT.CRC)

A fast, allocation-free implementation for incrementally calculating a CRC32 value on a stream of data slices as each slice becomes available.

## Features

1. [`CRC32Builder`](#crc32builder): Allows you to add data one slice at a time as it becomes available, keeping a running total of the CRC32 calculation. Once all data has arrived, the final CRC32 value is provided.

1. [`CRC32Calculator`](#crc32calculator): Exposes low-level methods for making your own CRC32 calculations without the `CRC32Builder` feature.

## CRC32Builder

**Initialization**

This is a struct. You must initialize it before you use it. It does NOT self-initialize and it does not check to make sure you have initialized it, because it is optimized for speed, not checking your mistakes.

The best way to obtain an initialized instance is to use the static `CRC32Builder.CreateNew()` method.

```csharp
using FFT.CRC32;

// Example 1.
CRC32Builder builder; // NOT INITIALIZED!!
builder.Initialize(); // ok now it is ready for use.

// Example 2.
var builder = new CRC32Builder(); // NOT INITIALIZED!!
builder.Initialize(); // ok now it is ready for use.

// Example 3.
var builder = CRC32Builder.CreateNew(); // immediately ready for use!
```

**Use**

Use the `CRC32Builder.Add(Span<byte> data)` method as many times as needed until all data has been received and calculated.

Use the `CRC32Builder.Value` property to get the CRC value for all the data that has been added so far.

Optionally use the `CRC32Builder.Initalize()` method to reset the builder to start a new CRC calculation.

The example `CRCAppender` class below shows one way you might use a `CRC32Builder`. 

```csharp

/// <summary>
/// Keeps a running calculation of the CRC of the data written to the inner stream,
/// and writes the CRC value to the inner stream when required. The CRC calculation is
/// reset after the CRC value is written to the inner stream.
/// </summary>
public struct CRCAppender
{
    /// <summary>
    /// The stream to be written to.
    /// </summary>
    private readonly Stream InnerStream;

    /// <summary>
    /// The builder that keeps track of the CRC value as each slice of data becomes available
    /// </summary>
    private readonly CRC32Builder CRCBuilder;

    /// <summary>
    /// Creates a new <see cref="CRCAppender"/> struct with the given inner stream.
    /// </summary>
    /// <param name="innerStream">The stream to be written to.</param>
    public CRCAppender(Stream innerStream)
    {
        this.InnerStream = innerStream;
        this.CRCBuilder = CRC32Builder.Create(); // gets an INITIALIZED builder.
    }

    /// <summary>
    /// Writes the given data to the inner stream, keeping track of the running CRC calculation.
    /// </summary>
    /// <param name="includeCRC">
    /// When true, the CRC value will also be written to the inner stream, and the CRC calculation
    /// will be reset ready to begin calculating a new CRC value for future incoming data.
    /// </param>
    public void Write(byte[] buffer, int offset, int count, bool includeCRC)
    {
        // add the new data to the CRC calculation
        this.CRCBuilder.Add(buffer.AsSpan().Slice(offset, count));
        // write the new data to the inner stream.
        this.InnerStream.Write(buffer, offset, count);
        // when it's necessary to finalize the crc calculation and append it to the inner stream:
        if (includeCRC)
        {
            // write the crc value to the inner stream
            this.InnerStream.Write(BitConverter.GetBytes(this.CRCBuilder.Value), 0, 4);
            // and reset the crc calculation, ready to begin again with new data as it arrives.
            this.CRCBuilder.Initialize();
        }
    }
}
```

## CRC32Calculator

Low-level CRC32 calculation methods.

See class documentation.