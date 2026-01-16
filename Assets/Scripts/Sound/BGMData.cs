using UnityEngine;

[CreateAssetMenu(fileName = "BGMData", menuName = "Audio/BGMData")]
public class BGMData : AudioDataBase
{
    [SerializeField]
    private BGM_ID id;
    public BGM_ID ID => id;

    [Header("BGMÝ’è")]
    [SerializeField]
    private bool loop = true;
    public bool Loop => loop;

    [SerializeField]
    private float introLength = 0f;
    public float IntroLength => introLength;
}
