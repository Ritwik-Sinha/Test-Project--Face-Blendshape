using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidSceneUIManager : MonoBehaviour
{
    [SerializeField] private Button playReactionButton;
    [SerializeField] private ReactionController reactionController;
    void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        playReactionButton.onClick.AddListener(PlayReaction);
    }
    private void PlayReaction()
    {
        reactionController.OnPlayReactionButton();
    }
}
