using astar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQ : MonoBehaviour
{



    void Start()
    {
        TestIntQ();
        TestStringQ();
        TestNodesQ();
    }

    private void TestIntQ()
    {
        CustomQueue<int> q = new CustomQueue<int>();
        Debug.Log(q.Get(1));
        q.Enqueue(1);
        q.Enqueue(2);
        q.Enqueue(3);
        Debug.Log(q.PrintQueueValues());
        Debug.Log("h: " + q.head.value);
        Debug.Log("t: " + q.tail.value);
        Debug.Log(q.Get(1));
    }

    private void TestStringQ()
    {
        CustomQueue<string> q = new CustomQueue<string>();
        q.Enqueue("A");
        q.Enqueue("B");
        q.Enqueue("C");
        Debug.Log(q.PrintQueueValues());
        Debug.Log("h: " + q.head.value);
        Debug.Log("t: " + q.tail.value);
    }

    private void TestNodesQ()
    {
        // Create a Queue of Nodes.
        Debug.Log("Create a Queue of Nodes.");
        CustomQueue<Node> openSet = new CustomQueue<Node>();
        Debug.Log("------------------------------------");

        // Test Getting Elements of empty Q.
        Debug.Log("Test Getting Elements of empty Q.");
        for (int i = 0; i < openSet.length; i++)
        {
            Debug.Log(openSet.Get(i).GetNodePosition());
        }
        Debug.Log("------------------------------------");

        // Test Adding Elements of empty Q.
        Debug.Log("Test Adding Elements of empty Q.");
        openSet.Enqueue(new Node((0,0)));
        openSet.Enqueue(new Node((1, 0)));
        openSet.Enqueue(new Node((0, 1)));

        // Test Getting Elements of non-empty Q.
        Debug.Log("Test Getting Elements of non-empty Q.");
        for (int i=0; i < openSet.length; i++)
        {
            Debug.Log(openSet.Get(i).GetNodePosition());
        }
        Debug.Log("------------------------------------");

        // Test Removing all Elements of Q.
        Debug.Log("Test Removing all Elements of Q.");
        //openSet.Dequeue(new Node((0, 0)));
        //openSet.Dequeue(new Node((1, 0)));
        //openSet.Dequeue(new Node((0, 1)));
        Debug.Log("tail: " + openSet.tail.value.GetNodePosition());
        openSet.Dequeue();
        Debug.Log("head: " + openSet.head.value.GetNodePosition());
        Debug.Log("tail: " + openSet.tail.value.GetNodePosition());
        openSet.Dequeue();
        openSet.Dequeue();
        Debug.Log("------------------------------------");

        // Test Getting Elements of emptied Q.
        Debug.Log("Test Getting Elements of emptied Q.");
        for (int i = 0; i < openSet.length; i++)
        {
            Debug.Log(openSet.Get(i).GetNodePosition());
        }
        Debug.Log("------------------------------------");


        // --
        // **


        // Test Adding Elements to emptied Q.
        Debug.Log("Test Adding Elements to emptied Q.");
        openSet.Enqueue(new Node((2, 1)));
        openSet.Enqueue(new Node((1, 2)));

        // Test Getting Elements of re-Dequeueulated Q.
        Debug.Log("Test Getting Elements of re-Dequeueulated Q.");
        for (int i = 0; i < openSet.length; i++)
        {
            Debug.Log(openSet.Get(i).GetNodePosition());
        }
        Debug.Log("------------------------------------");

        // Test Removing Elements of re-Dequeueulated Q.
        Debug.Log("Test Removing Elements of re-Dequeueulated Q.");
        openSet.Dequeue();

        // Test Getting Elements after removing an element from Q.
        Debug.Log("Test Getting Elements after removing an element from Q.");
        for (int i = 0; i < openSet.length; i++)
        {
            Debug.Log(openSet.Get(i).GetNodePosition());
        }
        Debug.Log("------------------------------------");

        // Test Removing last Element from Q.
        Debug.Log("Test Removing last Element from Q.");
        openSet.Dequeue();

        // Test Getting Elements from empty Q.
        Debug.Log("Test Getting Elements from empty Q.");
        for (int i = 0; i < openSet.length; i++)
        {
            Debug.Log(openSet.Get(i).GetNodePosition());
        }
        Debug.Log("------------------------------------");
    }

}
