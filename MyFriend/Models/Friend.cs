using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriend.Models
{
    public class Friend
    {
        public Friend()
        {
            Images = new List<Image>();
        }

        [Key]

        public int Id { get; set; }

        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "שם משפחה")]
        public string LastName { get; set; } = string.Empty;
        
        [Display(Name = "שם מליין"), NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }

        [Display(Name = "מספר פלאפון"), Phone]
        public string PhoneNumber { get; set; } =string.Empty;

        [Display(Name = " כתובת אימייל"), EmailAddress(ErrorMessage = " ")]
        public string EmailAddress { get; set; }

        public List<Image> Images { get; set; }

        [Display(Name = " הוספת תמונה"), NotMapped]
        public IFormFile SetImage
        {
            get
            {
                return null;
            }
            set
            {
                if (value == null)
                {
                    return;                     
                }
                // יצירת מקום בזיכרון בשביל התמונה
                MemoryStream stream = new MemoryStream();
                // לוקח את התמונה ומכניס אותה
                value.CopyTo(stream);

                // המרת המקום בזיכרון שיצרנו לבייטים
                byte[] streamArray = stream.ToArray();

                // הוספת התמונה לרשימת התמונות של החבר
                AddImage(streamArray);
            } 
        }
        public void AddImage(byte[] image)
        {
            Image img = new()
            {
                MyImage = image,
                Friend = this
            };
            //img.MyImage = image;
            //img.Friend = this;
            Images.Add(img);
        }
    }
}
