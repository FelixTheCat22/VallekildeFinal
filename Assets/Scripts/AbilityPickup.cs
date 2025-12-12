using UnityEngine;


[CreateAssetMenu(fileName = "AbilityPickup", menuName = "Scriptable Objects/PickupTypes/AbilityPickup")]
public class AbilityPickup : PickupType
{
    public Ability ability;
    
    public enum Ability
    {
        Dash,
        Grapeshot
    }
    
    public override void OnPickup(Player player)
    {
        switch (ability)
        {
            case Ability.Dash:
                player.dashAvailable = true;
                break;
            case Ability.Grapeshot:
                player.grapeshot = true;
                break;
        }
    }
}
