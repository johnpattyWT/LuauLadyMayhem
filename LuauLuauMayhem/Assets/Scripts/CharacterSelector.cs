using UnityEngine;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characterModels;
    public Animator playerAnimator;
    public Avatar[] avatars;
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnCharacterSelected);
    }

    void OnCharacterSelected(int index)
    {
        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(i == index);
        }

        if (index < avatars.Length)
        {
            playerAnimator.avatar = avatars[index];
        }
    }
}