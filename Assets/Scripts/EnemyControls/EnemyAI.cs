using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.EnemyControls
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        [SerializeField] private int _damage;
        [SerializeField] private GameObject _player;
        private Animator _anim;
        private NavMeshAgent _agent;
        private bool _isDead;
        private bool _attacking;
        private bool _chase;

        private WaitForSeconds _roarYield;
        private WaitForSeconds _flexYield;
        private WaitForSeconds _attackYield;

        private void OnEnable()
        {
            EventManager.Listen("onDamageEnemy", (Action<int, GameObject>)Health);
            EventManager.Listen("onChase", ChasePlayer);
            EventManager.Listen("onCeaseChase", StopChase);
        }

        void Start()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();

            _roarYield = new WaitForSeconds(5f);
            _flexYield = new WaitForSeconds(2f);
            _attackYield = new WaitForSeconds(5f);

            StartCoroutine(RoarRoutine());
            StartCoroutine(FlexRoutine());
        }

        private void Update()
        {
            if (_chase == false)
                return;

            _agent.SetDestination(_player.transform.position);

            if (_agent.remainingDistance < 1f)
            {
                _chase = false;
                _attacking = true;
                StartCoroutine(AttackRoutine());
            }
        }

        private void Health(int damage, GameObject enemy)
        {
            if (enemy.Equals(this.gameObject))
            {
                _health -= damage;
                if (_health <= 0)
                {
                    _health = 0;
                    _isDead = true;
                    EventManager.Fire("onDeath");
                    StartCoroutine(DeathRoutine());
                }
            }
        }

        private void ChasePlayer()
        {
            _anim.SetBool("Run", true);
            _chase = true;
        }

        private void StopChase()
        {
            _anim.SetBool("Run", false);
            StartCoroutine(RoarRoutine());
            StartCoroutine(FlexRoutine());
        }

        private void AttackDistanceCheck()
        {
            float distance = (transform.position - _player.transform.position).magnitude;
            if (distance > 1f)
            {
                _attacking = false;
                StopCoroutine(AttackRoutine());
                ChasePlayer();
            }
        }

        IEnumerator RoarRoutine()
        {
            while (_isDead == false)
            {
                yield return _roarYield;
                if (_attacking == false && _isDead == false)
                {
                    _anim.SetTrigger("Roar");
                }
            }
        }

        IEnumerator FlexRoutine()
        {
            while (_isDead == false)
            {
                yield return _flexYield;
                if (_attacking == false && _isDead == false)
                {
                    _anim.SetTrigger("Flex");
                }
            }
        }

        IEnumerator AttackRoutine()
        {
            while (_attacking == true)
            {
                _anim.SetTrigger("Attack");
                StopCoroutine(RoarRoutine());
                StopCoroutine(FlexRoutine());
                yield return _attackYield;
                AttackDistanceCheck();
            }
        }

        IEnumerator DeathRoutine()
        {
            _agent.isStopped = true;
            _anim.SetTrigger("Death");
            yield return new WaitForSeconds(10f);
            _anim.enabled = false;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDamageEnemy", (Action<int, GameObject>)Health);
            EventManager.UnsubscribeEvent("onChase", ChasePlayer);
            EventManager.UnsubscribeEvent("onCeaseChase", StopChase);
        }
    }
}

