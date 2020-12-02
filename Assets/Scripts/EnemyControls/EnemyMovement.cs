using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.EnemyControls
{
    public class EnemyMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _anim;

        [SerializeField] private int _waitTime;
        private float _timer;
        private float _radius;
        private bool _isDead;
        private bool _chase;


        private void OnEnable()
        {
            EventManager.Listen("onChase", Chase);
            EventManager.Listen("onCeaseChase", Chase);
            EventManager.Listen("onDeath", Death);
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isDead == true || _chase == true)
                return;

            _timer += Time.deltaTime;
            if (_timer >= _waitTime)
            {
                Vector3 newPos = GetRandomPosition(transform.position, _radius, -1);
                _agent.SetDestination(newPos);
                _anim.SetBool("Walk", true);

                if (_agent.remainingDistance < 1f)
                {
                    _anim.SetBool("Walk", false);
                }

                _timer = 0f;
            }
        }

        private Vector3 GetRandomPosition(Vector3 origin, float distance, int layerMask)
        {
            Vector3 randomPos = Random.insideUnitSphere * distance;

            randomPos += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomPos, out navHit, distance, layerMask);
            return navHit.position;
        }

        private void Death()
        {
            _isDead = true;
        }

        private void Chase()
        {
            _chase = !_chase;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onChase", Chase);
            EventManager.UnsubscribeEvent("onCeaseChase", Chase);
            EventManager.UnsubscribeEvent("onDeath", Death);
        }
    }
}

