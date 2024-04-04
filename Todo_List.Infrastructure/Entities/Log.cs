namespace Todo_List.Infrastructure.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public string LogName { get; set; } = default!;
        public string? LogDescription { get; set; }
        public DateTime LogCreationDate { get; set; }
        public string LogRelatedObjectsSerialized { get; set; } = default!;

    }
}
