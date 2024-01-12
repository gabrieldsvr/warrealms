using CityBuilderCore;
using TMPro;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// ui behaviour that lets players set number of walkers for the specified job
    /// </summary>
    public class TownJobInput : TooltipOwnerBase
    {
        [Tooltip("numeric input field for number of walkers")]
        public TMP_InputField Input;
        [Tooltip("the job which specified number of walkers should have")]
        public TownJob Job;

        public override string TooltipName => Job.Name;
        public override string TooltipDescription => Job.Description;

        private void Start()
        {
            if (Input.text != "0")
                textChanged(Input.text);
            Input.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<string>(textChanged));
        }

        public void SetText()
        {
            Input.SetTextWithoutNotify(TownManager.Instance.GetJobCount(Job).ToString());
        }

        public void Change(int delta)
        {
            if (!int.TryParse(Input.text, out int num))
                return;
            set(num + delta);
        }

        private void textChanged(string text)
        {
            if (!int.TryParse(text, out int num))
                return;
            set(num);
        }

        private void set(int num)
        {
            var neutralCount = TownManager.Instance.GetJobCount(null);
            var currentCount = TownManager.Instance.GetJobCount(Job);

            num = Mathf.Min(num, currentCount + neutralCount);
            num = Mathf.Max(num, 0);

            TownManager.Instance.SetJobCount(Job, num);

            Input.SetTextWithoutNotify(num.ToString());
        }
    }
}
