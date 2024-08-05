namespace MyFriend.DAL
{
    // מחלקה לניהול של החברים שלי
    public class Data
    {
        // משתנה סטטי לשמירת מופע יחיד של מחלקה
        static Data GetData;

        // מחרוזת לחיבור לבסיס הנתונים
        string ConnectionString = "" +
                                "server = MIKI_LEVI\\SQLEXPRESS;" +
                                " initial catalog = MyFriend;" +
                                " user id = sa ;" +
                                " password = 1234;" +
                                " TrustServerCertificate = Yes";

        // קונסטרקטור פרטי למניעת יצירת מופעים מחוץ למחלקה
        private Data()
        {
            // יצירת מופע של שכבת נתונים עם מחרוזת החיבור
            Layer = new DataLayer(ConnectionString);
        }

        // מאפיין סטטי לקבלת שכבת נתונים
        public static DataLayer Get
        {
            get
            {
                // יצירת מופע חדש של מחלקה במידה ולא קיים
                if (GetData == null)
                {
                    GetData = new Data();
                }
                // החזרת שכבת הנתונים
                return GetData.Layer;
            }
        }

        // מאפיין לשמירת שכבת בסיס הנתונים
        public DataLayer Layer { get; set; }


    }
}
