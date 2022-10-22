using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shockwave/Controller")]
public class ShockwaveController : ScriptableObject
{
    [SerializeField]
    private bool enabled = false;

    [SerializeField]
    private Vector3 epicenter = Vector3.zero;

    [SerializeField]
    private float distance = 1.0f;

    [SerializeField]
    private float maxDistance = 20.0f;

    [SerializeField]
    private float width = 1.0f;

    [SerializeField]
    private float intensity = 1.0f;

    [SerializeField]
    private float smoothing = 1.0f;

    [SerializeField]
    private float emissionIntensity = 140.0f;

    [SerializeField]
    private List<Material> environmentalMaterials = new List<Material>();

    private readonly int epicenterPropID = Shader.PropertyToID("_Shockwave_Epicenter_Position");
    private readonly int distancePropID = Shader.PropertyToID("_Shockwave_Distance");
    private readonly int maxDistancePropID = Shader.PropertyToID("_Shockwave_Max_Distance");
    private readonly int widthPropID = Shader.PropertyToID("_Shockwave_Width");
    private readonly int intensityPropID = Shader.PropertyToID("_Shockwave_Intensity");
    private readonly int smoothingPropID = Shader.PropertyToID("_Shockwave_Smothing");
    private readonly int emissionIntensityPropID = Shader.PropertyToID("_Shockwave_Emission_Intensity");
    private readonly int enabledPropID = Shader.PropertyToID("_Shockwave_Enabled");

    private void OnValidate()
    {
        foreach (var mat in environmentalMaterials)
        {
            if (mat == null)
                continue;

            mat.SetVector(epicenterPropID, epicenter);
            mat.SetFloat(distancePropID, distance);
            mat.SetFloat(maxDistancePropID, maxDistance);
            mat.SetFloat(widthPropID, width);
            mat.SetFloat(intensityPropID, intensity);
            mat.SetFloat(smoothingPropID, smoothing);
            mat.SetFloat(emissionIntensityPropID, emissionIntensity);
            mat.SetInt(enabledPropID, enabled ? 1 : 0);
        }
    }

    public void PlayShockwave(MonoBehaviour caller, Vector3 epicenter, float duration) => caller.StartCoroutine(ReleaseShockwave(epicenter, duration));

    private IEnumerator ReleaseShockwave(Vector3 epicenter, float duration)
    {
        foreach (var mat in environmentalMaterials)
        {
            if (mat == null)
                continue;

            mat.SetVector(epicenterPropID, epicenter);
            mat.SetFloat(distancePropID, 0.0f);
            mat.SetFloat(maxDistancePropID, maxDistance);
            mat.SetFloat(widthPropID, width);
            mat.SetFloat(intensityPropID, intensity);
            mat.SetFloat(smoothingPropID, smoothing);
            mat.SetFloat(emissionIntensityPropID, emissionIntensity);
            mat.SetInt(enabledPropID, 1);
        }

        float elapsedTime = 0.0f;
        while (elapsedTime <= duration)
        {
            float step = elapsedTime / duration;
            float dist = maxDistance * step;

            foreach (var mat in environmentalMaterials)
            {
                if (mat == null)
                    continue;

                mat.SetFloat(distancePropID, dist);
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        foreach (var mat in environmentalMaterials)
        {
            if (mat == null)
                continue;

            mat.SetInt(enabledPropID, 0);
        }
    }
}
