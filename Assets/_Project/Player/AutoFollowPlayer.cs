using Cinemachine;
using UnityEngine;

namespace ProjectAwakening.Player
{
    public class AutoFollowPlayer : MonoBehaviour
    {
        private void Start()
        {
			GetComponent<CinemachineVirtualCamera>().Follow	= GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
