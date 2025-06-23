using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Workflows.Events
{
    public class WorkflowCreatedEvent : DomainEventBase
    {
        public WorkflowId WorkflowId { get; }

        public WorkflowCreatedEvent(WorkflowId id)
        {
            Console.WriteLine("Workflow Created!");
            WorkflowId = id;
        }
    }
}