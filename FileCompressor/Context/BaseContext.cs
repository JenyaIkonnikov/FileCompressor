﻿using FileCompressor.Models;
using System;
using System.IO;

namespace FileCompressor.Context
{
    public abstract class BaseContext<TRead, TWrite> : IDisposable
        where TRead : BaseChunk
        where TWrite : BaseChunk
    {
        public const int BufferSize = 1024 * 1024 * 32;

        protected readonly FileStream InStream;
        protected readonly FileStream ToStream;

        public readonly int PartitionsCount;

        public BaseContext(string inFilePath, string toFilePath)
        {
            InStream = File.OpenRead(inFilePath);
            ToStream = File.Create(toFilePath);
            PartitionsCount = InitialPartitionsCount();
        }

        protected long LeftBytes => InStream.Length - InStream.Position;

        protected abstract int InitialPartitionsCount();

        public abstract TRead ReadChunk();
        public abstract TWrite ConvertReadToWriteModel(TRead readChunk);
        public abstract void WriteChunk(TWrite chunk);

        public void Dispose()
        {
            InStream.Close();
            ToStream.Close();
        }
    }
}
