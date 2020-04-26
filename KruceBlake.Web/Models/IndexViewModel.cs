using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace KruceBlake.Web.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            CurrentFramework = ((TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(TargetFrameworkAttribute), false).SingleOrDefault()).FrameworkName;
        }
        public string CurrentFramework { get; set; }
    }
}
