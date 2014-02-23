using UnityEngine;
using System.Collections;

namespace com.lj.ljUtil {

	public class StopWatchBehaviour : MonoBehaviour, IStopWatch {

		private StopWatchTic stopWatchTic;

		public float seconds;
		private float frequency;
		public float nextTic;
		private bool going;
		private bool previousGoing;

		public static StopWatchBehaviour NewInstance(string name = null) {
			GameObject gameObject = new GameObject((!string.IsNullOrEmpty(name)) ? name : "GameObjectStopWatch");

			gameObject.AddComponent<StopWatchBehaviour>();

			return gameObject.GetComponent<StopWatchBehaviour>();
		}

		public void Reset() {
			seconds = 0;
			going = false;
			previousGoing = false;
			nextTic = 0;

			CalculateNextTic();
		}

		public void Go() {
			going = true;
		}

		public void Stop() {
			going = false;
		}

		public float GetSeconds() {
			return seconds;
		}

		public void SetTicFrequency(float seconds) {
			frequency = seconds;
		}

		public void SetTicListener(StopWatchTic ticListener) {
			stopWatchTic += ticListener;
		}

		public void RemoveTicListener(StopWatchTic ticListener) {
			stopWatchTic -= ticListener;
		}

		private void Awake() {
			Reset();
			SetTicFrequency(0);
		}


		private void Update () {

			if (going && previousGoing) {
				seconds += Time.deltaTime;

				if (GetSeconds() >= nextTic) {
					CalculateNextTic();
					if (stopWatchTic != null) {
						stopWatchTic(this);
					}
				}
			}

			previousGoing = going;
		}

		private void CalculateNextTic() {
			if (frequency > 0) {
				while (nextTic <= GetSeconds()) {
					nextTic += frequency;
				}
			} else {
				nextTic = float.MaxValue;
			}
		}
	}
}	