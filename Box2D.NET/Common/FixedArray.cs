using System;
using System.Collections;
using System.Linq;

namespace Box2D.NET.Common;

public sealed class FixedArray<T> : IEnumerable
{
    private readonly T[] _array; // Internal array to store elements
    private int _length; // Tracks the number of valid elements

    /// <summary>
    /// Initializes a new instance of the FixedArray class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The fixed size of the array.</param>
    public FixedArray(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");
        }

        _array = new T[capacity];
        _length = 0;
    }

    /// <summary>
    /// Gets the maximum capacity of the array.
    /// </summary>
    public int Capacity => _array.Length;

    /// <summary>
    /// Gets or sets the number of valid elements in the array.
    /// </summary>
    public int Length
    {
        get => _length;
        set
        {
            if (value < 0 || value > _array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Length must be between 0 and Capacity.");
            }

            _length = value;
        }
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            return _array[index];
        }
        set
        {
            if (index < 0 || index >= _length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            _array[index] = value;
        }
    }

    /// <summary>
    /// Gets or sets the element at the specified index using System.Index.
    /// </summary>
    /// <param name="index">The index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[Index index]
    {
        get => this[index.GetOffset(_length)];
        set => this[index.GetOffset(_length)] = value;
    }

    /// <summary>
    /// Gets a slice of the array using System.Range.
    /// </summary>
    /// <param name="range">The range to slice.</param>
    /// <returns>A new array containing the sliced elements.</returns>
    public T[] this[Range range]
    {
        get
        {
            (int start, int length) = range.GetOffsetAndLength(_length);
            T[] slice = new T[length];
            Array.Copy(_array, start, slice, 0, length);
            return slice;
        }
    }

    public IEnumerator GetEnumerator() => _array.GetEnumerator();

    public bool SequenceEqual(FixedArray<T> other) => _array.SequenceEqual(other._array);
}