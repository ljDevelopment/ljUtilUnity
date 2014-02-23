using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace com.lj.ljUtil {

	[Serializable]
	public class DictionaryEditor {

		public List<string> keys;
		public List<string> values;

		public Dictionary<object, object> dictionary { get; private set; }

		private DictionaryEditor() {
			dictionary = new Dictionary<object, object>();
		}

		public string this[object key] {
			get {
				lock(dictionary) {
					if (!dictionary.ContainsKey(key)) {
						UpdateDictionaryNoLock();
					}
					return (dictionary.ContainsKey(key)) ? dictionary[key] as string: null; 
				}
			}
		}

		public DictionaryEditor UpdateDictionary() {
			lock(dictionary) {
				UpdateDictionaryNoLock();
			}
			return this;
		}

		private void UpdateDictionaryNoLock() {
			dictionary.Clear();

			using (List<string>.Enumerator enumeratorKeys = keys.GetEnumerator()) {
				using (List<string>.Enumerator enumeratorValues = values.GetEnumerator()) {
					while (enumeratorKeys.MoveNext()) {
						string key = enumeratorKeys.Current;
						
						if (!string.IsNullOrEmpty(key) && enumeratorValues.MoveNext()) {
							dictionary[key] = enumeratorValues.Current;
						}
					}
				}
			}
		}

	}
}
	