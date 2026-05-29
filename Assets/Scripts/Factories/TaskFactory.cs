using character.Interfaces;
using Manages;

namespace Factories
{
    public static class TaskFactory
    {
        public static ITask CreateTask(TaskType type)
        {
            return type switch
            {
                TaskType.DestroyData => new DestroyDataTask(),
                TaskType.KillTarget => new KillTargetTask(),
                TaskType.CollectData => new CollectDataTask(),
                _ => throw new System.NotImplementedException()
            };
        }
    }
}