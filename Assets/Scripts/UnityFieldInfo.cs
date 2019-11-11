using System;
using qb.Shared;
using UnityEngine;

namespace qb.Unity
{
    public class UnityFieldInfo : QBehaviour, IFieldInfo
    {
        [SerializeField] TranslatedText LevelText = null, LinesText = null, ScoreText = null, TimeText = null;

        public void UpdateLevelText(int value) => LevelText.Text = value.ToString();
        public void UpdateLinesText(int value) => LinesText.Text = value.ToString();
        public void UpdateScoreText(int value) => ScoreText.Text = value.ToString();
        public void UpdateTimeText(TimeSpan value) => TimeText.Text = value.ToString();
    }
}