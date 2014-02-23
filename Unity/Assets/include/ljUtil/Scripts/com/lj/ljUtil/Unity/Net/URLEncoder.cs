using UnityEngine;
using System.Collections;


namespace com.lj.ljUtil {

	public class URLEncoder {

		public static string Encode(string url) {
			return WWW.EscapeURL(url);
		}

		public static string Decode(string url) {
			return WWW.UnEscapeURL(url);
		}
	}

}