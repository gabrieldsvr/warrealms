using CityBuilderCore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CityBuilderTown
{
    /// <summary>
    /// used to continue the last saved game, disabled itself if there is no game to continue
    /// </summary>
    public class TownMenuContinue : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(SaveHelper.GetContinue()?.CheckSave() ?? false);
        }

        public void Continue()
        {
            var data = SaveHelper.GetContinue();
            var mission = data.GetMission();
            var difficulty = data.GetDifficulty();

            SceneManager.LoadSceneAsync(mission.SceneName).completed += o =>
            {
                TownManager.Instance.Fader.DelayedFadeIn();
                Dependencies.Get<IMissionManager>().SetMissionParameters(new MissionParameters()
                {
                    Mission = mission,
                    Difficulty = difficulty,
                    IsContinue = true,
                    ContinueName = data.Name
                });
            };
        }
    }
}
