using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JacobHomanics.UI;

namespace JacobHomanics.UI
{
    public class DefaultCurrentMaxConnector : BaseCurrentMaxConnector
    {
        [SerializeField] private float currentNum;
        [SerializeField] private float maxNum;

        public override float CurrentNum => currentNum;
        public override float MaxNum => maxNum;
    }
}
