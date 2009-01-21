using System;
using System.Collections;
using System.Collections.Generic;

namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
    /// <summary>
    /// MemoryPacketParameterCollection¿¡ ´ëÇÑ ¿ä¾à ¼³¸íÀÔ´Ï´Ù.
    /// </summary>
    public class MemoryPacketParameterCollection : ICollection, IList, ICloneable
    {
        private List<MemoryPacketParameter> parameters = new List<MemoryPacketParameter>();

        public MemoryPacketParameterCollection()
        {
        }

        public MemoryPacketParameterCollection(MemoryPacketParameterCollection collection)
            : this()
        {
            foreach (MemoryPacketParameter parm in collection)
            {
                MemoryPacketParameter newParm = new MemoryPacketParameter(parm);
                parameters.Add(newParm);
            }
        }

        public static MemoryPacketParameterCollection NewInstance(MemoryPacketParameterCollection collection)
        {
            return new MemoryPacketParameterCollection(collection);
        }

        #region ICollection ¸â¹ö

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)parameters).IsSynchronized;
            }

        }

        public int Count
        {
            get
            {
                return parameters.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)parameters).CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return ((ICollection)parameters).SyncRoot;
            }
        }

        #endregion

        #region IEnumerable ¸â¹ö

        public IEnumerator GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        #endregion

        #region IList ¸â¹ö

        public bool IsReadOnly
        {
            get
            {
                return ((IList)parameters).IsReadOnly;
            }
        }

        public object this[int index]
        {
            get
            {
                return parameters[index];
            }
            set
            {
                parameters[index] = (MemoryPacketParameter)value;
            }
        }

        public void RemoveAt(int index)
        {
            parameters.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            try { IsValidMemoryPacketParameter(value); }
            catch (Exception ex) { throw ex; };

            ((IList)parameters).Insert(index, value);
        }

        public void Remove(object value)
        {
            ((IList)parameters).Remove(value);
        }

        public bool Contains(object value)
        {
            return ((IList)parameters).Contains(value);
        }

        public void Clear()
        {
            parameters.Clear();
        }

        public int IndexOf(object value)
        {
            return ((IList)parameters).IndexOf(value);
        }

        public int Add(object value)
        {
            try { IsValidMemoryPacketParameter(value); }
            catch (Exception ex) { throw ex; };

            return ((IList)parameters).Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return ((IList)parameters).IsFixedSize;
            }
        }

        #endregion

        #region ICloneable ¸â¹ö

        public object Clone()
        {
            MemoryPacketParameterCollection collection = new MemoryPacketParameterCollection();

            foreach (MemoryPacketParameter parm in parameters)
            {
                MemoryPacketParameter newParm = new MemoryPacketParameter(parm);
                collection.Add(newParm);
            }

            return collection;
        }

        #endregion

        private void IsValidMemoryPacketParameter(object value)
        {
            IsMemoryPacketParameter(value);

            IsValidParameterName(value);
        }

        private void IsValidParameterName(object value)
        {
            if (Contains(value))
            {
                MemoryPacketParameter parm = (MemoryPacketParameter)value;
                string message = String.Format("ParameterName {0} is already in this collection.", parm.ParameterName);

                throw new ArgumentException(message);
            }
        }


        private void IsMemoryPacketParameter(object value)
        {
            if (value == null)
            {
                string message = GetInvalidParameterExceptionMessage(typeof(MemoryPacketParameter).ToString(), "null");
                throw new ArgumentNullException(message);
            }
            else
            {
                if (value.GetType() != typeof(MemoryPacketParameter))
                {
                    string message = String.Format(GetInvalidParameterExceptionMessage(typeof(MemoryPacketParameter).ToString(), value.GetType().ToString()));
                    throw new InvalidCastException(message);
                }
            }

            return;
        }

        private string GetInvalidParameterExceptionMessage(string mustBe, string current)
        {
            return String.Format("The type of a parameter must be '{0}', not '{1}'.", mustBe, current);
        }


        public int Add(MemoryPacketParameter parameter)
        {
            return Add((object)parameter);
        }


        public int GetByteCount()
        {
            int byteCount = 0;

            foreach (MemoryPacketParameter parm in parameters)
            {
                if (parm.FixedSize == true)
                {
                    byteCount = byteCount + parm.Size;
                }
                else
                {
                    byteCount = -1;
                    break;
                }
            }

            return byteCount;
        }

        public object this[string parameterName]
        {
            get
            {
                foreach (MemoryPacketParameter parm in parameters)
                {
                    if (parm.ParameterName.Equals(parameterName) == true)
                    {
                        return parm;
                    }
                }

                return null;
            }
        }
    }
}
