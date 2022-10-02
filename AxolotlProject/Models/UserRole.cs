namespace AxolotlProject.Models
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //constructor
        public UserRole(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
