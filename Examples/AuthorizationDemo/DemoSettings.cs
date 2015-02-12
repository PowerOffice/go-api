namespace AuthorizationDemo
{
    public class DemoSettings
    {
        /// <summary>
        ///     The application key.
        /// 
        ///     To obtain an application key you must register as a Integration Partner at http://api.go.poweroffice.net
        /// </summary>
        public static string ApplicationKey = "7ef7b9c3-7cae-48c1-a70d-d765f02babd0";

        /// <summary>
        ///     A client key. This key is to the generic test client in the PowerOffice GO Development Test Environment
        ///     The key identifies which client in PowerOffice GO the actions performed in the code will affect.
        ///     The end user of the integration will provide this key to the vendor after authorizing the integration component
        ///     in the PowerOffice GO user interface.
        /// </summary>
        public static string TestClientKey = "a4775855-fda0-4412-a361-5fb1381618d6";
    }
}
