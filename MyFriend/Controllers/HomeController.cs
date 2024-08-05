using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFriend.DAL;
using MyFriend.Models;
using System.Diagnostics;

namespace MyFriend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Friends()
        {
            List<Friend> friends = Data.Get.Friends.ToList();
        
            return View(friends);
        }

        // פונקציה שמחזירה דף להוספת חבר חדש
        public IActionResult Create()
        {
            return View(new Friend());
        }

        [HttpPost, ValidateAntiForgeryToken]
        // פונקציה שמקבלת עובד חדש ומכינסה לדאטה בייס ומציגה לי את שאר החברים  
        public IActionResult AddFriend(Friend friend)
        {
            Data.Get.Friends.Add(friend);
            Data.Get.SaveChanges();
            return RedirectToAction("Friends");
        }

        // פונקציה שפותחת את הפרטים של החבר 
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Friends");
            }

            Friend? friend = Data.Get.Friends.Include(f => f.Images).FirstOrDefault(Friends => Friends.Id == id);
            
            if (friend == null)
            {
                return RedirectToAction("Friends");
            }
            return View(friend);
        }

        // פונקציה שפותחת את העריכה של החבר 
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Friends");
            }
            Friend? friend = Data.Get.Friends.FirstOrDefault(Friends => Friends.Id == id);
            if (friend == null)
            {
                return RedirectToAction("Friends");
            }
            return View(friend);
        }

        // פונקציה שמעדכנת פרטים
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateFriend(Friend newFriend)
        {
            if (newFriend == null)
            {
                return RedirectToAction("Friends");
            }

            Friend? eFriend = Data.Get.Friends.FirstOrDefault(f => f.Id == newFriend.Id);

            if (eFriend == null)
            {
                return RedirectToAction("Friends");
            }
            // עדכון החבר מהדאטה בייס עם הפרטים החדשים
            Data.Get.Entry(eFriend).CurrentValues.SetValues(newFriend);
            Data.Get.SaveChanges();
            return RedirectToAction("Friends");
        }

        // פונקציה שמוחקת חבר מהרשימה
        public IActionResult DeleteFriend(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Friend> friendList = Data.Get.Friends.ToList();
            Friend? friendToRemove = friendList.Find(friend => friend.Id == id);
            if (friendToRemove == null)
            {
                return NotFound();
            }
            Data.Get.Friends.Remove(friendToRemove);
            Data.Get.SaveChanges();
            return RedirectToAction(nameof(Friends));
        }

        // פונקציה להוספת תמונה לחבר שקיים במערכת
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddNewImage(Friend friend)
        {
            // מביא את פרטי החבר מהדאטה בייס 
            Friend? friendFromDb = Data.Get.Friends.FirstOrDefault(f => f.Id == friend.Id);
            // בודק האם החבר בכלל קיים
            if (friendFromDb == null)
            {
                return NotFound();
            }
            // לוקח את התמונה שהכנסתי ומכניס אותה למקום הראשון
            byte[]? firstImage = friend.Images.Last().MyImage;
            if (firstImage == null)
            {
                return NotFound();
            }
            // מבצע הוספה לרשימה, וגם מבצע שמירה ת ולבסוף אני מחזיר את המשתמש למסך של החבר    
            friendFromDb.AddImage(firstImage);
            Data.Get.SaveChanges();
            return RedirectToAction("Details", new { id = friendFromDb.Id });
        }

        // פונקציה שפותחת\מחזירה את הדף הראשי
        public IActionResult Index()
        {
            return View();
        }

        // פונקציה שפותחת\מחזירה את הדף של הפרטיות
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
