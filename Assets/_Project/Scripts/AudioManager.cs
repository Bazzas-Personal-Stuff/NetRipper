using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    
    public enum AudioState {
        off,
        attack,
        sustain,
        decay,
    }

    public AudioState driveActivityState = AudioState.off;
    public AudioSource driveActivity;
    public AudioSource messageSound;
    public AudioSource canarySource;
    public AudioClip[] canarySounds;

    public float driveActivityLengthMin = 1f;
    public float driveActivityLengthMax = 5f;
    private float driveActivityLength = 2f;

    public float driveActivityMinVolume = 0.2f;
    
    public float driveActivityInOutTime = 0.8f;
    private float driveActivityTimer;


    public void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }

        instance = this;
    }
    
    public void Update() {
        switch (driveActivityState) {
            case AudioState.off:
                driveActivity.volume = driveActivityMinVolume;
                break;
            case AudioState.attack:
                if (driveActivityTimer > driveActivityInOutTime) {
                    driveActivityTimer = 0;
                    driveActivityState = AudioState.sustain;
                }
                
                driveActivity.volume = Mathf.Lerp(driveActivityMinVolume, 1, driveActivityTimer / driveActivityInOutTime);
                driveActivityTimer += Time.deltaTime;
                break;
            case AudioState.sustain:
                if (driveActivityTimer > driveActivityLength) {
                    driveActivityTimer = 0;
                    driveActivityState = AudioState.decay;
                }

                driveActivity.volume = 1;
                driveActivityTimer += Time.deltaTime;
                break;
            case AudioState.decay:
                if (driveActivityTimer > driveActivityInOutTime) {
                    driveActivityTimer = 0;
                    driveActivityState = AudioState.off;
                }
                
                driveActivity.volume = Mathf.Lerp(1, driveActivityMinVolume, driveActivityTimer / driveActivityInOutTime);
                driveActivityTimer += Time.deltaTime;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void PlayDriveActivity() {
        driveActivityLength = Random.Range(driveActivityLengthMin, driveActivityLengthMax);
        switch (driveActivityState) {
            case AudioState.off:
                driveActivityState = AudioState.attack;
                driveActivityTimer = 0;
                break;
            case AudioState.attack:
                break;
            case AudioState.sustain:
                driveActivityTimer = 0;
                break;
            case AudioState.decay:
                driveActivityState = AudioState.attack;
                driveActivityTimer = driveActivityInOutTime - driveActivityTimer;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void PlayMessageSound() {
        messageSound.PlayOneShot(messageSound.clip);
    }

    public void PlayCanarySound() {
        int idx = Random.Range(0, canarySounds.Length);
        canarySource.PlayOneShot(canarySounds[idx]);
    }
}