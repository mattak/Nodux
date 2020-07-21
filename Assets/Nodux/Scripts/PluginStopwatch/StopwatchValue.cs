using System;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    public struct StopwatchValue
    {
        public bool isPlaying;
        public float elapsedTime;
    }
}