using UnityEngine;

[CreateAssetMenu(fileName = "Sfx data", menuName = "Scriptable/Sfx data")]
public class SfxData : ScriptableObject
{
    public AudioClip buttonClickClip;
    public AudioClip buttonReselaseClip;
    public AudioClip togglePressedClip;
    public AudioClip toggleChangedClip;
}
