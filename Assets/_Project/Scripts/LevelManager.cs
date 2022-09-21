using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public Transform cameraTarget;
    public Dictionary<string, Level> levelDict = new();
    
    [Serializable] public struct LevelKeys {
        public string key;
        public Level level;
    }
    [SerializeField] private LevelKeys[] _levelKeyCorrespondence;


    public HashSet<string> recentConnectionSet = new();
    public TMP_Text recentConnectionString;
    
    public bool isConnectedToRemote;
    public Level currentLevel;
    public Directory workingDir;
    public SpriteRenderer playerSprite;

    public bool hasVisitedCanary;
    public UnityEvent onDisconnect;

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
            playerSprite.transform.position = workingDir.transform.position;
            playerSprite.enabled = true;
        }
        else {
            playerSprite.enabled = false;
        }
        
    }

    
    public void Connect(string level) {
        if (!recentConnectionSet.Contains(level)) {
            recentConnectionSet.Add(level);
            recentConnectionString.text += $"\n{level}";
        }
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
        cameraTarget.position = Vector3.zero;
        onDisconnect?.Invoke();
    }

    public void OnCanaryVisited() {
        StartCoroutine(BeginReconnect());
        if (!hasVisitedCanary) {
            hasVisitedCanary = true;
            CommandLineManager.instance.SubmitSilentCommand("dbg_dialogue 2");
        }
    }

    public IEnumerator BeginReconnect() {
        // Disconnect();
        isConnectedToRemote = false;
        yield return new WaitForSeconds(2f);
        Connect(currentLevel);
    }



    }
