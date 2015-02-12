using GoApi.Core;
using GoApi.Global;

namespace AuthorizationDemo
{
    public class Authorize
    {
        /// <summary>
        ///     Helper method to create an authorization settings instance for the given client key.
        ///     Access and access refresh tokens will be stored in a BasicTokenStore in the working folder
        /// </summary>
        /// <param name="clientKey">The client key.</param>
        /// <returns>AuthorizationSettings.</returns>
        public static AuthorizationSettings CreateAuthorizationSettings(string clientKey)
        {
            GoApi.Global.Settings.Mode = Settings.EndPointMode.Debug;

            return new AuthorizationSettings
            {
                ApplicationKey = DemoSettings.ApplicationKey,
                ClientKey = clientKey,
                TokenStore = new BasicTokenStore(@"tokenstore2.gks")
            };
        }

        /// <summary>
        ///     Helper method to create an authorization for the test client.
        /// </summary>
        /// <returns>Authorization.</returns>
        public static GoApi.Core.Authorization TestClientAuthorization()
        {
            return new GoApi.Core.Authorization(CreateAuthorizationSettings(DemoSettings.TestClientKey));
        }
    }
}
