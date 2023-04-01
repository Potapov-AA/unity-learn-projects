using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;


    private List<RecipeSO> waitingRecipeSOList;
    private float spwanRecipeTimer;
    private float spwanRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }


    private void Update() {
        spwanRecipeTimer -= Time.deltaTime;
        if (spwanRecipeTimer < 0f) {
            spwanRecipeTimer = spwanRecipeTimerMax;

            if (waitingRecipeSOList.Count <= waitingRecipeMax) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i=0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            bool plateContentsMatchesRecipe = true;
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                // Одинаковое количество ингридиентов
                bool ingridientFound = false;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingridientFound = true;
                            break;
                        }
                    }
                    if (!ingridientFound) {
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe) {
                    Debug.Log("Player delivered the correct recipe!");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }

        // Не найдено корректного рецепта
        Debug.Log("Player did not delivered the correct recipe!");
    }

}
