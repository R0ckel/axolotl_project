namespace AxolotlProject.Models
{
    public class User
    {
        public Guid Id { get; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public bool IsBanned { get; set; }
        //File Icon { get; set; } //need to save user`s icon. File - static class.

        public DateTime BirthDate { get; set; }
        public UserSex Sex { get; set; }

        //constructors
        public User() { }
        public User(string userName)
        {
            UserName = userName;
        }
        public User(Guid id, string login, string passwordHash, string userName,
            string userSurname, string email, string phoneNumber, string status,
            bool isBanned, DateTime birthDate, UserSex userSex)
        {
            Id = id;
            Login = login;
            PasswordHash = passwordHash;
            UserName = userName;
            UserSurname = userSurname;
            Email = email;
            PhoneNumber = phoneNumber;
            IsBanned = isBanned;
            BirthDate = birthDate;
            Sex = userSex;
            Status = status;
        }
    }
}
