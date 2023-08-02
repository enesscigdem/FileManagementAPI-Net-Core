using FileOrbis.DataAccessLayer.Context;

namespace FileOrbisApi.ContextSingleton
{
    public class FileOrbisContextSingleton
    {
        private static FileOrbisContext _instance;
        private static readonly object _lockObject = new object();

        private FileOrbisContextSingleton()
        {
            _instance = new FileOrbisContext();
        }

        public static FileOrbisContext GetInstance()
        {
            lock (_lockObject)
            {
                if (_instance == null)
                {
                    _instance = new FileOrbisContext();
                }
            }
            return _instance;
        }
    }
}
