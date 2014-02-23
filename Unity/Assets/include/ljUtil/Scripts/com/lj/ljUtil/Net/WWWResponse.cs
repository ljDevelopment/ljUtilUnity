using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.lj.ljUtil {

	public class WWWResponse  {

		public bool Ok { get { return string.IsNullOrEmpty(Error); } }
		public string Text { get; set; }
		public string Error { get; set; }

		public WWWResponse(string text = null) : this(text, null) {
		}

		protected WWWResponse(string text, string error = null) {
			Text = text;
			Error = error;
		}

		public override string ToString ()
		{
			return ObjectSerializer.Serialize(
				new Dictionary<object, object>() {
				{
					this.GetType().ToString(), 
					new Dictionary<object, object>() {
						{"Ok", Ok},
						{"Text", Text},
						{"Error", Error},
					}
				}}
			);
		}
	}

}