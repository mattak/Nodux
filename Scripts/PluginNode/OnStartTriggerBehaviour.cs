using System;
using UniRx;
using UnityEngine;

namespace Nodux.PluginNode
{
    public class OnStartTriggerBehaviour : MonoBehaviour
    {
        public IObservable<Unit> OnStartObservable => this.Subject;
        private ISubject<Unit> Subject = new ReplaySubject<Unit>();

        private void Start()
        {
            this.Subject.OnNext(Unit.Default);
            this.Subject.OnCompleted();
        }
    }
}