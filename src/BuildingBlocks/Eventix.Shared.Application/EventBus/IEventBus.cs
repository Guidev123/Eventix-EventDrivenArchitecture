namespace Eventix.Shared.Application.EventBus;

/// <summary>
/// Represents a generic messaging service interface for publishing, subscribing, requesting, and responding to integration events.
/// </summary>
public interface IEventBus : IDisposable
{
    /// <summary>
    /// Gets a value indicating whether the connection to the message broker is currently active.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Publishes the specified integration event to the message bus.
    /// </summary>
    /// <typeparam name="T">The type of the integration event to publish. Must derive from <see cref="IntegrationEvent"/>.</typeparam>
    /// <param name="integrationEvent">The integration event instance to be published. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;

    /// <summary>
    /// Subscribes to an integration event with the specified subscription ID and a callback function to handle messages of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the integration event to subscribe to. Must derive from <see cref="IntegrationEvent"/>.</typeparam>
    /// <param name="subscriptionId">The identifier for the subscription on the message bus.</param>
    /// <param name="onMessage">The method that handles the received integration event.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;
}