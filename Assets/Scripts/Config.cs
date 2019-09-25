using System;
using System.IO;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Action OnSkinChange = delegate { };

    static string SavePath;

    public static Config Instance;

    static Configuration Cfg;
    public static int SelectedSkin
    {
        get => Cfg.SelectedSkin;
        set
        {
            Cfg.SelectedSkin = value;
            Save();

            OnSkinChange();

            TopImage.Instance.texture = BackgroundMaker.GetTexture(value);
            BottomLine.Instance.texture = BackgroundMaker.GetBottomTexture(value);
        }
    }
    public static Language Language
    {
        get => Cfg.Language;
        set
        {
            Cfg.Language = value;
            Save();

            TranslatedText.SetLanguage(value);
        }
    }

    [SerializeField]
    public Texture2D Atlas = null;

    void Awake()
    {
        Instance = this;
        SavePath = Path.Combine(Application.persistentDataPath, "config.cfg");

        try
        {
            Cfg = File.Exists(SavePath) ?
                JsonUtility.FromJson<Configuration>(File.ReadAllText(SavePath)) :
                CreateNew();
        }
        catch
        {
            Cfg = CreateNew();
        }
    }

    static Configuration CreateNew() => new Configuration()
    {
        Language = Application.systemLanguage == SystemLanguage.English ?
            Language.English :
            Language.Russian
    };

    static void Save() => File.WriteAllText(SavePath, JsonUtility.ToJson(Cfg));

    void OnApplicationQuit() => Save();


    class Configuration
    {
        public int SelectedSkin = 1;
        public Language Language = Language.English;
    }
}