
using System.Collections;
using System;
using System.Collections.Generic;

namespace com.lj.ljUtil {

	public class TaskAction  {

		public Action<TaskAction> Callback { get; private set; }
		public IDictionary Parameters { get; private set; }

		public TaskAction(Action<TaskAction> callback, IDictionary parameters = null) {
			Callback = callback;

			if (parameters == null) {
				parameters = new Dictionary<object, object>();
			}
			Parameters = parameters;
		}

		public void DoWork() {
			Callback(this);
		}

	}

}
