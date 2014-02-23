
using System.Collections;
using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace com.lj.ljUtil {

	public class WWWImpl : IWWW {

		public static void WWWCallbackEmpty(WWWResponse wwwResponse) {}

		public WWWImpl() {
			ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
		}

		public void Get(WWWRequest wwwRequest, WWWCallback callback) {

			WebRequest request = WebRequest.Create(wwwRequest.FullURL);
			request.Method = "GET";

			WWWAsynState state = new WWWAsynState(wwwRequest, callback, request);
			StartRequest(state);
		}

		public void Post(WWWRequest wwwRequest, WWWCallback callback) {

			WebRequest request = WebRequest.Create(wwwRequest.URL);
			request.Method = "POST";

			WWWAsynState state = new WWWAsynState(wwwRequest, callback, request);
			StartRequest(state);
		}

		private void StartRequest(WWWAsynState state) {

			if (!string.IsNullOrEmpty(state.WRequest.Proxy)) {
				state.WRequest.Parameters[WWWConsts.REQUEST_CONTENT_TYPE_PROXY] = state.WRequest.Init[WWWConsts.REQUEST_CONTENT_TYPE];
				state.WRequest.Init[WWWConsts.REQUEST_CONTENT_TYPE] = "application/x-www-form-urlencoded";
			}

			IDictionaryEnumerator enumerator = state.WRequest.Init.GetEnumerator();
			while (enumerator.MoveNext()) {
				string key = (enumerator.Key is string) ? enumerator.Key as string : enumerator.Key.ToString();
				string value = (enumerator.Value != null) ? enumerator.Value.ToString() : null;

				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)) {
					if (WWWConsts.REQUEST_CONTENT_TYPE.Equals(key)) {
						if (value != null) { state.Request.ContentType = value;}
					} else {
						try {
							state.Request.Headers.Add(key, value);
						} catch(Exception e) {
							Logger.LogEceptionForce(new Exception(string.Format("key={0}, value={1}", key, value), e));
						}
					}
				}
			}
			
			if ("POST".Equals(state.Request.Method)) {
				state.Request.BeginGetRequestStream(new AsyncCallback(RequestStreamReady), state); 
			} else {
				state.Request.BeginGetResponse(new AsyncCallback(GetResponseCallback), state);
			}
		}

		private void RequestStreamReady(IAsyncResult ar) {
						
			WWWAsynState state = ar.AsyncState as WWWAsynState;

			string parametersJoined = state.WRequest.ParametersJoined;
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(parametersJoined);
			Stream postStream = state.Request.EndGetRequestStream(ar);
			postStream.Write(bytes, 0, bytes.Length);
			postStream.Close();

			state.Request.BeginGetResponse(new AsyncCallback(GetResponseCallback), state);
		}

		private void GetResponseCallback(IAsyncResult ar) {		

			WWWAsynState state = ar.AsyncState as WWWAsynState;

			bool responseOk = true;
			HttpWebResponse response = null;
			try {
				response = (HttpWebResponse)state.Request.EndGetResponse(ar);
			} catch (WebException e){
				responseOk = false;
				response = (HttpWebResponse)e.Response;
			}

			string text = null;
			string error = null;

			try {		
				StreamReader streamReader = new StreamReader(response.GetResponseStream());
				text = streamReader.ReadToEnd();
				streamReader.Close();					
			} catch (Exception ex) {
				text = ex.ToString();
			}
			
			
			if (!responseOk) {
				error = string.Format("Error {0}: {1} ({2})", response.StatusCode, response.StatusDescription, state.Request.RequestUri);
			} 

			if (state.Callback != null) {

				state.WRequest.Response.Text = text;
				state.WRequest.Response.Error = error;

				state.Callback(state.WRequest);
			}

			Logger.Log("state={0}", state.ToString());

		}
		
		public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) {
			return true;
		}
	}

}
