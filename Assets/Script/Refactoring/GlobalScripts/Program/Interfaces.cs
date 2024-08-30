using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIndexer<T>
{
    T this[int index] { get; set; }
}
