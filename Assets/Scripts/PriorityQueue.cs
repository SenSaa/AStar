using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T>
{

    public class Node<T>
    {
        public Node<T> next;
        public T value;
        public float priority;

        public Node()
        {
            this.next = null;
        }

        public Node(T value, float priority)
        {
            this.next = null;
            this.value = value;
            this.priority = priority;
        }
    }


    public List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

    public PriorityQueue() { }

    public int Length
    {
        get { return elements.Count; }
    }

    public void Enqueue(T element, float priority)
    {
        elements.Add(Tuple.Create(element, priority));
    }

    public T Dequeue()
    {
        Comparer<float> comparer = Comparer<float>.Default;
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (comparer.Compare(elements[i].Item2, elements[bestIndex].Item2) < 0)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

}
