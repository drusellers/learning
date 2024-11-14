namespace SampleSagas;

using MassTransit;

public class CustomerCache
{
    public Guid Id { get; set; }

}

public class CustomerCacheSyncState:
    SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public DateTime? LastSyncedAt { get; set; }

}

/// <summary>
/// Initial State
/// </summary>
public class SyncSaga:
    MassTransitStateMachine<CustomerCacheSyncState>
{
    // idle
    // create
    // pending
    public State Discover { get; set; } = null!;
    public State Researching { get; set; }= null!;
    public State NeedsToBeCreated { get; set; } = null;

    public Event ClientCreate { get; set; }= null!;

    // ErpUpdate(correlation id, Missing | Found, ERP Data)
    //
    public SyncSaga()
    {
        InstanceState(x => x.CurrentState);

        // When a client requests us to create a customer
        // step 1: check that they don't already exist
        Initially(
            // on any event
            When(ClientCreate)
                // Publish (Send Command) .. inspect ERPs
                .TransitionTo(Researching));


    }
}
