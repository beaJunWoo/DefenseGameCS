using UnityEngine;
using UnityEngine.UI;

public class SR_SoundManager : MonoBehaviour
{
    public bool bStartBgm;
    public static SR_SoundManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    
    [Header("#Background")]
    public AudioClip BackGroundClip;
    public float BackGroundVolume;
    AudioSource BackGroundPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;

    [Header("#BtnSFX")]
    public AudioClip[] btnSfxClips;
    public float btnSfxVolume;
    public int btnSfxchannels;

    public Slider BgmSlider;
    public Slider SfxSlider;
    public enum Sfx 
    {   GunFire,//0
        RifleFire,//1
        SniperFire,//2
        TankFire,//3
        MinigunFire,//4
        Dron,//5
        Bomb, //6
        EnemyBomb,//7
        Die,//8
        Attaked1,//9
        Attaked2,//10
        Attacked1,//11
        CountDown,//12
        Placement1, //13
        Placement2, //14
        Placement3, //15
        Placement4, //16
        Buying, //17
        SuccessGame, //18
        CarDestoy, //19

    }
    public enum BtnSfx
    {
        Click,//0
        Open,//1
        StageOpen,//2
        Warning,//3
        StartGame, //4
        StartDefence, //5
        Apper, //6
        Error,//7
        StageClose,//8
        SelectStore,//9
        Buy,//10

    }
    private void Awake()
    {
        instance = this;
        Init();
        if(bStartBgm)
        {
            PlayBgm();
        }
        
    }
    private void Start()
    {
        if(BgmSlider != null)
            BgmSlider.value = SR_GameManager.instance.bgmVolume;
        else
            bgmPlayer.volume = SR_GameManager.instance.bgmVolume;

        if (SfxSlider != null)
            SfxSlider.value = SR_GameManager.instance.sfxVolume;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayBtnSfx(BtnSfx.Click);
        }
        if (BgmSlider != null)
        {
            bgmPlayer.volume = BgmSlider.value+BgmSlider.value*bgmVolume;
            BackGroundPlayer.volume = BgmSlider.value+ BgmSlider.value*bgmVolume;
            SR_GameManager.instance.bgmVolume = BgmSlider.value;
        }
        if(SfxSlider != null)
        {
            btnSfxVolume = SfxSlider.value;
            sfxVolume = SfxSlider.value;
            SR_GameManager.instance.sfxVolume = SfxSlider.value;
        }
           
    }
    void Init()
    {
        GameObject BackGroundObject = new GameObject("BackGroundPlayer");
        BackGroundObject.transform.parent = transform;
        BackGroundPlayer = BackGroundObject.AddComponent<AudioSource>();
        BackGroundPlayer.playOnAwake = false;
        BackGroundPlayer.loop = true;
        BackGroundPlayer.volume = BackGroundVolume;
        BackGroundPlayer.clip = BackGroundClip;

        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index=0; index<channels; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
        PlayBackGround();
    }

    public void PlaySfx(Sfx sfx)
    {
        PlaySfx(0.0f,sfx, 0.0f);
    }
   
    public void PlaySfx(Sfx sfx, float time)
    {
        PlaySfx(0.0f, sfx,time);
    }
    [VisibleEnum(typeof(Sfx))]
    public void PlaySfx(int idx)
    {
        PlaySfx((Sfx)idx);
    }
    public void PlaySfx(float volume,Sfx sfx)
    {
        PlaySfx(volume,sfx, 0.0f);
    }
    public void PlaySfx(float volume, Sfx sfx, float time)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIdx = index% sfxPlayers.Length;
            if (sfxPlayers[loopIdx].isPlaying)
            {
                bool isOverPlay = (sfxPlayers[loopIdx].time / sfxPlayers[loopIdx].clip.length) >= 0.65f;
                if (!isOverPlay)
                    continue;
            }

            if (Sfx.GunFire <= sfx&& sfx<= Sfx.MinigunFire)
            {
                sfxPlayers[loopIdx].volume = sfxVolume - sfxVolume * 0.6f;
            }
            else
            {
                sfxPlayers[loopIdx].volume = sfxVolume + sfxVolume * volume;
            }
            sfxPlayers[loopIdx].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIdx].time = time;
            sfxPlayers[loopIdx].Play();
            break;
        }
    }
    [VisibleEnum(typeof(BtnSfx))]
    public void PlayBtnSfx(int index)
    {
        PlayBtnSfx(0.0f, (BtnSfx)index);
    }
    public void PlayBtnSfx(BtnSfx sfx)
    {
        PlayBtnSfx(0.0f,sfx);
    }
    public void PlayBtnSfx(float volume, BtnSfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIdx = index% sfxPlayers.Length;

           if (sfxPlayers[loopIdx].isPlaying)
               continue;
            sfxPlayers[loopIdx].volume = btnSfxVolume + btnSfxVolume * volume;
            sfxPlayers[loopIdx].clip = btnSfxClips[(int)sfx];
            sfxPlayers[loopIdx].Play();
            break;
        }
    }
    public void PlayBgm()
    {
       //bgmPlayer.volume = bgmVolume;
       bgmPlayer.clip = bgmClip;
       bgmPlayer.Play();
    }
    public void PlayBackGround()
    {
        //BackGroundPlayer.volume = BackGroundVolume;
        BackGroundPlayer.clip = BackGroundClip;
        BackGroundPlayer.Play();
    }
}
