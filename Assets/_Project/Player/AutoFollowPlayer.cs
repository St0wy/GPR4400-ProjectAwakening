using Cinemachine;
using UnityEngine;

namespace ProjectAwakening.Player
{
    public class AutoFollowPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
			GetComponent<CinemachineVirtualCamera>().Follow	= GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
