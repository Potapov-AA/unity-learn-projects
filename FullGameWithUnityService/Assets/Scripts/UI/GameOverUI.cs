using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI recipeDeliveredText;


    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

        Hide();
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            Show();

            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}