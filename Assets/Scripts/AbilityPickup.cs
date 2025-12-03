using UnityEngine;

public class AbilityPickup : Pickup
{
    public Ability ability;
    
    public enum Ability
    {
        Dash,
        Grapeshot
    }
    
    protected override void OnPickup(Player player)
    {
        switch (ability)
        {
            case Ability.Dash:
                player.dashAvailable = true;
                break;
            case Ability.Grapeshot:
                Debug.Log("Unimplemented grapeshot");
                break;
        }
    }
}
