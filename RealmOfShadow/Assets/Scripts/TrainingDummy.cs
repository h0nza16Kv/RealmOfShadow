using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TriggerHurtAnimation()
    {
        anim.SetTrigger("Hurt");
    }
}
