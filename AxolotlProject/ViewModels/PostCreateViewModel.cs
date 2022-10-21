using AxolotlProject.Models;

namespace AxolotlProject.ViewModels
{
    public class PostCreateViewModel
    {
        public IEnumerable<PostCategory> PostCategories { get; set; }
        
        public PostCreateViewModel()
        {
            PostCategories = new List<PostCategory>();
            
        }
    }
}
