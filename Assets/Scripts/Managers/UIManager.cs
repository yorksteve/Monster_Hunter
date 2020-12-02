using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private Text _ammoCount;
        [SerializeField] private Text _magazineCount;


        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            EventManager.Listen("onFiring", (Action<int>)MagazineDisplay);
            EventManager.Listen("onReload", (Action<int>)AmmoDisplay);
        }

        public void MagazineDisplay(int amount)
        {
            _magazineCount.text = amount.ToString();
        }

        public void AmmoDisplay(int amount)
        {
            _ammoCount.text = amount.ToString();
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onFiring", (Action<int>)MagazineDisplay);
            EventManager.UnsubscribeEvent("onReload", (Action<int>)AmmoDisplay);
        }
    }
}

