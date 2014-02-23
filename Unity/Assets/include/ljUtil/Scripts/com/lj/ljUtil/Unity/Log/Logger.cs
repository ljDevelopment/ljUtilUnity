using UnityEngine;
using System.Collections;
using System;

namespace com.lj.ljUtil {

	public class Logger  {

		private enum Level {
			Log,
			Warning,
			Error,
			Exception
		}

		private static bool isDebug;
		static Logger() {
			try {
				isDebug = Debug.isDebugBuild;
			} catch {
				// DO NOTHING
			}
		}

		public static void Log(string pattern, params object[] parameters) {
			Logger.LogIfEnabled(pattern, Level.Log, false, parameters);
		}
		
		public static void LogForce(string pattern, params object[] parameters) {
			Logger.LogIfEnabled(pattern, Level.Log, true, parameters);
		}

		public static void LogWarning(string pattern, params object[] parameters) {
			Logger.LogIfEnabled(pattern, Level.Warning, false, parameters);
		}

		public static void LogWarningForce(string pattern, params object[] parameters) {
			Logger.LogIfEnabled(pattern, Level.Warning, true, parameters);
		}

		public static void LogError(string pattern, params object[] parameters) {
			Logger.LogIfEnabled(pattern, Level.Error, false, parameters);
		}
		
		public static void LogErrorForce(string pattern, params object[] parameters) {
			Logger.LogIfEnabled(pattern, Level.Error, true, parameters);
		}

		public static void LogException(Exception exception) {
			Logger.LogIfEnabled(null, Level.Exception, false, exception);
		}
		
		public static void LogEceptionForce(Exception exception) {
			Logger.LogIfEnabled(null, Level.Exception, true, exception);
		}


		private static void LogIfEnabled(string pattern, Level level, bool force, params object[] parameters) {

			bool logIt = isDebug || force;


			if (logIt) {
				try {
					if (level == Level.Exception) {
						object exception = (parameters.Length > 0) ? parameters[0] : null;
						if (exception != null && exception is Exception) {
							Debug.LogException(exception as Exception);
						} else {
							Debug.LogWarning(string.Format("Trying to log an exception, expected as the first param: params.length = {0}, object = {1}", parameters.Length, exception));
						}
					} else {
						string message = string.Format(pattern, parameters);
						
						switch(level) {
						case Level.Log:
							Debug.Log(message);
							break;
						case Level.Warning:
							Debug.LogWarning(message);
							break;
						case Level.Error:
							Debug.LogError(message);
							break;
						default:
							// DO NOTHING
							break;
						}
					}
				} catch(Exception e) {
					Debug.LogException(e);
				}
			}
		}
	}

}
