

namespace com.lj.ljUtil {

	public delegate void StopWatchTic(IStopWatch stopWatch);

	public interface IStopWatch {

		void Reset();
		void Go();
		void Stop();
		float GetSeconds();
		void SetTicFrequency(float seconds);
		void SetTicListener(StopWatchTic ticListener);
		void RemoveTicListener(StopWatchTic ticListener);

	}

}