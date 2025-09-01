using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    // Array of all character GameObjects
    public GameObject[] characters;
    // Index of currently selected character
    public int selectedCharacter = 0;
    // UI text to display character name
    public TextMeshProUGUI characterName;

    // Switch to next character
    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].transform.position = new Vector3(0, 1, 0);
        characters[selectedCharacter].SetActive(true);
        characterName.text = characters[selectedCharacter].gameObject.name;
    }

    // Switch to previous character
    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].transform.position = new Vector3(0,1,0);
        characters[selectedCharacter].SetActive(true);
        characterName.text = characters[selectedCharacter].gameObject.name;
    }

    // Save selected character and start the game
    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    // Play button click sound Auxillary Function for Character Select Scene
    public void ButtonClick()
    {
        AudioManager.Instance.PlaySound(0);
    }
}
