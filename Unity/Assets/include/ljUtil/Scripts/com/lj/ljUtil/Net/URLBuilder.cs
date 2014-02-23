
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace com.lj.ljUtil {

	public class URLBuilder {

		private const string URL_PARAMETERS_STARTER = "?";
		private const string URL_PARAMETERS_JOINER = "&";

		public static string BuildURL(string url, IDictionary parameters = null, bool urlEncode = true) {

			StringBuilder stringBuilder = new StringBuilder(url);

			if (parameters != null) {

				StringBuilder parametersBuilder = BuildURLParametersPrivate(parameters);
				if (parametersBuilder.Length > 0) {
					stringBuilder.Append(URL_PARAMETERS_STARTER);
					stringBuilder.Append(parametersBuilder);
				}
			}

			return stringBuilder.ToString();
		}

		public static string BuildURLParameters(IDictionary parameters = null, bool urlEncode = true) {
			return BuildURLParametersPrivate(parameters).ToString();
		}

		private static StringBuilder BuildURLParametersPrivate(IDictionary parameters = null, bool urlEncode = true, StringBuilder stringBuilder = null) {
			if (stringBuilder == null) {
				stringBuilder = new StringBuilder();
			}

			if (parameters != null) {
				IDictionaryEnumerator enumerator = parameters.GetEnumerator();
				if (enumerator.MoveNext()) {
					do {
						string key = (enumerator.Key != null) ? enumerator.Key as string : null;
						string value = (enumerator.Value != null) ? enumerator.Value as string : null;

						if (!string.IsNullOrEmpty(key) && value != null) {
							if (urlEncode) { value = URLEncoder.Encode(value); }
							stringBuilder.AppendFormat("{0}={1}", key, value);
						}
						if (!enumerator.MoveNext()) {
							break;
						}
						stringBuilder.Append(URL_PARAMETERS_JOINER);
					} while(true);
				}
				
			}		

			return stringBuilder;
		}
	}
}