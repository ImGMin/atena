[System.Serializable]
public class ArrayData<T>
{
    public T[] array;

    public ArrayData(int size)
    {
        array = new T[size];
    }
}
