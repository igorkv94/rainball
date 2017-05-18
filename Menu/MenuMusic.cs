using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuMusic : MonoBehaviour {
	private Data data;
	private bool muteSound = false;
	private bool muteMusic = false;
	private Sprite[] spritesMute;
	private Image soundImg;
	private Image musicImg;
	private AudioSource musicAudio;

	void Start () {
		spritesMute = Resources.LoadAll<Sprite>("mute/");
		data = GameObject.Find("Data").GetComponent<Data>();
		soundImg = GameObject.Find("Sound").GetComponent<Image>();
		musicImg = GameObject.Find("Music").GetComponent<Image>();
		musicAudio = GameObject.Find ("Music").GetComponent<AudioSource> ();
		if (data.getMuteSound())
		{
			muteSound = true;
			updSoundSprite();
		}
		if (data.getMuteMusic())
		{
			muteMusic = true;
			updMusicSprite();
		}
	}

	//Save sound settings and set it 
	public void setSound()
	{
		muteSound = !muteSound;
		data.setMuteSound();
		updSoundSprite();
	}

	//Change the sound sprite
	private void updSoundSprite() { 
		if (muteSound)
		{
			soundImg.sprite = spritesMute[3];
		}
		else {
			soundImg.sprite = spritesMute[2];
		}
	}

	//Save music settings and set it 
	public void setMusic()
	{
		muteMusic = !muteMusic;
		data.setMuteMusic();
		updMusicSprite();
	}

	//Change the music sprite and state
	private void updMusicSprite()
	{
		if (muteMusic)
		{
			musicImg.sprite = spritesMute[1];
			musicAudio.Stop();
		}
		else {
			musicImg.sprite = spritesMute[0];
			musicAudio.Play();
		}
	}
}
