using System;
using System.Collections.Generic;
using System.Text;
namespace csharptest
{
    class MyLinkedListNode<T>
    {
        public T Data;
        public MyLinkedListNode<T> Next;
        public MyLinkedListNode<T> Prev;

    }

    class MyLinkedList<T>
    {
        public MyLinkedListNode<T> Head = null; // First room
        public MyLinkedListNode<T> Tail = null; // Last room
        public int Count = 0;

        public MyLinkedListNode<T> AddLast(T data)
        {
            MyLinkedListNode<T> newRoom = new MyLinkedListNode<T>();
            newRoom.Data = data;

            // if no head in list
            if (Head == null)
                Head = newRoom;

            // if tail in list
            if (Tail != null)
            {
                Tail.Next = newRoom;
                newRoom.Prev = Tail;
            }

            Tail = newRoom;
            Count++;
            return newRoom;
        }
        public void Remove(MyLinkedListNode<T> room)
        {
            if (Head == room)
                Head = Head.Next;

            if (Tail == room)
                Tail = Tail.Prev;

            if (room.Prev != null)
                room.Prev.Next = room.Next;

            if (room.Next != null)
                room.Next.Prev = room.Prev;

            Count--;


        }
    }

    class Board
    {
        public int[] _data = new int[25]; // 배열
        public MyLinkedList<int> _data3 = new MyLinkedList<int>(); // 연결리스트

        public void Initialize()
        {
            _data3.AddLast(101);
            _data3.AddLast(102);
            MyLinkedListNode<int> node = _data3.AddLast(103);
            _data3.AddLast(104);
            _data3.AddLast(105);
            _data3.Remove(node);

        }
    }
}