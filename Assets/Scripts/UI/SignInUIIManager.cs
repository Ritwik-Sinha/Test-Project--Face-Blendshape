using System;
using System.Collections;
using System.Collections.Generic;
using SignIn;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInUIIManager : MonoBehaviour
{
    [SerializeField] private Button signInButton;
    
    void Start()
    {
        AddListeners();    
    }

    private void AddListeners()
    {
        signInButton.onClick.AddListener(GoogleSignInCalled);
    }

    private void GoogleSignInCalled()
    {
        GoogleSignInManager.Instance.SignIn(()=>
        {
            Debug.Log("Sign In Successful with Button Click");
            SceneManager.LoadScene("HumanoidScene");
        }, ()=>
        {
            Debug.Log("Sign In Failed with Button Click");
        });
    }
}
