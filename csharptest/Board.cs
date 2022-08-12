using System;
using System.Collections.Generic;
using System.Text;
namespace csharptest
{
    class MyList<T>
    {
        const int DEFAULT_SIZE = 1;
        T[] _data = new T[DEFAULT_SIZE];

        public int Count = 0; // data used
        public int Capacity { get { return _data.Length; } } // data reserved

        public void Add(T item)
        {
            // check available space
            if (Count >= Capacity)
            {
                // 공간을 확보한다
                T[] newArray = new T[Count * 2];
                for (int i = 0; i < Count; i++)
                    newArray[i] = _data[i];
                _data = newArray;
            }
            // data add
            _data[Count] = item;
            Count++;

        }

        public T this[int index] 
        {
            get { return _data[index]; } 
            set { _data[index] = value; }
        }

        public void RemoveAt(int index)
        {
            for (int i = index; i < Count - 1; i++)
            {
                _data[i] = _data[i + 1];
            }
            _data[Count - 1] = default(T); // 마지막 배열의 원소를 없애준다
            Count--;
        }
    }
    class Board
    {
        public int[] _data = new int[25]; // 배열
        public List<int> _data2 = new List<int>(); // 동적배열  
        public LinkedList<int> _data3 = new LinkedList<int>(); // 연결리스트

        public void Initialize()
        {
            _data2.Add(101);
            _data2.Add(102);
            _data2.Add(103);
            _data2.Add(104);
            _data2.Add(105);
            int temp = _data2[2];

            _data2.RemoveAt(2); // remove index value

        }
    }
}
