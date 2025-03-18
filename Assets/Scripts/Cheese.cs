using UnityEngine;

public class Cheese : ObstacleBase
{
    [SerializeField] private AudioClip _collideClip;

    protected override void OnMouseCollided(Mouse mouse)
    {
        Context.Instance.IncreaseScore();

        if(_collideClip != null)
            Context.Instance.PlaySoundOneShot(_collideClip);

        Release();
    }
}