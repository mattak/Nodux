using System;
using UniRx;
using UnityEngine;

namespace UnityLeaf.PluginLeaf
{
    public class OnStartTriggerBehaviour : MonoBehaviour
    {
        public IObservable<Unit> OnStartObservable => this.Subject;
        private ISubject<Unit> Subject = new Subject<Unit>();

        private void Start()
        {
            this.Subject.OnNext(Unit.Default);
        }
    }
}