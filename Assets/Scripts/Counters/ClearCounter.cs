using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //no kitchen object
            if (player.HasKitchenObject()) {
                //player carry something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                //player not carry anything
            }
        } else {
            //has kitchen object
            if (player.HasKitchenObject()) {
                //player carry something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //player carry plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) { 
                        GetKitchenObject().DestroySelf();
                    }
                } else {
                    //player carry no plate but smth else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) { 
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else {
                //player not carry anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
