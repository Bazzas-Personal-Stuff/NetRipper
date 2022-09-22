using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class File : MonoBehaviour {
    public enum VisitState {
        invisible,
        pinged,
        read,
        ripped
    }

    public VisitState visitState;

    public Color invisColor;
    public Color pingedColor;
    public Color readColor;
    public Color rippedColor;
    
    public Color currentColor;

    public SpriteRenderer iconSprite;
    public TMP_Text label;

    public UnityEvent onRead;
    public UnityEvent onRip;

    [Multiline(5)]
    public string readableText;
    
    private bool _isFading;
    private float _fadeTimer;
    [SerializeField] private float _fadeMaxTime = 1;
    private Color _fadeBeginColor;
    private Color _fadeEndColor;
    
    
    private void Awake() {

        label.text = name;
        currentColor = invisColor;    
        iconSprite.color = currentColor;
        label.color = currentColor;
    }

    private void Update() {
        if (_isFading) {
            _fadeTimer += Time.deltaTime;
            if (_fadeTimer > _fadeMaxTime) {
                _isFading = false;
            }
            currentColor = Color.Lerp(_fadeBeginColor, _fadeEndColor, _fadeTimer / _fadeMaxTime);
            iconSprite.color = currentColor;
            label.color = currentColor;
            _fadeTimer += Time.deltaTime;
        }
    }
    
    public void Ping() {
        if (visitState == VisitState.invisible) {
            StartFade(pingedColor);
            visitState = VisitState.pinged;
        }
    }

    public void Read() {
        if ((int)visitState < (int)VisitState.read) {
            StartFade(readColor);
            visitState = VisitState.read;
            onRead?.Invoke();
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
