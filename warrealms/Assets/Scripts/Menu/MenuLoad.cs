using CityBuilderCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CityBuilderTown
{
    /// <summary>
    /// displays the created save games per mission and difficulty<br/>
    /// save games can be selected and then loaded or deleted
    /// </summary>
    public class MenuLoad : MonoBehaviour
    {
        [Tooltip("dropdown that is used for the mission, options and events are hooked up by the behaviour")]
        public TMPro.TMP_Dropdown MissionDropdown;
        [Tooltip("dropdown that is used for the difficulty, options and events are hooked up by the behaviour")]
        public TMPro.TMP_Dropdown DifficultyDropdown;
        [Tooltip("parent transform for the save games")]
        public Transform Content;
        [Tooltip("prefab used for the save games")]
        public SaveGame SaveGamePrefab;
        [Tooltip("button gets disabled when no deletable save is selected")]
        public Button DeleteButton;
        [Tooltip("button gets disabled when no save is selected")]
        public Button LoadButton;

        private List<SaveGame> _saveGames;
        private SaveGame _selectedSaveGame;
        private Mission _mission;
        private Difficulty _difficulty;

        private void Start()
        {
            var missions = Dependencies.Get<IKeyedSet<Mission>>();

            _mission = missions.Objects[0];

            MissionDropdown.ClearOptions();
            MissionDropdown.AddOptions(missions.Objects.Select(o => new TMPro.TMP_Dropdown.OptionData(o.Name)).ToList());
            MissionDropdown.onValueChanged.AddListener(i =>
            {
                _mission = missions.Objects[i];
                loadSaves();
            });

            var difficulties = Dependencies.Get<IKeyedSet<Difficulty>>();

            _difficulty = difficulties.Objects[0];

            DifficultyDropdown.ClearOptions();
            DifficultyDropdown.AddOptions(difficulties.Objects.Select(o => new TMPro.TMP_Dropdown.OptionData(o.Name)).ToList());
            DifficultyDropdown.onValueChanged.AddListener(i =>
            {
                _difficulty = difficulties.Objects[i];
                loadSaves();
            });

            loadSaves();
        }

        private void OnEnable()
        {
            if (_mission != null)
                loadSaves();
        }

        private void OnDisable()
        {
            _saveGames.ForEach(b => Destroy(b.gameObject));
            _saveGames = null;
            _selectedSaveGame = null;
        }

        public void Delete()
        {
            _mission.Delete(_selectedSaveGame.SaveName, _difficulty);

            loadSaves();
        }

        public void Load()
        {
            var save = _selectedSaveGame.SaveName;

            SceneManager.LoadSceneAsync(_mission.SceneName).completed += o =>
            {
                TownManager.Instance.Fader.DelayedFadeIn();
                Dependencies.Get<IMissionManager>().SetMissionParameters(new MissionParameters()
                {
                    Mission = _mission,
                    Difficulty = _difficulty,
                    IsContinue = true,
                    ContinueName = save
                });
            };
        }

        private void loadSaves()
        {
            _selectedSaveGame = null;

            if (_saveGames == null)
            {
                _saveGames = new List<SaveGame>();
            }
            else
            {
                _saveGames.ForEach(b => Destroy(b.gameObject));
                _saveGames.Clear();
            }

            var saves = _mission.GetSaves(_difficulty);

            if (saves != null)
            {
                foreach (var save in saves)
                {
                    var saveGame = Instantiate(SaveGamePrefab, Content);

                    saveGame.Initialize(_mission, _difficulty, save);
                    saveGame.Button.onClick.AddListener(new UnityAction(() => select(saveGame)));

                    _saveGames.Add(saveGame);
                }
            }

            select(_saveGames.FirstOrDefault());
        }

        private void select(SaveGame saveGame)
        {
            _selectedSaveGame?.Deselect();
            _selectedSaveGame = saveGame;
            _selectedSaveGame?.Select();

            DeleteButton.interactable = _selectedSaveGame != null && !string.IsNullOrWhiteSpace(_selectedSaveGame.SaveName);
            LoadButton.interactable = _selectedSaveGame != null;
        }
    }
}
