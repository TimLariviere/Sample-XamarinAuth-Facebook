using System;
using UIKit;
using Xamarin_FacebookAuth.Authentication;
using Xamarin_FacebookAuth.Services;

namespace Xamarin_FacebookAuth.iOS
{
    public partial class ViewController : UIViewController, IFacebookAuthenticationDelegate
	{
		public ViewController(IntPtr handle) : base (handle)
		{
            
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            FacebookLoginButton.TouchUpInside += OnFacebookLoginButtonClicked;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            var auth = new FacebookAuthenticator(Configuration.ClientId, Configuration.Scope, this);
            var authenticator = auth.GetAuthenticator();
            var viewController = authenticator.GetUI();
            PresentViewController(viewController, true, null);
        }

        public async void OnAuthenticationCompleted(FacebookOAuthToken token)
        {
            DismissViewController(true, null);

            var facebookService = new FacebookService();
            var email = await facebookService.GetEmailAsync(token.AccessToken);

            FacebookLoginButton.SetTitle($"Connected with {email}", UIControlState.Normal);
        }

        public void OnAuthenticationFailed(string message, Exception exception)
		{
			DismissViewController(true, null);

            var alertController = new UIAlertController
            {
                Title = message,
                Message = exception?.ToString()
            };
            PresentViewController(alertController, true, null);
        }

        public void OnAuthenticationCanceled()
		{
			DismissViewController(true, null);

			var alertController = new UIAlertController
			{
				Title = "Authentication canceled",
				Message = "You didn't completed the authentication process"
			};
			PresentViewController(alertController, true, null);
        }
    }
}

