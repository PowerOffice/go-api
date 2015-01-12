Authentication, Technical details
=================================

This document describes the technical details of the authorization and authentication process. 

### GO-API SDK
If you use the C# GO-API SDK or a third party oAuth client library, you should not need to dig into the technical details in this document. The details is provided for developers that for some reason can not use the SDK or a third party oAuth client library.

### Third party oAuth client libraries

If you use a third party oAuth library to assist with the authentication process, the *application key* is provided in as the *client key* value and the *client key* equals the oAuth *client secret*. 

A list of oAuth client libraries can be found on at [oauth.net](http://oauth.net/2/).

The oAuth spesification documentation can be found [here](http://tools.ietf.org/html/rfc6749). Read [Section 4.4 Client Credentials Grant](http://tools.ietf.org/html/rfc6749#section-4.4) for the specifics of the grant flow used by the GO-API.

# Prerequisites

Before your integration component can be authorized to perform API calls, you must first [register as a developer](../Registration.md) to obtain an *application key*.


# The authorization process

Befor any API calls can be made, you must obtain an access key from the authorization server. The access key must be provided with all calls to the API.

The authentication process use the oAuth2.0 client credentials grant flow to request and provide access keys and refresh keys. All calls must be made with the https protocol. Http requests will be denied.

When you have obtained an *application key* and an *client key* following the process described in **First steps** you are ready to request an *access token* from the authorization server.

When you send the *application key* and the *client key* to the authorization server, you will receive an *access token* and a *refresh token* from the authorization server.

The *access token* is then provided to all API calls. The *access token* will expire after 20 minutes. You will then request a new *access token* from the authorization server using the *refresh token*.


## Requesting access and refresh tokens

Requests to the authorization server must provide the *application key* and an *client key* in the `Authorization` header. The keys must be separated by `:` and the whole string must be Base64 encoded.

The `Content-Type` header must be set to `application/x-www-form-urlencoded` and the body must contain `grant_type=client`

A typical request would look something like this:

	POST https://go.poweroffice.net/OAuth/Token HTTP/1.1
	Authorization: Basic MTk3MEY2QUQtRTM1RS00RUJGLTlEQTctNTEwOTYyQ0U3RTQ2OjE4MTM1NDRCLUNGMDgtNDlFNy1BOTYwLTBENDM0NEFCRTJDMQ==
	Content-Type: application/x-www-form-urlencoded; charset=utf-8

	grant_type=client_credentials

The response from the authorization server would then look something like this:

	HTTP/1.1 200 OK
	Content-Type: application/json;charset=UTF-8
	Access-Control-Allow-Origin: *

	{
		"access_token":"JSM8aIY5SQofcuLZnOkTvuTL5N-RLepbX2xdUIE39zW-JhCQDyqAnXjUMW1nbfA2u3My4iWny5pA1u_wceV64WWPj9UUo927cp5AKojxbkH8Wbjnie2656VXzZ6nnftYl55j3Dz-5t_a8YPHgaPGkMXJ6tus2sagtsrb6MiPoxD36dGyPiefmLVthO9DfvM2UmDGXCwjV9yPI5g4QNEqvOmLwGAWnjo8s0oVsT7IMhsBZQe4rFIUNRkuF1-qwySMy_eMpbfoXKaiTYMowDb8f8pxMuWGn7HOLS2vEXq72m-Bl95hKNcGRbFcww1WFGDy7_0q3QaIxHxz4sImFZf5xaD0cxDt2YFMlaD9UjheR6OfKjcYbhg7PK_wNh-VNzTNJKMUu8zdRTiO28QBxLq91TQHnRaS4_V-FYhxOiR3LyGFDzBXzBZyUfVd5_GxjT4H8JdPGoqz85ffZRm2YetBCnPP_ue5XPvSoHhKX_ANsxH8Tf7bn4QVChiR3oS4KTTs",
		"token_type":"bearer",
		"expires_in":599,
		"refresh_token":"B9ZhmwKNf-pZZV92HSnoizwVCg_vebl5Ixne7_tS3rRylOEo2aNygNZ9ILFnJxyma0MU1y1e27KMC2sFW3MJKuCwxr-RZ-a7IlCPVJYfF2DN5rRLIBgqkgukq3z8YMvSg2SXowfYSHxOFWIofrv8L6kkstzc-rWtVaWQxlS6TM8oeGLHWRC48aLabCDE2J10FWVCmKGNFzqUKT8AgPMsnVzYMLzBB_DPJbVyLo8MxERzJLNPvU4Yfd0-Lu2I_QGY-JBqCN3cQ4vQOqWxCrmuoZ2HWy8a453U_raYfFzF959DxnRZ1meJ9yL7n_YDbcJVmKHDGZBfS_ojqrw97d0PEgoVCJzAc4n9GcTZXmXHriOx2_3Ll2c-T4FY1Jh0UjZRE3axkmMIqDAZtedH6ySnFezCol4HuujujG0bU-VSGmNw9RTpULg_FAKfRpxGeRpvF9EzyrT-yt7EIUcLCgGmZXHhdCRyXeAcIbginS0Pu7spRU9M3dGESY5w3-sYTKeR"
	}

The *access token*, expiration date/time and the refresh token should be safely stored somewhere to be reused on subsequent API requestes. The *access token* can be reused by new sessions until it expires. A new *access token* can then be requested using the *refresh token*.

## Requesting a new access token using the refresh token

**TODO: Details not documented yet**


## Calling a GO-API function

After an *access token* has been obtained, you can use it to call GO-API functions. The *access token* is provided in the request to authorize/authenticate the API call.

A typical GO-API request would look something like this:

	GET https://api.go.poweroffice.net/customer/?organizationNo=123456789 HTTP/1.1
	Authorization: Bearer JSM8aIY5SQofcuLZnOkTvuTL5N-RLepbX2xdUIE39zW-JhCQDyqAnXjUMW1nbfA2u3My4iWny5pA1u_wceV64WWPj9UUo927cp5AKojxbkH8Wbjnie2656VXzZ6nnftYl55j3Dz-5t_a8YPHgaPGkMXJ6tus2sagtsrb6MiPoxD36dGyPiefmLVthO9DfvM2UmDGXCwjV9yPI5g4QNEqvOmLwGAWnjo8s0oVsT7IMhsBZQe4rFIUNRkuF1-qwySMy_eMpbfoXKaiTYMowDb8f8pxMuWGn7HOLS2vEXq72m-Bl95hKNcGRbFcww1WFGDy7_0q3QaIxHxz4sImFZf5xaD0cxDt2YFMlaD9UjheR6OfKjcYbhg7PK_wNh-VNzTNJKMUu8zdRTiO28QBxLq91TQHnRaS4_V-FYhxOiR3LyGFDzBXzBZyUfVd5_GxjT4H8JdPGoqz85ffZRm2YetBCnPP_ue5XPvSoHhKX_ANsxH8Tf7bn4QVChiR3oS4KTTs

The response would then be something like this:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"code":"123",
			"name":"Hello World!",
			"organizationNo":"123456789"
		},
		"success":true
	}

