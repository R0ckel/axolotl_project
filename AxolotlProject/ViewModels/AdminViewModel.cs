using AxolotlProject.Models;

namespace AxolotlProject.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<User> searchedUsers { get; set; }
        public IEnumerable<User> bannedUsers { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
