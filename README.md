# FFT.CRC32

[![NuGet package](https://img.shields.io/nuget/v/FFT.CRC.svg)](https://nuget.org/packages/FFT.CRC)

A fast, allocation-free implementation for incrementally calculating a CRC32 value on a stream of data slices as each slice becomes available.

## Features

1. [`CRC32Builder`](#crc32builder): Allows you to add data one slice at a time as it becomes available, keeping a running total of the CRC32 calculation. Once all data has arrived, the final CRC32 value is provided.

1. [`CRC32Calculator`](#crc32calculator): Exposes low-level methods for making your own CRC32 calculations without the `CRC32Builder` feature.

## CRC32Builder

**Quick start**
```csharp
using FFT.CRC;

// in the real world, instead of this demo data, 
// you would most likely be receiving your data spans from
// some kind of stream
Span<byte> demoData = new byte[] {1,2,3,4,5,6,7,8,9};

// get an INITIALIZED builder struct
var crcBuilder = CRC32Builder.CreateNew();

// add two data slices
crcBuilder.Add(demoData.Slice(0,2));
crcBuilder.Add(demoData.Slice(2,2));

// get the CRC32 value for all data added so far, if you need to
var crcValue = crcBuilder.Value;

// add some more data if you want to
crcBuilder.Add(demoData.Slice(4,2));

// get the updated CRC32 value
crcValue = crcBuilder.Value;

// optionally reset the builder ready for new data
crcBuilder.Initialize();
```

**Usage notes -- Initialization**

This is a mutable struct that requires initialization. It is optimized for speed and does not contain any guards against developer carelessness such as forgetting to initialize it, or not understanding the intricacies of storing and passing a mutable struct in c#.

The best way to obtain an initialized instance is to use the static `CRC32Builder.CreateNew()` method.

```csharp
using FFT.CRC32;

// Example 1.
CRC32Builder builder; // NOT INITIALIZED!!
builder.Initialize(); // ok now it is ready for use.

// Example 2.
var builder = new CRC32Builder(); // NOT INITIALIZED!!
builder.Initialize(); // ok now it is ready for use.

// Example 3 - recommended.
var builder = CRC32Builder.CreateNew(); // immediately ready for use!
```

You can call the builder's `Initialize()` method at any time to reset the calculation to accept new data from the beginning.

## CRC32Calculator

Low-level CRC32 calculation methods.

See class documentation.