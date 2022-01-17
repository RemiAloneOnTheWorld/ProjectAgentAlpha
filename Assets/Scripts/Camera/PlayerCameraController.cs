using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    private Animator _animator;
    private static readonly int OnAttackPhase = Animator.StringToHash("OnAttackPhase");
    private static readonly int OnPreparationPhase = Animator.StringToHash("OnPreparationPhase");
    private static readonly int OnDestroyPhase = Animator.StringToHash("OnDestructionPhase");
    private static readonly int OnSwitchToFirst = Animator.StringToHash("SwitchToFirstCam");
    private static readonly int OnSwitchToSecond = Animator.StringToHash("SwitchToSecCam");

    private void Start() {
        _animator = GetComponent<Animator>();

    }

    public void OnPrepPhaseOver() {
        _animator.ResetTrigger(OnPreparationPhase);
        _animator.SetTrigger(OnAttackPhase);
    }

    public void OnAttackPhaseOver() {
        _animator.ResetTrigger(OnAttackPhase);
        _animator.SetTrigger(OnDestroyPhase);
    }

    public void OnDestructionPhaseOver() {
        _animator.ResetTrigger(OnDestroyPhase);
        _animator.SetTrigger(OnPreparationPhase);
    }

    public void SwitchToFirstDestructionCamera() {
        _animator.ResetTrigger(OnSwitchToSecond);
        _animator.SetTrigger(OnSwitchToFirst);
    }

    public void SwitchToSecondDestructionCamera() {
        _animator.ResetTrigger(OnSwitchToFirst);
        _animator.SetTrigger(OnSwitchToSecond);
    }
}
