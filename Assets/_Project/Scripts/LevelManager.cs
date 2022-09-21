using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public Transform cameraTarget;
    public Dictionary<string, Level> levelDict = new();
    
    [Serializable] public struct LevelKeys {
        public string key;
        public Level level;
    }
    [SerializeField] private LevelKeys[] _levelKeyCorrespondence;
    
    
    public bool isConnectedToRemote;
    public Level currentLevel;
    public Directory workingDir;

    public bool hasVisitedCanary;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        foreach (LevelKeys pair in _levelKeyCorrespondence) {
            levelDict.Add(pair.key, pair.level);
        }
    }


    public void Update() {
        if (isConnectedToRemote) {
            cameraTarget.position = workingDir.transform.position;
        }
        else {
            cameraTarget.position = Vector3.zero;
        }
        
    }

    
    public void Connect(string level) {
        Connect(levelDict[level]);
    }

    public void Connect(Level level) {
        currentLevel = level;
        isConnectedToRemote = true;
        workingDir = currentLevel.entryPoint;
        workingDir.Visit();
    }

    public void Disconnect() {
        isConnectedToRemote = false;
    }

    public void OnCanaryVisited() {
        StartCoroutine(BeginReconnect());
        if (!hasVisitedCanary) {
            hasVisitedCanary = true;
            CommandLineManager.instance.SubmitSilentCommand("dbg_dialogue 1");
        }
    }

    public IEnumerator BeginReconnect() {
        Disconnect();
        yield return new WaitForSeconds(0.5f);
        Connect(currentLevel);
    }



    }
