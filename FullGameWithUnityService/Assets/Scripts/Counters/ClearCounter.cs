using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {


    [SerializeField] private KitchenObjectSO kitchenObjectSO;


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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player несет тарелку
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                } else {
                    // Player несет не тарелку, а что-то другое
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // На стойке контейнер
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                         
                    }
                }
            } else {
                // Player ничего не несет
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
