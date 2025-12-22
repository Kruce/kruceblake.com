using System.Reflection;
using System.Runtime.Versioning;

namespace KruceBlake.Web.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            var attr = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(TargetFrameworkAttribute), false)
                .SingleOrDefault() as TargetFrameworkAttribute;

            CurrentFramework = string.IsNullOrEmpty(attr?.FrameworkName) ? ".NET" : $"{attr?.FrameworkName} ({attr?.FrameworkDisplayName})";
        }
        public string CurrentFramework { get; set; }
    }
}
