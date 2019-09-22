using System;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    public struct StopwatchValue
    {
        public bool IsPlaying;
        public float ElapsedTime;
    }
}