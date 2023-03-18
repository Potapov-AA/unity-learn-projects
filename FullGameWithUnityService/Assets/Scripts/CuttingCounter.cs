using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {


    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // Когда не имеет KitchenObject
            if (player.HasKitchenObject()) {
                // Player несет что-то
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                // Player ничего не несет
            }
        } else {
            // Когда имеет KitchenObject
            if (player.HasKitchenObject()) {
                // Player несет что-то
            } else {
                // Player ничего не несет
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject()) {
            // Если имеет KitchenObject
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
