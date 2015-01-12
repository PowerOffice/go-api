namespace AuthorizationDemo
{
    public class DemoSettings
    {
        /// <summary>
        ///     The application key.
        /// 
        ///     To obtain an application key you must register as a Integration Partner at http://api.go.poweroffice.net
        /// </summary>
        public static string ApplicationKey = "1970F6AD-E35E-4EBF-9DA7-510962CE7E46";

        /// <summary>
        ///     A client key. This key is to the generic test client in the PowerOffice GO Development Test Environment
        ///     The key identifies which client in PowerOffice GO the actions performed in the code will affect.
        ///     The end user of the integration will provide this key to the vendor after authorizing the integration component
        ///     in the PowerOffice GO user interface.
        /// </summary>
        public static string TestClientKey = "1813544B-CF08-49E7-A960-0D4344ABE2C1";
    }
}
