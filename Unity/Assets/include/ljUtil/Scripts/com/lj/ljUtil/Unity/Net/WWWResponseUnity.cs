
using System.Collections;
using UnityEngine;

namespace com.lj.ljUtil {

	public class WWWResponseUnity : WWWResponse {

		public WWWResponseUnity(WWW www) {
			Error = www.error;
			if (Ok) {
				Text = www.text;
			}
		}

	}
}
