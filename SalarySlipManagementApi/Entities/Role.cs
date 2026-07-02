namespace SalarySlipManagementApi.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
