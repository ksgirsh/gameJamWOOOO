using UnityEngine;
using UnityEngine.UI;

public class MainMenuMusicButton : MonoBehaviour
{
    [SerializeField] AudioSource music;
    bool toggle;
    [SerializeField] Sprite[] toggleSprite;
    private Image rend;

    void Start()
    {
        rend = gameObject.GetComponent<Image>();

    }

    public void Toggle()
    {
        toggle = !toggle;
        if (toggle == true)
        {
            music.volume = 0;
            rend.sprite = toggleSprite[1];

        } else
        {
            music.volume = 1f;
            rend.sprite = toggleSprite[0];
        }
    }
}
