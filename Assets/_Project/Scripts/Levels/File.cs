using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class File : MonoBehaviour {
    public enum VisitState {
        invisible,
        pinged,
        ripped
    }

    public VisitState visitState;

    public string fileName;
    public Color invisColor;
    public Color pingedColor;
    public Color rippedColor;

    public SpriteRenderer iconSprite;
    public TMP_Text label;
    
    public UnityAction onRip;

    public string readableText;
    
    private bool _isFading;
    private float _fadeTimer;
    [SerializeField] private float _fadeMaxTime = 1;
    private Color _fadeBeginColor;
    private Color _fadeEndColor;
    
    
    private void Start() {
        
        label.text = fileName;
        iconSprite.color = invisColor;
        label.color = invisColor;
    }

    private void Update() {
        if (_isFading) {
            Color curColor = Color.Lerp(_fadeBeginColor, _fadeEndColor, _fadeTimer / _fadeMaxTime);
            iconSprite.color = curColor;
            label.color = curColor;
            _fadeTimer += Time.deltaTime;
        }
    }
    
    public void Ping() {
        if (visitState == VisitState.invisible) {
            StartFade(pingedColor);
            visitState = VisitState.pinged;
        }
    }

    public void Rip() {
        if ((int)visitState < (int)VisitState.ripped) {
            StartFade(rippedColor);
            visitState = VisitState.ripped;
            onRip?.Invoke();
        }
    }
    
    private void StartFade(Color end) {
        _fadeBeginColor = iconSprite.color;
        _fadeEndColor = end;
        _fadeTimer = 0;
        _isFading = true;
    }
}
