namespace ArchyTECH.Core.Caching
{
    public static class Application
    {
        static Application()
        {
            Cache = new Cache("applicationCache");
        }

        public static ICache Cache { get; set; }
    }
}