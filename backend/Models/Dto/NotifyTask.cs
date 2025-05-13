using TaskSync.Enums;

namespace TaskSync.Models.Dto
{
    public class NotifyTask
    {
        public NOTIFY_STATUS Status { get; }
        public TaskDto Data { get; }

        public NotifyTask(NOTIFY_STATUS status, TaskDto data)
        {
            Status = status;
            Data = data;
        }
    }
}
