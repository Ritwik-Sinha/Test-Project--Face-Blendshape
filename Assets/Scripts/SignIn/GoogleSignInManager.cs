using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;

namespace SignIn
{
    public class GoogleSignInManager : MonoBehaviour
    {
        private string webClientId = "433078046252-drlfpj6m2n08aafv41q82piko5mdck0v.apps.googleusercontent.com";
        private GoogleSignInConfiguration configuration;

        public static GoogleSignInManager Instance;
        private static Action onSignInSuccess;
        private static Action onSignInFail;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }

        void Start()
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = webClientId,
                RequestEmail = true,
                RequestIdToken = true,
                UseGameSignIn = false,
                RequestProfile = true
            };
        }

        public void SignIn(Action onSignInSuccess, Action onSignInFail)
        {
            GoogleSignInManager.onSignInSuccess = onSignInSuccess;
            GoogleSignInManager.onSignInFail = onSignInFail;
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }

        public void SignOut()
        {
            DebugDisplay("Calling SignOut");
            GoogleSignIn.DefaultInstance.SignOut();
        }

        public void OnDisconnect()
        {
            DebugDisplay("Calling Disconnect");
            GoogleSignIn.DefaultInstance.Disconnect();
        }

        public void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                        DebugDisplay("Got Error: " + error.Status + " " + error.Message);
                    }
                    else
                    {
                        DebugDisplay("Got Unexpected Exception?!?" + task.Exception);
                    }
                }
                DebugDisplay("Some fault");
                onSignInFail?.Invoke();
            }
            else if (task.IsCanceled)
            {
                DebugDisplay("Canceled");
                onSignInFail?.Invoke();
            }
            else
            {
                DebugDisplay("Welcome: " + task.Result.DisplayName + "!");
                DebugDisplay("Email = " + task.Result.Email);
                DebugDisplay("Google ID Token = " + task.Result.IdToken);
                DebugDisplay("Email = " + task.Result.Email);

                onSignInSuccess?.Invoke();
            }
        }

        public void OnGamesSignIn()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = true;
            GoogleSignIn.Configuration.RequestIdToken = false;

            DebugDisplay("Calling Games SignIn");

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }

        public void SignInSilently(Action onSignInSuccess = null, Action onSignInFail = null)
        {
            GoogleSignInManager.onSignInSuccess = onSignInSuccess;
            GoogleSignInManager.onSignInFail = onSignInFail;
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            DebugDisplay("Calling SignIn Silently");

            GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
        }

        private void DebugDisplay(string str)
        {
            Debug.Log(str);
        }

    }
}