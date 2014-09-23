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
            mArray = new T[0];
        }
        
        public void Add(T item) {
           //현재 배열의 사이즈 고려하여 아이템 추가
           T[] arrayB = new T[mArray.Length+1];
           arrayB[mArray.Length] = item;
           for (int i = 0; i < mArray.Length; i++)
           {
               arrayB[i] = mArray[i];
           }
           mArray = arrayB;
           //updateArray(mArray, mArray.Length);
        }


       /* private void updateArray(T[] array, int length)
        {
            T[] tempArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                if (i < array.Length)
                {
                    tempArray[i] = array[i];
                }
                else
                {
                    tempArray[i] = default(T);
                }
            }

            mArray = new T[length + 1];
            for (int i = 0; i < length; i++)
            {
                mArray[i] = tempArray[i];
            }
        }
        */

        public IEnumerator GetEnumerator()
        {
            return mArray.GetEnumerator();
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
            get {return mArray.Length; }
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

        //발견된 첫 객체만 remove
        public bool Remove(T item)
        {
            int n = 0;
            bool isFind = false;
            T[] bArray = new T[mArray.Length];

            for (int i = 0; i < mArray.Length; i++)
			{
                if (!mArray[i].Equals(item))
                {
                    //지울 아이템이 아닌 것들로만 배열을 구성한다.
                    bArray[n] = mArray[i];
                    n = n + 1;
                }
                else
                {
                    if (!isFind)
                    {
                        bArray[n] = default(T);
                        isFind = true;
                    }
                    else
                    {
                        bArray[n] = mArray[i];
                        n = n + 1;
                    }
                }
			}
            //n은 유효한 아이템 갯수
            T[] cArray = new T[n]; 
            for (int i = 0; i < n; i++)
            {
                cArray[i] = bArray[i];
            }
            mArray = cArray;

            return true;
        }
        private T GetItem(int index){
            return mArray[index];
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
             mArray[index] = item;
        }

        public void RemoveAt(int index)
        {
           mArray[index] = default(T);
        }

        public T this[int index, T item] 
        {
            set 
            {
                mArray[index] = item;
            }
        }

        public T this[int index]
        {
            get
            {
                //YounList[0] 반환시.
                return mArray[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
