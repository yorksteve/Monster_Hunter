using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.EnemyControls
{
    public class EnemyDetection : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EventManager.Fire("onChase");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EventManager.Fire("onCeaseChase");
            }
        }
    }
}

