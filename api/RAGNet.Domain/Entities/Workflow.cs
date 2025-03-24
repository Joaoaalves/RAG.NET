namespace RAGNET.Domain.Entities
{
    public class Workflow
    {
        private string Id { get; set; } = String.Empty;
        private string Name { get; set; } = String.Empty;
        private DateTime CreatedAt { get; set; }

        private DateTime UpdatetAt { get; set; }

        /* Relations
            User
            Chunkers
            Query Enhancers
            Filters
            Rankers
        */
    }
}