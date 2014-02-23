
using System.Collections;

namespace com.lj.ljUtil {

	public delegate void WWWCallback(WWWRequest wwwRequest);

	public interface IWWW  {

		void Get(WWWRequest wwwRequest, WWWCallback callback);
		void Post(WWWRequest wwwRequest, WWWCallback callback);

	}
}