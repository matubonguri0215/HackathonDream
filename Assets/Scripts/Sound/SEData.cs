using UnityEngine;

[CreateAssetMenu(fileName = "SEData", menuName = "Audio/SEData")]
public class SEData : AudioDataBase
{
    [SerializeField]
    private SE_ID id;
    public SE_ID ID => id;

    [Header("SEÝ’è")]
    [SerializeField]
    private bool loop = false;
    public bool Loop => loop;

    [Header("3D‰¹‹¿Ý’è")]
    [SerializeField]
    private bool is3D = false;
    public bool Is3D => is3D;

    [SerializeField, Range(0f, 500f)]
    private float maxDistance = 100f;
    public float MaxDistance => maxDistance;

    [SerializeField, Range(0f, 1f)]
    private float spatialBlend = 1f;
    public float SpatialBlend => spatialBlend;

    [Header("ƒ‰ƒ“ƒ_ƒ€Ý’è")]
    [SerializeField]
    private bool randomPitch = false;
    public bool RandomPitch => randomPitch;

    [SerializeField, Range(0f, 0.5f)]
    private float pitchVaritation = 0.1f;
    public float PitchVariation => pitchVaritation;
}