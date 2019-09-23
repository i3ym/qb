using System;

public class SettingsLanguageMenu : Menu
{
    protected override Type PreviousMenuType => typeof(SettingsMenu);

    protected override void Init()
    {
        Buttons[0].OnSelect += () => TranslatedText.SetLanguage(Language.Russian);
        Buttons[1].OnSelect += () => TranslatedText.SetLanguage(Language.English);

        Buttons[0].OnClick += () => SetMenu(PreviousMenuType);
        Buttons[1].OnClick += () => SetMenu(PreviousMenuType);
    }
}