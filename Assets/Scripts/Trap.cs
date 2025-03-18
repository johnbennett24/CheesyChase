using UnityEngine;

public class Trap : ObstacleBase
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _trigger = "Collided";

    [SerializeField] private AudioClip _collideClip;

    public override void Release()
    {
        base.Release();

        _animator.Rebind();
    }

    protected override void OnMouseCollided(Mouse mouse)
    {
        Context.Instance.FinishGame();

        _animator.SetTrigger(_trigger);

        if (_collideClip != null)
            Context.Instance.PlaySoundOneShot(_collideClip);
    }
}