using Microsoft.AspNetCore.Identity;

namespace blogv1.Identity
{
    public class BlogIdentityUser : IdentityUser //kırmızı alt olunca ctrl. using microsoft
    {

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
