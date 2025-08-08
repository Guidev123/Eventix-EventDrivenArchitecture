using Eventix.Shared.Infrastructure.EventBus;

namespace Eventix.Shared.Application.EventBus;

/// <summary>
/// Represents a generic messaging service interface for publishing and subscribing integration events.
/// </summary>
public interface IBus : IDisposable
{
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
    /// Publishes the specified integration event to the message bus with a specific exchange type.
    /// </summary>
    /// <typeparam name="T">The type of the integration event to publish. Must derive from <see cref="IntegrationEvent"/>.</typeparam>
    /// <param name="integrationEvent">The integration event instance to be published. Cannot be <see langword="null"/>.</param>
    /// <param name="exchangeType">The type of exchange.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishAsync<T>(T integrationEvent, ExchangeTypeEnum exchangeType, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;

    /// <summary>
    /// Subscribes to an integration event with the specified subscription ID and a callback function to handle messages of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the integration event to subscribe to. Must derive from <see cref="IntegrationEvent"/>.</typeparam>
    /// <param name="queueName">The name for the queue on the message bus.</param>
    /// <param name="onMessage">The method that handles the received integration event.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SubscribeAsync<T>(string queueName, Func<T, Task> onMessage, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;

    /// <summary>
    /// Subscribes to an integration event with the specified subscription ID and a callback function to handle messages of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the integration event to subscribe to. Must derive from <see cref="IntegrationEvent"/>.</typeparam>
    /// <param name="queueName">The name for the queue on the message bus.</param>
    /// <param name="onMessage">The method that handles the received integration event.</param>
    /// <param name="exchangeType"> The type of exchange.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SubscribeAsync<T>(string queueName, Func<T, Task> onMessage, ExchangeTypeEnum exchangeType, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;
}