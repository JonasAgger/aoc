namespace AdventOfCode.Library;

public class Deque<T>
{
    private T[] array = new T[1024];

    private int index = 0;

    public void Add(T entry)
    {
        array[index] = entry;
        index += 1;
        ResizeIfNeeded();
    }

    private void ResizeIfNeeded()
    {
        if (index == array.Length)
        {
            var newArray = new T[array.Length * 2];
            array.CopyTo(newArray.AsSpan());
            array = newArray;
        }
    }

    public T? Pop()
    {
        return index == 0 ? default : array[--index];
    }
}