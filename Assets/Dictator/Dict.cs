using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

	[Serializable]
	public class Dict{

	}
	
	
    /// <summary>
    /// Subclass this class to start using Dictionary
    /// Abstract means you cannot create instance of this class, but your subclass does not have to be abstract
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
	public abstract class Dict<TKey, TValue> : Dict, 
        ISerializationCallbackReceiver, 
        IDictionary<TKey, TValue>, 
        IEnumerator<KeyValuePair<TKey,TValue>>
	{
        
		/// <summary>
        /// The Actual Dictionary object instance
        /// you don't have to access this field to use your dictionary
        /// </summary>
        public Dictionary<TKey,TValue> DictInstance = new Dictionary<TKey, TValue>();
		

        
		/// <summary>
        /// Fields to enable serialization in Unity
        /// Basically, Dictator will copy values from DictInstance to 2 list
        /// Unity then use those 2 fields to serialize the Dictionary
        /// 
        /// When deserializing, Dictator copies value from those 2 lists back to the DictInstance
        /// except for indexes found in _errorList
        /// </summary>
        [SerializeField]
		List<TKey> _key = new List<TKey>();
		[SerializeField]
		List<TValue> _val = new List<TValue>();
		[SerializeField]
		List<int> _errorList = new List<int>();
		
		
        
		/// <summary>
        /// Field for telling the inspector what kind of type the key values are
        /// intentionally unused
        /// </summary>
        [SerializeField]
		TKey _keyType;
		[SerializeField]
		TValue _valType;

        //Divider is used to divide key value display into 2 parts
        [SerializeField]
        float _divider = 0;

        [SerializeField]
        int _selected;

        [SerializeField] bool reset;

    /// <summary>
    /// before uniting serialize this instance, dict values are copied to 2 lists which will then be serialized
    /// </summary>
    public void OnBeforeSerialize()
		{


			if(_errorList.Count == 0)
			{
				//break dict into 2 list, then serialize 2 list
				_key.Clear();
				_val.Clear();
				foreach (var kvp in DictInstance)
				{
					_key.Add(kvp.Key);
					_val.Add(kvp.Value);
				}
			}
        }

        
        /// <summary>
        /// After deserializiation is done, copy the value from 2 lists back to the DictInstance
        /// </summary>
        public void OnAfterDeserialize()
        {
            if (reset || _key.Count != _val.Count)
            {
                _key.Clear();
            _val.Clear();
            _errorList.Clear();
                reset = false;
            }
            DictInstance.Clear();

        int count = Math.Min(_key.Count, _val.Count);

            //first pass, looking for duplicates
            _errorList.Clear();
            var duplicateItems = from x in _key
				                group x by x into grouped
				                where grouped.Count() > 1
				                select grouped.Key;
            var list = duplicateItems.ToList();

            foreach (var k in list)
            {
                var res = Enumerable.Range(0, _key.Count)
                                     .Where(i => EqualityComparer<TKey>.Default.Equals(_key[i],k))
                                     .ToList();
                _errorList.AddRange(res);
            }
			
			//second pass, look for null keys
			for (int i = 0; i < count; i++) {
				if(_key[i] == null)
				{
                        _errorList.Add(i);
				}

            }

            // no errors, populate our dict;
				for (var i = 0; i < count; i++)
				{
					if(_key[i] == null)
					{
						_errorList.Add(i);
					}
					else{
						if(!_errorList.Contains(i))
						DictInstance.Add(_key[i], _val[i]);
						
					}
				}
            //suppresses unused warning >_<
            _divider += 0;
		}

        #region DictionaryAPI
        public void Add(TKey k, TValue v){DictInstance.Add(k,v);}
		public IEnumerator GetEnumerator(){return DictInstance.GetEnumerator();}

        public IEqualityComparer<TKey> Comparer { get { return DictInstance.Comparer; } }
        public int Count { get { return DictInstance.Count; } }

        public TValue this[TKey key]
        {
            get { return DictInstance[key]; }
            set { DictInstance[key] = value; }
        }
        public System.Collections.Generic.Dictionary<TKey,TValue>.KeyCollection Keys { get { return DictInstance.Keys; } }
        public System.Collections.Generic.Dictionary<TKey,TValue>.ValueCollection Values { get { return DictInstance.Values; } }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return DictInstance.Keys;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return DictInstance.Values;

            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                throw new NotSupportedException();//yet
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotSupportedException();//yet
            }
        }

        public void Clear() { DictInstance.Clear();}
        public bool ContainsKey(TKey key){return DictInstance.ContainsKey(key);}
        public bool ContainsValue(TValue value) {return DictInstance.ContainsValue(value);}
        public new bool Equals(object obj) {return DictInstance.Equals(obj);}

        public virtual void OnDeserialization(object sender)
        {
            DictInstance.OnDeserialization(sender);
        }
        public bool Remove(TKey key) { return DictInstance.Remove(key);}

        public override string ToString()
        {
            return DictInstance.ToString();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return DictInstance.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            DictInstance.Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var entry = DictInstance[item.Key];
            return entry != null && EqualityComparer<TValue>.Default.Equals(entry, item.Value);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return DictInstance.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();//yet
        }

        public bool MoveNext()
        {
            throw new NotSupportedException();//yet
    }

        public void Reset()
        {
            throw new NotSupportedException();//yet
    }

        public void Dispose()
        {
            throw new NotSupportedException();//yet
    }
        #endregion
    }
	