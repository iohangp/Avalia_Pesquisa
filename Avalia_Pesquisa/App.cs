using System;

namespace Avalia_Pesquisa
{
    public class App
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "http://www.avaliapesquisa.dx.am";

        public static void Initialize()
        {
            if (UseMockDataStore)
                ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
            else
                ServiceLocator.Instance.Register<IDataStore<Item>, CloudDataStore>();
        }
    }
}
