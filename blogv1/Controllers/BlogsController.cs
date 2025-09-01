using blogv1.Context;
using blogv1.Models;
using Microsoft.AspNetCore.Mvc;

namespace blogv1.Controllers
{
    public class BlogsController : Controller
    {
        private readonly BlogDbContext _context;

        public BlogsController(BlogDbContext context)
        {
            _context = context;
        }//verıtabanı baglantısını yapıyor

        public IActionResult Index()//blogsun ıcındekı ındexe lıst sekllınde ındex gonderme
        {
            // var blogs = _context.Blogs.ToList();
            var blogs = _context.Blogs.Where(x => x.Status == 1).ToList();

            return View(blogs);
        }
        public IActionResult Details(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();//blogs tablsouna baglandık nerede dedık. benzersız zaten ıd ama fırs orderdefault lkullanıoz
            //first or default uygun olanı veya default olanı dondurur

            blog.ViewCount += 1;
            _context.SaveChanges(); //view sayısını gunceller

            var comment = _context.Comments.Where(x => x.BlogId == id).ToList();
            ViewBag.Comments = comment.ToList();
            //bu da bızım commentı dondurmek ıcın mekanızmamız
            return View(blog);
        }
        [HttpPost]
        public IActionResult CreateComment(Comment model) { 
            model.PublishDate = DateTime.Now;
            _context.Comments.Add(model);

            var blog = _context.Blogs.Where(x => x.Id == model.BlogId).FirstOrDefault();
            //veritabanına git o blogu bul gel
            blog.CommentCount += 1;

            _context.SaveChanges();
            return RedirectToAction("Details", new {id = model.BlogId});
            //tekrar detaılsa verıyor yenıleyıp glb
        
        }  

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        //bize bir şey gondermesı lazım contactın

        public IActionResult CreateContact(Contact model) { 
           
            model.CreaatedAt = DateTime.Now;//olusturma tarıhı burda
            //once verıtabanına baglayalım
            _context.Contacts.Add(model);   
            _context.SaveChanges(); //kayıt ettık 

            return RedirectToAction("Index");
        }

        public IActionResult Support()
        {
            return View();
        }




    }
}
