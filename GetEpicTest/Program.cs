using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Threading.Tasks;

namespace GetEpicTest
{
    class Program
    {
        public static string auth = "";
        public static string outtype = "img";
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("====================== No arguments found! ======================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-a=PHPSESSID - For your PHPSESSID token. requires a valid subscription");
                Console.WriteLine("https://i.imgur.com/dgjJRj2.png");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-bid=BOOKID - ID of the book you wish to download.");
                Console.WriteLine("IDs can be found in the url of the book, for example, ");
                Console.WriteLine("https://www.getepic.com/app/read/BOOKID");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-t=pdf|img - Used to select the output format, default img.");
                Console.WriteLine("=============================================================================");
                Console.WriteLine("Press return/enter to exit.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                List<string> bookids = new List<string>();

                foreach (var argument in args)
                {
                    if (argument.StartsWith("-a="))
                    {
                        auth = argument.Split('=')[1];
                    }
                    else if (argument.StartsWith("-bid="))
                    {
                        bookids.Add(argument.Split('=')[1]);
                    }
                    else if (argument.StartsWith("-t="))
                    {
                        outtype = argument.Split('=')[1];
                    }
                }

                if (auth == "")
                {
                    Console.WriteLine("No auth code found!!");
                    Console.WriteLine("make sure you use an auth code with -a=");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    foreach (string id in bookids)
                    {
                        getBook(id);
                    }
                }
            }

            Console.WriteLine("Finsihed.");
            Console.WriteLine("Press return to exit.");
            Console.ReadLine();
        }

        public static void getBook(string id)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" + auth);
                string bookjson = client.DownloadString("https://api.getepic.com/webapi/index.php?class=WebBook&method=getFullDataForWeb&bookId=" + id);
                Book.Info bookInfo = null;
                try
                {
                    bookInfo = JsonConvert.DeserializeObject<Book.Info>(bookjson);
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't parse json correctly, probably incorrect PHPSESSID");
                    Console.WriteLine("Press return to exit.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                List<string> imgUrls = new List<string>();
                foreach (var page in bookInfo.Result.Epub.Spine)
                {
                    string finalImguRL = getImgUrl("/" + page.Page);
                    imgUrls.Add(finalImguRL);
                }

                if (outtype == "img")
                {
                    getImg(imgUrls, bookInfo.Result.Book.Title, bookInfo.Result.Book.Author);
                }
                else if (outtype == "pdf")
                {
                    GetPDF(imgUrls, bookInfo.Result.Book.Title, bookInfo.Result.Book.Author);
                }
            }
        }

        public static string getImgUrl(string imageurl)
        {
            //THESE TOKENS / SALTS MIGHT CHANGE, I HAVE NO IDEA. 
            var token = substring("kje4fc6f017797e20e3gfdoijdro3498u") + substring("ed8933800968d99bb73fgdjgh09234kdk");
            DateTimeOffset now = DateTimeOffset.UtcNow;
            long epochtime = now.ToUnixTimeMilliseconds();

            string i = CreateMD5(imageurl + "?ttl=" + epochtime + "&auth=" + token);
            string url = "https://cdn.getepic.com/" + imageurl + "?ttl=" + epochtime + "&token=" + i;
            return url;
        }

        static void getImg(List<string> imgurls, string bookname, string authorname)
        {
            bookname = MakeValidFileName(bookname).Trim();
            authorname = MakeValidFileName(authorname).Trim();

            Console.WriteLine("Downloading: " + bookname);

            Directory.CreateDirectory(bookname);

            using (var client = new WebClient())
            {
                int pagenum = 1;
                foreach (var img in imgurls)
                {
                    string filename = bookname + "\\" + bookname + " - " + authorname + " - page-" + pagenum.ToString("000") + ".jpg";
                    if (!File.Exists(filename))
                    {
                        Console.Write("\rPage: " + (pagenum));
                        byte[] data = client.DownloadData(img);
                        File.WriteAllBytes(filename, data);
                    }
                    else
                    {
                        Console.Write("\rPage: " + filename + " - Already exists!");
                    }
                    pagenum++;
                }
                Console.WriteLine("\nFinished: " + bookname + " - " + authorname);
            }
        }

        static void GetPDF(List<string> imgurls, string bookname, string authorname)
        {
            bookname = MakeValidFileName(bookname).Trim();
            authorname = MakeValidFileName(authorname).Trim();

            Console.WriteLine("Downloading: " + bookname);

            string filename = bookname + " - " + authorname + ".pdf";

            if (!File.Exists(filename))
            {
                using (var client = new WebClient())
                {
                    int pagenum = 1;
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(bookname + " - " + authorname + ".pdf"));

                    foreach (var img in imgurls)
                    {
                        Console.Write("\rPage: " + (pagenum));
                        byte[] data = client.DownloadData(img);

                        Image image = new Image(ImageDataFactory.Create(data));
                        Document doc = new Document(pdfDoc, new PageSize(image.GetImageWidth(), image.GetImageHeight()));

                        image = new Image(ImageDataFactory.Create(data));
                        pdfDoc.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                        image.SetFixedPosition(pagenum, 0, 0);
                        doc.Add(image);
                        pagenum++;
                    }

                    pdfDoc.Close();
                    Console.WriteLine("\nFinished: " + bookname + " - " + authorname);
                }
            }
            else
            {
                Console.WriteLine(filename + " - Already exists!");
            }
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "");
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string substring(string input)
        {
            return input.Substring(3, 16);
        }
    }
}
