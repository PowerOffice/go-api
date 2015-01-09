﻿using GoApi.Core;

namespace ApiAuthorization
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
            return new AuthorizationSettings
            {
                DeveloperKey = DemoSettings.DeveloperKey,
                ClientKey = clientKey,
                TokenStore = new BasicTokenStore(@"tokenstore.gks")
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
