using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {


    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParents kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO(){
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParents kitchenObjectParent) {
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParents already has a KitchenObject!");
        }

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParents GetKitchenObjectParent() {
        return kitchenObjectParent;
    }


}
