using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Directory : MonoBehaviour {

    public enum VisitState {
        invisible,
        pinged,
        visited,
        listed
    }

    public Color currentColor;
    
    public Color invisColor;
    public Color pingColor;
    public Color visitColor;
    public Color listedColor;
    
    private bool _isFading;
    private float _fadeTimer;
    [SerializeField] private float _fadeMaxTime = 1;
    private Color _fadeBeginColor;
    private Color _fadeEndColor;
    
    public VisitState visitState = VisitState.invisible;
    public bool hasCanary;
    public SpriteRenderer circleSprite;
    public SpriteRenderer canarySprite;
    public TMP_Text label;

    [NonSerialized] public List<Directory> connected = new();
    [NonSerialized] public List<File> files = new();

    public UnityEvent onFirstVisit;
    public UnityEvent onFirstList;

    public UnityEvent onVisitStatusUpdate;

    private void Awake() {
        
        label.text = name;
        currentColor = invisColor;
        circleSprite.color = currentColor;
        label.color = currentColor;
        canarySprite.enabled = false;
        
    }


    private void Update() {
        if (_isFading) {
            _fadeTimer += Time.deltaTime;
            if (_fadeTimer > _fadeMaxTime) {
                _isFading = false;
            }
            currentColor = Color.Lerp(_fadeBeginColor, _fadeEndColor, _fadeTimer / _fadeMaxTime);
            circleSprite.color = currentColor;
            label.color = currentColor;
        }
    }
    

    public void Ping() {
        if (visitState == VisitState.invisible) {
            visitState = VisitState.pinged;
            StartFade(pingColor);
            onVisitStatusUpdate?.Invoke();
        }
    }

    public void Visit() {
        if ((int)visitState < (int)VisitState.visited) {
            visitState = VisitState.visited;
            StartFade(visitColor);
            onVisitStatusUpdate?.Invoke();
            onFirstVisit?.Invoke();
        }
        if (hasCanary) {
            canarySprite.enabled = true;
            LevelManager.instance.OnCanaryVisited();
        }
    }

    public void List() {
        if ((int)visitState < (int)VisitState.listed) {
            visitState = VisitState.listed;
            StartFade(listedColor);
            onFirstList?.Invoke();
            onVisitStatusUpdate?.Invoke();
        }

        foreach (Directory d in connected) {
            d.Ping();
        }

        foreach (File f in files) {
            f.Ping();
        }
    }

    private void StartFade(Color end) {
        _fadeBeginColor = circleSprite.color;
        _fadeEndColor = end;
        _fadeTimer = 0;
        _isFading = true;
    }
}
