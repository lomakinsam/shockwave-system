using System.Collections;
using UnityEngine;

public class ShockwaveTrigger : MonoBehaviour
{
    [SerializeField]
    private ShockwaveController shockwaveController;

    [SerializeField]
    private Vector3 epicenter = Vector3.zero;

    [SerializeField]
    private float duration = 3.0f;

    [SerializeField]
    private float activationDelay = 3.0f;

    private void Start() => StartCoroutine(TriggerShockwave());

    private IEnumerator TriggerShockwave()
    {
        yield return new WaitForSeconds(activationDelay);

        shockwaveController.PlayShockwave(this, epicenter, duration);
    }
}
