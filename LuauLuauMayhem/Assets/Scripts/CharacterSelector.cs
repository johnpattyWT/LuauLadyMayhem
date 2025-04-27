using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characterModels; // Assign your character models
    public Animator playerAnimator; // The main player Animator
    public Avatar[] avatars; // The avatars that match the models
    public TMP_Dropdown dropdown; // Your dropdown UI element

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnCharacterSelected);
    }

    void OnCharacterSelected(int index)
    {
        // Deactivate all models
        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(i == index);
        }

        // Swap only the Avatar
        if (avatars.Length > index)
        {
            playerAnimator.avatar = avatars[index];
        }
    }
}
