namespace TrackingService.Services
{
    public static class LiveAggregator
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, int> _counters = new Dictionary<string, int>();

        public static void IncrementCounter(string key)
        {
            lock (_lock)
            {
                if (!_counters.ContainsKey(key))
                    _counters[key] = 0;
                _counters[key]++;
            }
        }

        public static Dictionary<string, int> GetSnapshot()
        {
            lock (_lock)
            {
                // Ritorniamo una copia
                return new Dictionary<string, int>(_counters);
            }
        }

        // (Opzionale) se vuoi un reset
        public static void ResetAll()
        {
            lock (_lock)
            {
                _counters.Clear();
            }
        }
    }
}
