
using System.Collections;
using System.Collections.Generic;

namespace com.lj.ljUtil {

	public class WWWRequest  {

		public IDictionary Parameters { get; private set; }
		public IDictionary Init { get; private set; }

		public WWWResponse Response { get; private set; }

		public bool HasProxy { get { return !string.IsNullOrEmpty(Proxy); }  }
		public string Proxy { get; set; }
		public string URL { 
			get {
				return (HasProxy) ? Proxy : url;
			} 
			set {
				url = value;
			}
		}
		public string FullURL { get { return URLBuilder.BuildURL(URL, AllParameters); } }
		public string ParametersJoined { get { return URLBuilder.BuildURLParameters(AllParameters, false); } }
		public string ParametersJoinedEncoded { get { return URLBuilder.BuildURLParameters(AllParameters); } }

		public IDictionary AllParameters {
			get {
				IDictionary allParameters = null;

				if (HasProxy) {
					allParameters = new Dictionary<object, object>();
					{
						IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
						while(enumerator.MoveNext()) {
							allParameters[enumerator.Key] = enumerator.Value;
						}
					}
					allParameters["url"] = url;
				} else {
					allParameters = Parameters;
				}

				return allParameters;
			}
		}

		private string url;

		public WWWRequest(string url, IDictionary parameters, IDictionary init) {
			URL = url;
			Parameters = (parameters != null) ? parameters : new Dictionary<object, object>();
			Init = (init != null) ? init : new Dictionary<object, object>();
			Response = new WWWResponse();
		}


		public override string ToString ()
		{
			return ObjectSerializer.Serialize(
				new Dictionary<object, object>() {
				{
					this.GetType().ToString(), 
					new Dictionary<object, object>() {
						{"URL", URL},
						{"Parameters", Parameters},
						{"Init", Init},
						{"Response", Response},
					}
				}}
			);
		}
	}
}