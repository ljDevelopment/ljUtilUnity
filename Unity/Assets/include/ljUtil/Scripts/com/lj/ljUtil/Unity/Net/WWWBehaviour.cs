using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.lj.ljUtil {

	public class WWWBehaviour : MonoBehaviour, IWWW {

		private const string GAMEOBJECT_NAME_INSTANCE = "_GameObjectWWWBehaviourInstance";
		private const string GAMEOBJECT_NAME_DEFAULT = "GameObjectWWWBehaviourInstance";

		private static WWWBehaviour instance;
		public static WWWBehaviour Instance {
			get {
				if (instance == null) {
					GameObject container = GameObject.Find(GAMEOBJECT_NAME_INSTANCE);
					if (container == null) {
						instance = WWWBehaviour.NewInstance(GAMEOBJECT_NAME_INSTANCE);
					} else {
						instance = container.GetComponent<WWWBehaviour>();
					}
				}
				return instance;
			}
		}

		public static WWWBehaviour NewInstance(string containerName = GAMEOBJECT_NAME_DEFAULT) {
			GameObject gameObjectTemp = new GameObject(containerName, typeof(WWWBehaviour));
			WWWBehaviour instance = gameObjectTemp.GetComponent<WWWBehaviour>();
			return instance;
		}



		private const string CALLBACK_PARAM_CALLBACK = "callback";
		private const string CALLBACK_PARAM_REQUEST = "request";

		private static Action<TaskAction> wwwCallback = (TaskAction taskAction) => {
			WWWRequest request = taskAction.Parameters[CALLBACK_PARAM_REQUEST] as WWWRequest;
			WWWCallback callbackInAction = taskAction.Parameters[CALLBACK_PARAM_CALLBACK] as WWWCallback;
				
			if (!request.Response.Ok) {
				Logger.LogWarning("ERROR: {0}", request);
			}
			
			if (callbackInAction != null) {
				callbackInAction(request);
			}
		};

		public static void GetStatic(WWWRequest wwwConfig, WWWCallback callback) { Instance.Get(wwwConfig, callback); }
		public static void PostStatic(WWWRequest wwwConfig, WWWCallback callback) { Instance.Post(wwwConfig, callback); }


		public void Get(WWWRequest wwwRequest, WWWCallback callback) {

			bool isWebplayer = false;
#if UNITY_WEBPLAYER
			isWebplayer = true;
#endif
			if (wwwRequest.Init.Count > 0 && isWebplayer) {
				wwwRequest.Parameters.Add(WWWConsts.PARAMETER_KEY_METHOD, WWWConsts.PARAMETER_KEY_GET);
				Post(wwwRequest, callback);
			} else {

				if (!string.IsNullOrEmpty(wwwRequest.Proxy)) {
					wwwRequest.Parameters[WWWConsts.REQUEST_CONTENT_TYPE_PROXY] = wwwRequest.Init[WWWConsts.REQUEST_CONTENT_TYPE];
					wwwRequest.Init[WWWConsts.REQUEST_CONTENT_TYPE] = "application/x-www-form-urlencoded";
				}

				Hashtable headers = new Hashtable();
				{
					IDictionaryEnumerator enumerator = wwwRequest.Init.GetEnumerator();
					while (enumerator.MoveNext()) {
						object key = enumerator.Key;
						object value = enumerator.Value;
						if (key != null && value != null) {
							headers[key] = value;
						}
					}
				}

				WWW www = new WWW(wwwRequest.FullURL, null, headers);
				
				StartCoroutine(YieldWWW(www, new TaskAction(
					wwwCallback,
					new Dictionary<string, object>() {{CALLBACK_PARAM_CALLBACK, callback}, {CALLBACK_PARAM_REQUEST, wwwRequest}}
				)));
			}

		}



		public void Post(WWWRequest wwwRequest, WWWCallback callback) {

			if (wwwRequest.HasProxy) {
				wwwRequest.Parameters[WWWConsts.REQUEST_CONTENT_TYPE_PROXY] = wwwRequest.Init[WWWConsts.REQUEST_CONTENT_TYPE];
				wwwRequest.Init[WWWConsts.REQUEST_CONTENT_TYPE] = "application/x-www-form-urlencoded";
			}

			WWWForm form = new WWWForm();

			{
				IDictionaryEnumerator enumerator = wwwRequest.AllParameters.GetEnumerator();
				while (enumerator.MoveNext()) {

					string key = enumerator.Key as string;
					string value = enumerator.Value as string;

					if (null != key && null != value) {
						form.AddField(key, value);
					} else {
						Logger.LogWarning("Key or value... null: key={0}, value={1}", key, value);
					}
				}
			}
			Hashtable headers = form.headers;
			{
				IDictionaryEnumerator enumerator = wwwRequest.Init.GetEnumerator();

				while (enumerator.MoveNext()) {
					string key = (enumerator.Key is string) ? enumerator.Key as string : enumerator.Key.ToString();
					string value = (enumerator.Value != null) ? enumerator.Value.ToString() : null;
					
					if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)) {
						try {
							headers[key] = value;
						} catch(Exception e) {
							Logger.LogEceptionForce(new Exception(string.Format("key={0}, value={1}", key, value), e));
						}
					}
				}
			}
			WWW www = new WWW(wwwRequest.URL, form.data, headers);

			StartCoroutine(YieldWWW(www, new TaskAction(
				wwwCallback,
				new Dictionary<string, object>() {{CALLBACK_PARAM_CALLBACK, callback}, {CALLBACK_PARAM_REQUEST, wwwRequest}}
			)));
		}

		private IEnumerator YieldWWW(WWW www, TaskAction actionCallback) {
			if (string.IsNullOrEmpty(www.error)) {
				yield return www;
			}

			WWWRequest wwwRequest = actionCallback.Parameters[CALLBACK_PARAM_REQUEST] as WWWRequest;
			if (wwwRequest != null) {
				wwwRequest.Response.Error = www.error;
				if (wwwRequest.Response.Ok) {
					wwwRequest.Response.Text = www.text;
				}
			}

			Logger.Log("wwwRequest={0}", wwwRequest.ToString());

			actionCallback.DoWork();
		}
	}
}
