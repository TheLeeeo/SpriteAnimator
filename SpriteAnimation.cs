using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    PlayOnce,
    Loop
}

public enum FrameOnFinished
{
    first,
    last
}

[System.Serializable]
[CreateAssetMenu(order = 30)]
public class SpriteAnimation : ScriptableObject
{
    private void OnEnable()
    {
        if(AnimationType.Loop == _animationType)
        {
            _frameOnFinished = FrameOnFinished.first; //For default sprite
        }
    }

    [SerializeField]
    private float _animationTime;
    public float animaitonTime => _animationTime;

    [SerializeField]
    private AnimationType _animationType = AnimationType.Loop;
    public AnimationType animationType => _animationType;

    [SerializeField]
    private FrameOnFinished _frameOnFinished;
    public FrameOnFinished frameOnFinished => _frameOnFinished;

    [SerializeField]
    private string _animationName;
    public string animationName => _animationName;

    [SerializeField]
    private Sprite[] _frames;
    public Sprite[] frames => _frames;   
}
