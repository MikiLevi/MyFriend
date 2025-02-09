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

        // ������� ������� �� ������ ��� ���
        public IActionResult Create()
        {
            return View(new Friend());
        }

        [HttpPost, ValidateAntiForgeryToken]
        // ������� ������ ���� ��� ������� ����� ���� ������ �� �� ��� ������  
        public IActionResult AddFriend(Friend friend)
        {
            Data.Get.Friends.Add(friend);
            Data.Get.SaveChanges();
            return RedirectToAction("Friends");
        }

        // ������� ������ �� ������ �� ���� 
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

        // ������� ������ �� ������ �� ���� 
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

        // ������� ������� �����
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
            // ����� ���� ������ ���� �� ������ ������
            Data.Get.Entry(eFriend).CurrentValues.SetValues(newFriend);
            Data.Get.SaveChanges();
            return RedirectToAction("Friends");
        }

        // ������� ������ ��� �������
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

        // ������� ������ ����� ���� ����� ������
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddNewImage(Friend friend)
        {
            // ���� �� ���� ���� ������ ���� 
            Friend? friendFromDb = Data.Get.Friends.FirstOrDefault(f => f.Id == friend.Id);
            // ���� ��� ���� ���� ����
            if (friendFromDb == null)
            {
                return NotFound();
            }
            // ���� �� ������ ������� ������ ���� ����� ������
            byte[]? firstImage = friend.Images.Last().MyImage;
            if (firstImage == null)
            {
                return NotFound();
            }
            // ���� ����� ������, ��� ���� ����� � ������ ��� ����� �� ������ ���� �� ����    
            friendFromDb.AddImage(firstImage);
            Data.Get.SaveChanges();
            return RedirectToAction("Details", new { id = friendFromDb.Id });
        }

        // ������� ������\������ �� ��� �����
        public IActionResult Index()
        {
            return View();
        }

        // ������� ������\������ �� ��� �� �������
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
