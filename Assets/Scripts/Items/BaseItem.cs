using UnityEngine;
using BananaGame.Player;

namespace BananaGame.Items;

public abstract class BaseItem : MonoBehaviour
{
    public string ItemName;
    public Sprite ItemIcon;
    [TextArea] public string ItemDescription;

    ///<summary>This method is called when the player uses the item.</summary>
    public abstract void Use(PlayerController player);

}