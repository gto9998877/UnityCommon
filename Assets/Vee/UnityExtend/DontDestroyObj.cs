
using UnityEngine;

namespace Vee.UnityExtend
{
	public class DontDestroyObj : MonoBehaviour {

		void Awake() {
			DontDestroyOnLoad(transform.gameObject);
		}
	}
}
