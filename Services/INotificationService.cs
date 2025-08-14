namespace PoemGenerator.Monolith.Notifications;

public interface INotificationService
{
    Task NotifyPoemAddedAsync(int id, string title, string author);
}
