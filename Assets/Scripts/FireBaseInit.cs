using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;

public  class FireBaseInit : MonoBehaviour
{
    private DependencyStatus dependencyStatus;
    public static FireBaseInit instance;
    private bool _isInit;

    private void Awake()
    {
        if (instance == null) { 
            instance = this; 
        } else if (instance == this)
        {
            Destroy(gameObject); 
        }
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        
        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWith(checkTask => {
            // Peek at the status and see if we need to try to fix dependencies.
            Firebase.DependencyStatus status = checkTask.Result;
            if (status != Firebase.DependencyStatus.Available) {
                return Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith(t => {
                    return Firebase.FirebaseApp.CheckDependenciesAsync();
                }).Unwrap();
            } else {
                return checkTask;
            }
        }).Unwrap().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // TODO: Continue with Firebase initialization.
               _isInit=true;
            } else {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void FirebaseStartLevel(int levelNumber=1)
    {
        
        if (!_isInit) return;
        Debug.Log("SendFirebaseEvent");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
            new Parameter(FirebaseAnalytics.ParameterLevel,levelNumber));
    }
}
