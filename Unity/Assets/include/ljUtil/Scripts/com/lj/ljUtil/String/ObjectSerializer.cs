using UnityEngine;
using System.Collections;
using System.Text;

namespace com.lj.ljUtil {

	public class ObjectSerializer {

		public static string Serialize(object instance) {
			return SerializePrivate(instance).ToString();
		}

		private static StringBuilder SerializePrivate(object instance, StringBuilder builder = null) {
			if (builder == null) {
				builder = new StringBuilder();
			}

			if (instance == null) {
				builder.Append("null");
			} else if (instance is string) {
				SerializeString(instance as string, builder);
			} else if (instance is IDictionary) {
				SerializeDictionary(instance as IDictionary, builder); 
			} else if (instance is IEnumerable) {
				SerializeEnumerable(instance as IEnumerable, builder);
			} else {
				builder.AppendFormat("{0}", instance);
			}

			return builder;
		}

		private static void SerializeString(string instance, StringBuilder builder) {
			builder.AppendFormat("\"{0}\"", instance);
		}

		private static void SerializeEnumerable(IEnumerable instance, StringBuilder builder) {
			builder.Append("[");

			IEnumerator enumerator = instance.GetEnumerator();
			if (enumerator.MoveNext()) {
				do {
					SerializePrivate(enumerator.Current, builder);
					if (!enumerator.MoveNext()) {
						break;
					}
					builder.Append(", ");
				} while(true);
			}

			builder.Append("]");
		}

		private static void SerializeDictionary(IDictionary instance, StringBuilder builder) {
			builder.Append("[");
			
			IDictionaryEnumerator enumerator = instance.GetEnumerator();
			if (enumerator.MoveNext()) {
				do {
					SerializePrivate(enumerator.Key, builder);
					builder.Append(" : ");
					SerializePrivate(enumerator.Value, builder);
					if (!enumerator.MoveNext()) {
						break;
					}
					builder.Append(", ");
				} while(true);
			}
			
			builder.Append("]");
		}
	}
}
