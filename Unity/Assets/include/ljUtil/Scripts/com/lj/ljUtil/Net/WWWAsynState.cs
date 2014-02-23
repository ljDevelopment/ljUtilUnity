
using System.Collections;
using System.Net;
using System.Collections.Generic;

namespace com.lj.ljUtil {

	public class WWWAsynState {

		public WWWRequest WRequest { get ; private set; }
		public WWWCallback Callback { get ; private set; } 
		public WebRequest Request { get ; private set; }

		public WWWAsynState(WWWRequest wRequest, WWWCallback callback, WebRequest request) {
			WRequest = wRequest;
			Callback = callback;
			Request = request;
		}

		public override string ToString () {
			return ObjectSerializer.Serialize(
				new Dictionary<object, object>() {
				{
					this.GetType().ToString(), 
					new Dictionary<object, object>() {
						{"WRequest", WRequest},
						{"Callback", Callback},
						{"Request", WRequest},
					}
				}}
			);
		}
	}

}