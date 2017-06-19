using System.Collections;
using System.Collections.Generic;

public class SemiStack<T>
{
    private List<T> items = new List<T>();
    public void Push(T item)
    {
        items.Add(item);
    }
    public T Peek()
    {
        if (Count > 0)
            return items[Count - 1];
        else return default(T);
    }
    public T Pop()
    {
        if (items.Count > 0)
        {
            T t = items[Count - 1];
            items.RemoveAt(Count - 1);
            return t;
        }
        else return default(T);
    }
    public List<T> Remove(T t)
    {
        List<T> ret = new List<T>();
        for (int i = Count - 1; i >= 0; i--)
        {
            if (items[i].Equals(t))
            {
                ret.Add(items[i]);
                items.RemoveAt(i);
            }
        }
        return ret;
    }
    public int Count
    {
        get { return items.Count; }
    }
}