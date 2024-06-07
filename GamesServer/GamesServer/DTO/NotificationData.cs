using GamesServer.Enums;

namespace GamesServer.DTO
{
    [Serializable]
    public class NotificationData
    {
        public NotificationType Type { get; set; }
        public object Args {  get; set; }

        public NotificationData(NotificationType type, object args)
        {
            Type = type;
            Args = args;
        }
    }
}
