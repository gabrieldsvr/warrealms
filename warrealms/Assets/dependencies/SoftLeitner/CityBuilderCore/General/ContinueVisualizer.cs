using UnityEngine;
using UnityEngine.SceneManagement;

namespace CityBuilderCore
{
    /// <summary>
    /// checks whether the game can be continued<br/>
    /// mission and difficulty can be shown in a text<br/>
    /// whether continuing is possible is fired as an event
    /// </summary>
    public class ContinueVisualizer : MonoBehaviour
    {
        [Tooltip("optional, can be used to show the mission and difficulty when continuing is possible")]
        public TMPro.TMP_Text Text;
        [Tooltip("fires on start, parameter is whether continuing is possible, use to enable/disable buttons or show/hide ui elements")]
        public BoolEvent Checked;

        private SaveHelper.ContinueData _continueData;

        private void Start()
        {
            Checked?.Invoke(check());
        }

        public void Continue()
        {
            if (check())
            {
                var mission = _continueData.GetMission();
                var difficulty = _continueData.GetDifficulty();
                var name = _continueData.Name;

                SceneManager.LoadSceneAsync(_continueData.GetMission().SceneName).completed += o =>
                {
                    Dependencies.Get<IMissionManager>().SetMissionParameters(new MissionParameters() { Mission = mission, Difficulty = difficulty, IsContinue = true, ContinueName = name });
                };
            }
            else
            {
                Checked?.Invoke(false);//continue no longer valid, something changed after Start
            }
        }

        private bool check()
        {
            var data = SaveHelper.GetContinue();
            if (data == null || !data.CheckSave())
            {
                _continueData = null;

                if (Text)
                    Text.text = string.Empty;

                return false;
            }
            else
            {
                _continueData = data;

                var mission = _continueData.GetMission();
                var difficulty = _continueData.GetDifficulty();
                if (Text)
                    Text.text = mission.Name + (difficulty == null ? string.Empty : difficulty.Name);

                return true;
            }
        }
    }
}