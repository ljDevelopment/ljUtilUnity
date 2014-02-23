using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.lj.ljUtil {

	public delegate void GUITextComponentOnEvent(TextComponentEvent textEvent, KeyCode keyCode, GUITextComponent component);

	public enum TextComponentEvent {
		Pressed,
		Released
	};
	
	public enum TextComponentType {
		Field,
		Area,
		Label,
		Box
	};

	public class GUITextComponent : MonoBehaviour {



		public string Text = "";
		public TextComponentType Type = TextComponentType.Field;
		public GUILayoutOption[] Options;

		private Dictionary<KeyCode, HashSet<GUITextComponentOnEvent>> listeners;
		private Rect rectangle;

		public static GUITextComponent NewInstance(string name = null) {

			if (string.IsNullOrEmpty(name)) { name = "GUITextComponentContainer"; }

			GameObject gameObject = new GameObject(name);
			gameObject.AddComponent<GUITextComponent>();

			return gameObject.GetComponent<GUITextComponent>();
		}

		public void Configure(params GUILayoutOption[] options) {
			if (options != null) {
				Options = options;
			} else {
				options = new GUILayoutOption[]{};
			}
		}

		public void RegisterEventListener(KeyCode keyCode, GUITextComponentOnEvent listener) {
			lock(listeners) {
				if (!listeners.ContainsKey(keyCode)) {
					listeners[keyCode] = new HashSet<GUITextComponentOnEvent>();
				}
				HashSet<GUITextComponentOnEvent> keyListeners = listeners[keyCode];

				keyListeners.Add(listener);
			}
		}

		public bool UnregisterEventListener(KeyCode keyCode, GUITextComponentOnEvent listener) {
			bool removed = false;

			lock(listeners) {
				if (listeners.ContainsKey(keyCode)) {
					removed = listeners[keyCode].Remove(listener);
					if (listeners[keyCode].Count == 0) {
						listeners.Remove(keyCode);
					}
				}
			}

			return removed;
		}

		public void OnGUI() {
			if (Options == null || Options.Length == 0) {
				Text = DoGUI(Text, Options);
			}
		}

		public void DoGUILayout() {
			switch(Type) {
			case TextComponentType.Field:
				Text = GUILayout.TextField(Text, Options);
				break;
			case TextComponentType.Area:
				Text = GUILayout.TextArea(Text, Options);
				break;
			case TextComponentType.Label:
				GUILayout.Label(Text, Options);
				break;
			case TextComponentType.Box:
				GUILayout.Box(Text, Options);
				break;
			}
		}

		private void Awake() {
			listeners = new Dictionary<KeyCode, HashSet<GUITextComponentOnEvent>>();
			rectangle = new Rect();
		}

		private void Update() {
			lock(listeners) {
				if (listeners.Count > 0) {
					foreach(KeyValuePair<KeyCode, HashSet<GUITextComponentOnEvent>> keyKeyListener in listeners) {
						TextComponentEvent? keyEvent = null;

						KeyCode keyCode = keyKeyListener.Key;
						if (Input.GetKeyDown(keyCode)) {
							keyEvent = TextComponentEvent.Pressed;
						} else if (Input.GetKeyUp(keyCode)) {
							keyEvent = TextComponentEvent.Released;
						}

						if (keyEvent.HasValue) {
							foreach(GUITextComponentOnEvent listener in keyKeyListener.Value) {
								listener(keyEvent.Value, keyCode, this);
							}
						}
					}

				}
			}
		}

		private string DoGUI(string text, params GUILayoutOption[] options) {
			rectangle.x = transform.localPosition.x; 
			rectangle.y = transform.localPosition.y;
			rectangle.width = transform.localScale.x;
			rectangle.height = transform.localScale.y;

			switch(Type) {
			case TextComponentType.Field:
				text = GUI.TextField(rectangle, text);
				break;
			case TextComponentType.Area:
				text = GUI.TextArea(rectangle, text);
				break;
			case TextComponentType.Label:
				GUI.Label(rectangle, text);
				break;
			case TextComponentType.Box:
				GUI.Box(rectangle, text);
				break;
			}

			return text;
		}

	}

}