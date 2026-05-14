using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[System.Serializable]
public struct PlayerAnimationClips
{
    public AnimationClip Idle;
    public AnimationClip Run;
    public AnimationClip Jump;
    public AnimationClip Fall;
    public AnimationClip Die;
    public AnimationClip Hit;
}

public class PlayerPlayableAnimator : IDisposable
{
    public enum State { Idle, Run, Jump, Fall, Die, Hit }

    private PlayableGraph _graph;
    private AnimationPlayableOutput _output;
    private AnimationClipPlayable _currentPlayable;

    private readonly PlayerAnimationClips _clips;
    private State _currentState;

    public PlayerPlayableAnimator(Animator animator, PlayerAnimationClips clips)
    {
        _clips = clips;

        _graph = PlayableGraph.Create("PlayerAnimatorGraph");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        _output = AnimationPlayableOutput.Create(_graph, "Animation", animator);

        SetState(State.Idle, true);
    }

    public void SetState(State state, bool forceUpdate = false)
    {
        if (!forceUpdate && _currentState == state) return;

        _currentState = state;
        AnimationClip clip = GetClip(state);

        if (clip == null)
        {
            Debug.LogError($"[PlayerPlayableAnimator] Missing clip for state: {state}");
            return;
        }

        if (_currentPlayable.IsValid())
        {
            _currentPlayable.Destroy();
        }

        _currentPlayable = AnimationClipPlayable.Create(_graph, clip);
        _output.SetSourcePlayable(_currentPlayable);

        _graph.Play();
    }

    private AnimationClip GetClip(State state)
    {
        return state switch
        {
            State.Idle => _clips.Idle,
            State.Run => _clips.Run,
            State.Jump => _clips.Jump,
            State.Fall => _clips.Fall,
            State.Die => _clips.Die,
            State.Hit => _clips.Hit,
            _ => _clips.Idle
        };
    }

    // Обязательно к вызову при уничтожении объекта, иначе будет утечка памяти (Memory Leak)
    public void Dispose()
    {
        if (_graph.IsValid())
            _graph.Destroy();
    }
}