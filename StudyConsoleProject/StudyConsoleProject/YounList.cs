using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace StudyConsoleProject
{

    public class YounListEnumerator<T> : IEnumerator<T>
    {
        private YounList<T> mArray;
        private T mItem;

        private int mIndex;

        public YounListEnumerator(YounList<T> list) 
        {
            mArray = list;
            mItem = default(T);
            mIndex = -1;

        }
        public T Current
        {
            get { return mItem; }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        //열거자를 컬렉션의 다음 요소로 이동합니다
        public bool MoveNext()
        {
            mIndex = ++mIndex;
            if (mIndex<mArray.Count*2)
            {
                //이동 할 요소가 있으면 next item이 current가 되어야한다.
                mItem = mArray[mIndex/2];
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            mIndex = -1;
        }
    }


    public class YounList<T>: IList<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        T[] mArray = null;

        public YounList()
        {
            mArray = new T[0];
        }
        
        public void Add(T item) {

            Array.Resize(ref mArray, mArray.Length + 1);
            mArray[mArray.Length - 1] = item;

           //현재 배열의 사이즈 고려하여 아이템 추가
           /*T[] arrayB = new T[mArray.Length+1];
           arrayB[mArray.Length] = item;
           for (int i = 0; i < mArray.Length; i++)
           {
               arrayB[i] = mArray[i];
           }
           mArray = arrayB;*/
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
            return new YounListEnumerator<T>(this);
        }

       
        public void CopyTo(Array array, int index)
        {
            Array.Copy(mArray, array, index);
            return;
        }

        public int Count
        {
            get {return mArray.Length; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return mArray; }
        }


        public void Clear()
        {
            Array.Clear(mArray, 0, mArray.Length);
            return;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < mArray.Length; i++)
            {
                if (Array.Equals(mArray[i],item))
                {
                    return true;
                }
            }
            return false;
        }

        //특정 Array 인덱스에서 시작하여 ICollection<T>의 요소를 Array에 복사합니다.
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.ConstrainedCopy(mArray,0,array,arrayIndex,mArray.Length);
            return;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < mArray.Length; i++)
            {
                if (Array.Equals(mArray[i], item))
                {
                    this.RemoveAt(i);
                    break;
                }
            }
            return true;
            /*
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
            mArray = cArray;*/


        }
        private T GetItem(int index){
            return mArray[index];
        }

        public int IndexOf(T item)
        {
            //IList<T>에서 특정 항목의 인덱스를 확인합니다.
            for (int i = 0; i < mArray.Length; i++)
            {
                if (Array.Equals(mArray[i],item))
                {
                    return i;
                }
            }
            return 0;
        }

        public void Insert(int index, T item)
        {
            //인덱스에 들어가고 들어간 자리만큼 한자리 늘어나야한다.
            int n = 0;
            T[] bArray = new T[mArray.Length+1];
            for (int i = 0; i < bArray.Length; i++)
            {
                if (i==index)
                {
                    bArray[i] = item;
                }
                else
                {
                    bArray[i] = mArray[n];
                    n = n + 1;
                }
            }
            mArray = bArray;
            return;
        }

        public void RemoveAt(int index)
        {

            int n = 0;
            T[] bArray = new T[mArray.Length];
            
            for (int i = 0; i < mArray.Length; i++)
            {
                if (i!=index)
                {
                    bArray[n] = mArray[i];
                    n = n + 1;
                }
            }

            T[] cArray = new T[n];
            Array.Copy(bArray, cArray, n);
            mArray = cArray;
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
               mArray[index] = value;
            }
        }
    }
 
 
}
