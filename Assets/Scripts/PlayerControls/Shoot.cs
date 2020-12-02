using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.EnemyControls;

namespace Scripts.PlayerControls
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _tracerRounds;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private ParticleSystem _reloadSteam;
        [SerializeField] private GameObject _bloodSplat;
        [SerializeField] private Animator _anim;
        [SerializeField] private int _magazineSize = 40;
        private int _damageAmount = 5;
        private int _ammo = 160;
        private int _magazine;
        private bool _reloading;
        private WaitForSeconds _reloadYield;
        

        void Start()
        {
            _magazine = _magazineSize;
            _reloadYield = new WaitForSeconds(1f);
        }

        void Update()
        {
            if (_reloading == false)
            {
                //cast ray from center of the screen through the radicule 
                Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hitInfo; //Store information about the object we hit

                //cast a ray
                if (Input.GetMouseButtonDown(0))
                {
                    _muzzleFlash.Play();
                    _tracerRounds.Play();

                    if (Physics.Raycast(rayOrigin, out hitInfo))
                    {
                        Debug.Log(hitInfo);
                        EnemyAI enemy = hitInfo.collider.GetComponent<EnemyAI>();

                        if (enemy != null)
                        {
                            GameObject blood = Instantiate(_bloodSplat, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                            Destroy(blood, 5.0f);
                            EventManager.Fire("onDamageEnemy", _damageAmount, enemy.gameObject);
                        }
                    }

                    Magazine();
                }
                else
                {
                    _muzzleFlash.Stop();
                    _tracerRounds.Stop();
                }
            }
        }

        private void Magazine()
        {
            _magazine--;
            if (_magazine > 0)
            {
                EventManager.Fire("onFiring", _magazine);
            }
            else
            {
                EventManager.Fire("onFiring", 0);
                if (_ammo > 0)
                {
                    _reloading = true;
                    StartCoroutine(Reload());
                }
            }
        }

        private IEnumerator Reload()
        {
            _muzzleFlash.Stop();
            _tracerRounds.Stop();

            if ((_ammo - _magazineSize) >= 0)
            {
                // Play reload animation
                Debug.Log("Reloading");
                _anim.SetTrigger("Reload");
                _reloadSteam.Play();
                yield return _reloadYield;
                _reloadSteam.Stop();
                _magazine = _magazineSize;
                _ammo -= _magazineSize;
                Debug.Log(_ammo);
                EventManager.Fire("onReload", _ammo);
                EventManager.Fire("onFiring", _magazine);
                _reloading = false;
            }
            else if (_ammo > 0 && (_ammo < _magazineSize))
            {
                // Play reload animation
                _anim.SetTrigger("Reload");
                _reloadSteam.Play();
                yield return _reloadYield;
                _reloadSteam.Stop();
                _magazine = (_magazineSize - _ammo);
                _ammo = 0;
                EventManager.Fire("onReload", _ammo);
                EventManager.Fire("onFiring", _magazine);
                _reloading = false;
            }
            else if (_ammo == 0)
            {
                _magazine = 0;
                Debug.Log("Out of ammo!");
            }
        }
    }
}

