using System;
using Xamarin.Auth;

namespace Xamarin_FacebookAuth.Authentication
{
    public class FacebookAuthenticator
    {
        private const string AuthorizeUrl = "https://www.facebook.com/v2.0/dialog/oauth/";
        private const string RedirectUrl = "http://www.facebook.com/connect/login_success.html";
        private const bool IsUsingNativeUI = false;

        private OAuth2Authenticator _auth;
        private IFacebookAuthenticationDelegate _authenticationDelegate;

        public FacebookAuthenticator(string clientId, string scope, IFacebookAuthenticationDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;

            _auth = new OAuth2Authenticator(clientId, scope,
                                            new Uri(AuthorizeUrl),
                                            new Uri(RedirectUrl),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        public void OnPageLoading(Uri uri)
        {
            _auth.OnPageLoading(uri);
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = new FacebookOAuthToken
                {
                    AccessToken = e.Account.Properties["access_token"]
                };
                _authenticationDelegate.OnAuthenticationCompleted(token);
            }
            else
            {
                _authenticationDelegate.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticationDelegate.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}
