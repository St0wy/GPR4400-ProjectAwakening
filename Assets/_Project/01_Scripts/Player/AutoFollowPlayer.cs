﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace ProjectAwakening
{
    public class AutoFollowPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
			GetComponent<CinemachineVirtualCamera>().Follow	= GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
