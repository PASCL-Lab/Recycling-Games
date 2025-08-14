using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public TextMeshProUGUI characterName;

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].transform.position = new Vector3(0, 1, 0);
        characters[selectedCharacter].SetActive(true);
        characterName.text = characters[selectedCharacter].gameObject.name;
    }

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

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void ButtonClick()
    {
        AudioManager.Instance.PlaySound(0);
    }
}
