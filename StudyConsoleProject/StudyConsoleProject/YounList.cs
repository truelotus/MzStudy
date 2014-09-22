using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace StudyConsoleProject
{
    public class YounList<T>: IList<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        T[] mArray = null;

        public YounList()
        {
            mArray = new T[1];
        }
        
        public void Add(T item) {
            //현재 배열의 사이즈 고려하여 아이템 추가
            mArray[mArray.Length - 1] = item;
           //updateArray(mArray, mArray.Length);
        }

        
        /*private void updateArray(T[] arr,int length)
        {
            arr = new T[length];
            for (int i = 0; i < length; i++)
            {
                //사이즈보다 작으면
                if (i<length)
                {
                    
                }
            }
        }*/


        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }


        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
