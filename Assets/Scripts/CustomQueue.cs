using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomQueue<T>
{

    public class Node<T>
    {
        public Node<T> next;
        public T value;
        public Node()
        {
            this.next = null;
        }
        public Node(T value)
        {
            this.next = null;
            this.value = value;
        }
    }

    public Node<T> head;
    public Node<T> tail;
    public int length;
    public List<T> values;

    public CustomQueue()
    {
        this.head = null;
        this.tail = null;
        this.length = 0;
        this.values = new List<T>();
    }


    public void Enqueue(T element)
    {
        // Increment legnth
        this.length += 1;

        Node<T> newNode = new Node<T>(element);

        // If queue is empty, then new node is front and rear both  
        if (this.tail == null)
        {
            this.head = this.tail = newNode;
        }
        else
        {
            // Add the new node at the end of queue and change rear  
            this.tail.next = newNode;
            this.tail = newNode;
        }
    }

 
    public T Dequeue()
    {
        // Decrement length.
        this.length = length > 0 ? length -= 1 : length = 0;

        // If queue is empty, throw exception.
        if (this.head == null)
        {
            throw new Exception("Q is empty!");
        }

        // Store previous front and move front one node ahead  
        Node<T> temp = this.head;
        this.head = this.head.next;

        // If front becomes null, then change rear also as NULL  
        if (this.head == null)
        {
            this.tail = null;
        }

        return temp.value;
    }

    public T Get(int index, bool debug=false)
    {
        // Store iteration counter:
        var counter = 0;
        // Linear search from the head of the linkedList:
        var curr_node = this.head;
        while (curr_node != null)
        {
            if (index == counter)
            {
                if (debug)
                {
                    Debug.Log("index == counter");
                    Debug.Log(this.head.value);
                    Debug.Log(curr_node.value);
                }
                return curr_node.value;
            }
            counter += 1;
            // next node
            curr_node = curr_node.next;
        }
        return default(T);
    }

    public string PrintQueueValues()
    {
        string valuesStr = "Values: ";
        foreach(var value in this.values)
        {
            valuesStr += value + " | ";
        }
        return valuesStr;
    }

}
