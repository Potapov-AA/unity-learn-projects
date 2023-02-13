using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthlabel;
    [SerializeField] private InventoryPopup popup;
    [SerializeField] private TextMeshProUGUI levelEnding;

    void Awake(){
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelCompleted);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }
    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelCompleted);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    void Start() {
        OnHealthUpdated();

        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.M)){
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
    }

    public void SaveGame() {
        Managers.Data.SaveGameState();
    }

    public void LoadGame() {
        Managers.Data.LoadGameState();  
    }

    private void OnHealthUpdated() {
        string message = "Health: " + Managers.Player.health + "/" + Managers.Player.maxHealth;
        healthlabel.text = message;
    }

    private void OnLevelCompleted(){
        StartCoroutine(CompleteLevel());
    }
    private IEnumerator CompleteLevel(){
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";

        yield return new WaitForSeconds(2);

        Managers.Mission.GoToNext();
    }

    private void OnLevelFailed(){
        StartCoroutine(FailLevel());
    }
    private IEnumerator FailLevel(){
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed :(";

        yield return new WaitForSeconds(2);

        Managers.Player.Respawn();
        Managers.Mission.RestartCurrent();
    }

    private void OnGameComplete(){
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "You Finished the Game!";
    }
}
