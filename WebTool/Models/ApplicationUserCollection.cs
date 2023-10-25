using System.Collections.Generic;

namespace WebTool
{
    public class ApplicationUserCollection : List<ApplicationUser>
    {
        public ApplicationUserCollection(IEnumerable<ApplicationUser> users) : base(users) { }
    }
}