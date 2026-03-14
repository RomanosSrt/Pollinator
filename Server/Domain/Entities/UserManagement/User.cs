namespace API.Domain.Entities.UserManagement
{
    public class User
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public UserType usertype { get; set; }
    }
}
