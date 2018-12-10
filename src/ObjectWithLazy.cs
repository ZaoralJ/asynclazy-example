namespace AsyncLazy.Example
{
    using System.Threading.Tasks;

    public class ObjectWithLazy
    {
        private readonly AsyncLazy<int> _lazyNumber1;
        private readonly AsyncLazy<int> _lazyNumber2;

        /// <summary>
        /// startLazy is only for demo purpose ;-)
        /// </summary>
        public ObjectWithLazy(bool startLazy, AsyncLazyFlags asyncLazyFlags = AsyncLazyFlags.None)
        {
            _lazyNumber1 = new AsyncLazy<int>(async () =>
            {
                await Task.Delay(1500).ConfigureAwait(false);
                return 42;
            });

            _lazyNumber2 = new AsyncLazy<int>(async () =>
            {
                await Task.Delay(1500).ConfigureAwait(false);
                return 42;
            });

            if (startLazy)
            {
                _lazyNumber1.Start();
                _lazyNumber2.Start();
            }
        }

        public int Number1 => _lazyNumber1.Value;
        public int Number2 => _lazyNumber2.Value;
    }
}
