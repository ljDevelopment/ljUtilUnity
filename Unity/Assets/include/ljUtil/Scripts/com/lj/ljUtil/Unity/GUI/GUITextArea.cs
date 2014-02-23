using UnityEngine;
using System.Collections;

namespace com.lj.ljUtil {

	[ExecuteInEditMode]
	public class GUITextArea : MonoBehaviour {

		public string Text;

		private Rect rect;

		private void Awake() {
			rect = new Rect();
		}

		private void OnGUI() {

			float width = transform.localScale.x;
			float height = transform.localScale.y;

			rect.Set(transform.localPosition.x - width/2, 
				transform.localPosition.y - height/2,
				width,
			    height);

			Text = GUI.TextArea(rect, Text);
		}
	}	

}