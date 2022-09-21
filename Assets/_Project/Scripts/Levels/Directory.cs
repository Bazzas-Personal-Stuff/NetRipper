using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Directory : MonoBehaviour {

    public enum VisitState {
        invisible,
        pinged,
        visited,
        listed
    }

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
    public string dirName = "directory";
    public bool hasCanary;
    public SpriteRenderer circleSprite;
    public SpriteRenderer canarySprite;
    public TMP_Text label;

    public Directory[] connected;

    
    private void Start() {
        
        label.text = dirName;
        circleSprite.color = invisColor;
        label.color = invisColor;
        canarySprite.enabled = false;
        
    }


    private void Update() {
        if (_isFading) {
            Color curColor = Color.Lerp(_fadeBeginColor, _fadeEndColor, _fadeTimer / _fadeMaxTime);
            circleSprite.color = curColor;
            label.color = curColor;
            _fadeTimer += Time.deltaTime;
        }
    }
    

    public void Ping() {
        if (visitState == VisitState.invisible) {
            visitState = VisitState.pinged;
            StartFade(pingColor);
        }
    }

    public void Visit() {
        if ((int)visitState < (int)VisitState.visited) {
            visitState = VisitState.visited;
            StartFade(visitColor);
        }
        if (hasCanary) {
            LevelManager.instance.OnCanaryVisited();
        }
    }

    private void StartFade(Color end) {
        _fadeBeginColor = circleSprite.color;
        _fadeEndColor = end;
        _fadeTimer = 0;
        _isFading = true;
    }
}
