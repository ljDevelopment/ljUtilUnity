
using System.Collections;
using System;

namespace com.lj.ljUtil {

	public class TimeUtil {

		public static long NowUnixMilliseconds {
			get { return (long)(DateTime.Now.ToUniversalTime() - new DateTime (1970, 1, 1)).TotalMilliseconds; }
		}
	}

}
