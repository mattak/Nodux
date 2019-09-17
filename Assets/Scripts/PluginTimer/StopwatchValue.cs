using System;

namespace Nodux.PluginTimer
{
    [Serializable]
    public struct StopwatchValue
    {
        public bool IsPlaying;
        public float ElapsedTime;
    }
}