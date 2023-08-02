using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    public Sprite muteSprite, unmuteSprite;
    private SpriteRenderer musicSpriteRend;
    private SpriteRenderer sfxSpriteRend;

    // Start is called before the first frame update
    void Start()
    {
        musicVolume = SharedData.musicVolume;
        sfxVolume = SharedData.sfxVolume;

        var musicSprite = musicSlider.transform.Find("NoteSprite");
        musicSpriteRend = musicSprite.GetComponent<SpriteRenderer>();
        var sfxSprite = sfxSlider.transform.Find("NoteSprite");
        sfxSpriteRend = sfxSprite.GetComponent<SpriteRenderer>();

        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);
    }

    public void SetMusicVolume(float _volume)
    {
        if (_volume < 0)
        {
            _volume = 0f;
        }
        
        musicSpriteRend.sprite = _volume > 0f ? unmuteSprite : muteSprite;

        RefreshMusicSlider(_volume);
        SharedData.musicVolume = _volume;

    }

    public void SetVolumeFromMusicSlider()
    {
        SetMusicVolume(musicSlider.value);
    }

    private void RefreshMusicSlider(float _volume)
    {
        musicSlider.value = _volume;
    }

    public void SetSfxVolume(float _volume)
    {
        if (_volume < 0)
        {
            _volume = 0f;
        }

        sfxSpriteRend.sprite = _volume > 0f ? unmuteSprite : muteSprite;

        RefreshSfxSlider(_volume);
        SharedData.sfxVolume = _volume;

    }

    public void SetVolumeFromSfxSlider()
    {
        SetSfxVolume(sfxSlider.value);
    }

    private void RefreshSfxSlider(float _volume)
    {
        sfxSlider.value = _volume;
    }
}
