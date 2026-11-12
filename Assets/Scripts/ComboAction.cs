using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Create ComboAction", fileName = "ComboAction", order = 0)]
public class ComboAction : ScriptableObject
{
    public AnimatorOverrideController AnimatorOverrideController;
    public DamageInfo DamageInfo;
    public float AttackTime;
}
