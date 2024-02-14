using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioSoundsAndMusic : MonoBehaviour
{
    public AudioSource musicCanal;
    public AudioSource soundCanal1;
    public AudioSource soundCanal2;
    public AudioSource soundCanal3;

    //MUSIC CANAL
    public AudioClip GameOverMusic;
    public AudioClip MarioDiesMusic;
    public AudioClip OutOfTimeMusic;
    public AudioClip StageClearMusic;
    public AudioClip WorldClearMusic;

    public AudioClip OverworldMusic;
    public AudioClip HuriedOverworldMusic;
    public AudioClip UndergroundMusic;
    public AudioClip HuriedUndergroundMusic;
    public AudioClip UnderwaterMusic;
    public AudioClip HuriedUnderwaterMusic;

    //SOUND CANAL 1
    public AudioClip JumpSmallSound;
    public AudioClip JumpBigSound;
    public AudioClip PipeSound;

    //SOUND CANAL 2
    public AudioClip BumpSound;
    public AudioClip BrickSmashSound;

    //SOUND CANAL 3
    public AudioClip HpUpSound;
    public AudioClip KickSound;
    public AudioClip PauseSound;
    public AudioClip PowerUpSound;
    public AudioClip CoinSound;

    public enum MusicNames { GameOverMusic, MarioDiesMusic, OutOfTimeMusic, StageClearMusic, WorldClearMusic, Overworld, HuriedOverworld, Underground, HuriedUnderground, Underwater, HuriedUnderwater };
    public enum SoundNames {
        HpUpSound,
        JumpSmallSound,
        JumpBigSound,
        KickSound,
        PauseSound,
        PowerUpSound,
        BumpSound,
        BrickSmashSound,
        CoinSound,
        PipeSound
    };


    public void playMusic(MusicNames music)
	{
		switch (music)
		{
            case MusicNames.GameOverMusic:
                musicCanal.clip = GameOverMusic;
                musicCanal.loop = false;
                break;

            case MusicNames.MarioDiesMusic:
                musicCanal.clip = MarioDiesMusic;
                musicCanal.loop = false;
                break;

            case MusicNames.OutOfTimeMusic:
                musicCanal.clip = OutOfTimeMusic;
                musicCanal.loop = true;
                break;

            case MusicNames.StageClearMusic:
                musicCanal.clip = StageClearMusic;
                musicCanal.loop = false;
                break;

            case MusicNames.WorldClearMusic:
                musicCanal.clip = WorldClearMusic;
                musicCanal.loop = false;
                break;

            // ----

            case MusicNames.Overworld:
                musicCanal.clip = OverworldMusic;
                musicCanal.loop = true;
                break;

            case MusicNames.HuriedOverworld:
                musicCanal.clip = HuriedOverworldMusic;
                musicCanal.loop = true;
                break;

            case MusicNames.Underground:
                musicCanal.clip = UndergroundMusic;
                musicCanal.loop = true;
                break;

            case MusicNames.HuriedUnderground:
                musicCanal.clip = HuriedUndergroundMusic;
                musicCanal.loop = true;
                break;

            case MusicNames.Underwater:
                musicCanal.clip = UnderwaterMusic;
                musicCanal.loop = true;
                break;

            case MusicNames.HuriedUnderwater:
                musicCanal.clip = HuriedUnderwaterMusic;
                musicCanal.loop = true;
                break;

        }

        
        musicCanal.Play();
        
	}


    public void playSound(SoundNames sound)
    {
        switch (sound)
        {
            // canal 1
            case SoundNames.JumpSmallSound:
                soundCanal1.clip = JumpSmallSound;
                soundCanal1.Play();
                break;

            case SoundNames.JumpBigSound:
                soundCanal1.clip = JumpBigSound;
                soundCanal1.Play();
                break;

            case SoundNames.PipeSound:
                soundCanal1.clip = PipeSound;
                soundCanal1.Play();
                break;





            // canal 2
            case SoundNames.BumpSound:
                soundCanal2.clip = BumpSound;
                soundCanal2.Play();
                break;

            case SoundNames.BrickSmashSound:
                soundCanal2.clip = BrickSmashSound;
                soundCanal2.Play();
                break;




            // canal 3
            case SoundNames.HpUpSound:
                soundCanal3.clip = HpUpSound;
                soundCanal3.Play();
                break;

            case SoundNames.KickSound:
                soundCanal3.clip = KickSound;
                soundCanal3.Play();
                break;

            case SoundNames.PauseSound:
                soundCanal3.clip = PauseSound;
                soundCanal3.Play();
                break;

            case SoundNames.PowerUpSound:
                soundCanal3.clip = PowerUpSound;
                soundCanal3.Play();
                break;

            case SoundNames.CoinSound:
                soundCanal3.clip = CoinSound;
                soundCanal3.Play();
                break;
        }


    }
}
