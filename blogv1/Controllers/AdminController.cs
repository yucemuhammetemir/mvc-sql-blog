using System.Threading.Tasks;
using blogv1.Context;
using blogv1.Identity;
using blogv1.Models;
using blogv1.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace blogv1.Controllers
{
    [Authorize]//admıne artık gırıs oyle kolay olmucak
  //logoutu yazınca bunu actık tekrar
    public class AdminController : Controller
    {
        private readonly BlogDbContext _context;
        //userları ımplamente etme ıcın
        private readonly UserManager<BlogIdentityUser> _userManager;//basksa bır taban baglantısı ıcın yani
        //alttakı de gırıs tutacak
        private readonly SignInManager<BlogIdentityUser> _signInManager;
        public AdminController(BlogDbContext context, UserManager<BlogIdentityUser> userManager, SignInManager<BlogIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;//ctrl . ile add parameters dedık
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var dashboard = new DashboardViewModel();

            var toplamblogsayisi = _context.Blogs.Count();
            var toplamgoruntulenme = _context.Blogs.Select(x => x.ViewCount).Sum();
            //var encokgoruntulneneblog = _context.Blogs.OrderByDescending(x => x.ViewCount).FirstOrDefault();
           // var ensonyayinlananblog = _context.Blogs.OrderByDescending(x => x.PublishDate).FirstOrDefault();
          //  var toplamyorumsayisi = _context.Comments.Count();
           // var encokyorumalanblogId = _context.Comments
                                     //   .GroupBy(x => x.BlogId) // BlogId'ye göre grupla
                                    //    .OrderByDescending(g => g.Count()) // Grupları yorum sayısına göre azalan sırala
                                      //  .Select(g => g.Key) // En çok yorumu olan BlogId'yi al
                                     //   .FirstOrDefault(); // İlk sonucu getir
            //var encokyorumalanblog = _context.Blogs.Where(x => x.Id == encokyorumalanblogId).FirstOrDefault();

            var bugunyapilanyorumsayisi = _context.Comments.Where(x => x.PublishDate.Date == DateTime.Now.Date).Count();

            dashboard.TotalBlogCount = toplamblogsayisi;
            dashboard.TotalViewCount = toplamgoruntulenme;
           // dashboard.MostViewedBlog = encokgoruntulneneblog;
          //  dashboard.LatestBlog = ensonyayinlananblog;
           // dashboard.TotalCommentCount = toplamyorumsayisi;
           // dashboard.MostCommentedBlog = encokyorumalanblog;
            dashboard.TodayCommentCount = bugunyapilanyorumsayisi;





            return View(dashboard);
        }

        public IActionResult Blogs()
        {
            var blogs = _context.Blogs.ToList();
            //sorguya gerek yok tum blogları gosterıcem
            
            return View(blogs);
        }
        
        public IActionResult EditBlog(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            return View(blog);
        }

        public IActionResult DeleteBlog(int id) //once bir id alıcaz
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            //sen git sec sectıkten sonra sen gıt verıtabanına baglan
            _context.Blogs.Remove(blog);//sil
            _context.SaveChanges(); //kaydet
            //return View();
            return RedirectToAction("Blogs"); //sildikten sonra admındekı blogsa gıtsın tekrar
        }
        //edit post
        //verıtabaına kaydetme yapıcaz sımdı edıtte bunda bır post ıslemı olucak ondan
        [HttpPost]
        public IActionResult EditBlog(Blog model)//ben buraya blog turunde bır model gonderıcem tanımayınca blog yazısını ctrl .
        {
            var blog = _context.Blogs.Where(x => x.Id == model.Id).FirstOrDefault();//blogu bul
            //artık hem verıtabanında bır blogum var hem de modelde bır blogum var
            blog.Name = model.Name;
            blog.Descripton = model.Descripton;
            blog.Tags = model.Tags;
            blog.ImageUrl = model.ImageUrl;
            _context.SaveChanges(); //en son kaydedıom
            return RedirectToAction("Blogs");
            //return View()/*;*/
        }
        //status toggle ac kapa
        public IActionResult ToggleStatus(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();//blogu bul
            if(blog.Status == 1)
            {
                blog.Status = 0;
            }
            else
            {
                blog.Status = 1;
            }
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }

        public IActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBlog(Blog model)
        {
            model.PublishDate = DateTime.Now;
            model.Status = 1;
            _context.Blogs.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }
        public IActionResult Comments(int? blogId)
        {
            var comments = new List<Comment>();//hpsterıyor nereden geldıgını bu sefer 0 degılse
            if(blogId == null)
            {
                 comments = _context.Comments.ToList();
            }
            else
            {
                 comments = _context.Comments.Where(x => x.BlogId == blogId).ToList();
            }

                return View(comments);
        }
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comments.Where(x => x.Id == id).FirstOrDefault();
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Comments");
        }

        public IActionResult Register() { 
        
            return View();
        }

        //bir kayıt ıslemı yapıcaz yanı post ve regıstervıewmodel turunde bır model alıcam kullanıcıdan

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //burada sifre olusturuken buyuk harf kucuk harf sayi 6 karekter ve simge gerekli buna dıkkat etmeli simge de gerekiyormus
            if (model.Password == model.RePassword)
            {
                var user = new BlogIdentityUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Blogs"); //once actıon sonra route
        }

        public IActionResult Contact()
        {
            var contact = _context.Contacts.ToList();
            return View(contact);
        }


    }
}
