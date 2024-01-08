using CityBuilderCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// tasks are behaviours that contain a series of action for a walker to complete<br/>
    /// this can be as simple as an item pickup or more complex like a farming field that changes throughout the years<br/>
    /// they are either managed(creation, persistence, termination) globally by the <see cref="TownManager"/> or locally by, for example, a building component<br/>
    /// when they are not otherwise occupied <see cref="TownWalker"/>s query the <see cref="TownManager"/> for the most appropriate task which depends on their position and job
    /// </summary>
    public abstract class TownTask : KeyedBehaviour
    {
        [Tooltip("when assigned only walkers that have this job can perform the task, otherwise all walkers can")]
        public TownJob Job;
        [Tooltip("icon displayed in ui, for example the action view")]
        public Sprite Icon;
        [Tooltip("will be rotated")]
        public Transform Visual;
        [Tooltip("walkers instantiate this in their hand when they execute this job")]
        public GameObject Tool;

        public Guid Id { get; private set; } = Guid.NewGuid();

        public virtual IEnumerable<TownWalker> Walkers => Enumerable.Empty<TownWalker>();

        public Vector2Int Point => Dependencies.Get<IGridPositions>().GetGridPosition(transform.position);

        protected bool _isTerminated;
        protected bool _isSuspended;

        protected virtual void Awake() => TownManager.Instance.RegisterTask(this);
        protected virtual void Update()
        {
            if (Visual)
                Visual.Rotate(Vector3.up, Time.deltaTime * 100f);
        }
        protected virtual void OnDestroy() => TownManager.Instance.DeregisterTask(this);

        public virtual bool CanStartTask(TownWalker walker) => !_isTerminated && !_isSuspended;
        public abstract WalkerAction[] StartTask(TownWalker walker);
        public abstract void ContinueTask(TownWalker walker);
        public abstract void FinishTask(TownWalker walker, ProcessState process);
        public virtual void Terminate()
        {
            if (_isTerminated)
                return;
            _isTerminated = true;
            TownManager.Instance.DeregisterTask(this);
            Walkers.ToList().ForEach(w => w.CancelProcess());
            Destroy(gameObject);
        }
        public virtual void SignalTask(TownWalker walker, string key) { }
        public virtual bool ProgressTask(TownWalker walker, string key) => false;

        public virtual void SuspendTask()
        {
            _isSuspended = true;
            TownManager.Instance.DeregisterTask(this);
            Walkers.ToList().ForEach(w => w.CancelProcess());
            if (Visual)
                Visual.gameObject.SetActive(false);
        }
        public virtual void ResumeTask()
        {
            _isSuspended = false;
            TownManager.Instance.RegisterTask(this);
            if (Visual)
                Visual.gameObject.SetActive(true);
        }

        public virtual string GetDescription() => name;
        public virtual string GetDebugText() => null;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            try
            {
                string debugText = GetDebugText();
                if (string.IsNullOrWhiteSpace(debugText))
                    return;

                UnityEditor.Handles.Label(transform.position, debugText);
            }
            catch
            {
                //dont care
            }
        }
#endif

        #region Saving
        public TownTaskData SaveData()
        {
            return new TownTaskData()
            {
                Id = Id.ToString(),
                Key = Key,
                Point = Point,
                Extras = saveExtras()
            };
        }
        public void LoadData(TownTaskData data)
        {
            Id = new Guid(data.Id);

            loadExtras(data.Extras);
        }

        protected virtual string saveExtras() => null;
        protected virtual void loadExtras(string json) { }
        #endregion
    }
}
