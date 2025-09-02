using blogv1.Context;
using blogv1.Models;
using Microsoft.AspNetCore.Mvc;

namespace blogv1.Controllers
{
    public class AdminController : Controller
    {
        private readonly BlogDbContext _context;

        public AdminController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
