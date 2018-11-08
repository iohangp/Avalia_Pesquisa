using System;

namespace Avalia_Pesquisa
{
    public class App
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "http://192.168.0.14/avaliapesquisa/";

        public static void Initialize()
        {
            if (UseMockDataStore)
                ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
            else
                ServiceLocator.Instance.Register<IDataStore<Item>, CloudDataStore>();
        }
    }
}
