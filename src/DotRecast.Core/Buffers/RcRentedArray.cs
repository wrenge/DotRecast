﻿using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace DotRecast.Core.Buffers
{
    public static class RcRentedArray
    {
        public static RcRentedArray<T> RentDisposableArray<T>(int minimumLength)
        {
            var array = ArrayPool<T>.Shared.Rent(minimumLength);
            return new RcRentedArray<T>(ArrayPool<T>.Shared, array, minimumLength);
        }
    }

    public class RcRentedArray<T> : IDisposable
    {
        private ArrayPool<T> _owner;
        public T[] RentedArray;
        private bool _disposed;

        public int Length { get; }
        public bool IsDisposed => _disposed;

        internal RcRentedArray(ArrayPool<T> owner, T[] array, int length)
        {
            _owner = owner;
            RentedArray = array;
            Length = length;
            _disposed = false;
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowHelper.ThrowExceptionIfIndexOutOfRange(index, Length);
                return ref RentedArray[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] AsRentedArray()
        {
            return RentedArray;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _owner?.Return(RentedArray, true);
            RentedArray = null;
            _owner = null;
        }
    }
}