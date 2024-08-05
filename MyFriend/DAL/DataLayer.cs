using Microsoft.EntityFrameworkCore;
using MyFriend.Models;  

namespace MyFriend.DAL
{
    // DbContext קלאס שמייצג את שכבת הנתונים שיורש מקלאס בשם
    public class DataLayer : DbContext
    {
        // קונסטרקטור שמקבל מחרוזת חיבור ומעביר לקונסטרקטור האב
        public DataLayer(string connectionString) : base(GetOptinos(connectionString))
        {
            // בודק אם יש דאטה בייס, ואם אין הוא מייצר  
            Database.EnsureCreated();

            // להכניס נתונים בפעם הראשונה
            Seed();
        }
        // פונקציה להכניס חבר ראשון לרשימה
        private void Seed()
        {
            // בודק האם הרשימה של החברים שלי ריקה
            if (Friends.Count() > 0) 
            {
                return;
            }
            Friend firstFriend = new Friend();
            firstFriend.FirstName = "שירה";
            firstFriend.LastName = "כהן";
            firstFriend.EmailAddress = "S1@GMAIL.COM";
            firstFriend.PhoneNumber = "0504343888";

            Friends.Add(firstFriend);
            SaveChanges();
        }
        // מייצר טבלא באס קיו אל בשביל החברים
        public DbSet<Friend> Friends { get; set; }

        // מייצר טבלא באס קיו אל בשביל התמונות של החברים שלי
        public DbSet<Image> Images { get; set; }


        // פונקיצה שמחזירה את האפשרויות למסד הנתונים
        private static DbContextOptions GetOptinos(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(),
                 connectionString).Options;
        }
    }
}
