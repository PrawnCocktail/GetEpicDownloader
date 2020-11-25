using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEpicTest
{
    class Book
    {
        public class Info
        {
            public long Success { get; set; }
            public Result Result { get; set; }
            //public List<object> Messages { get; set; }
            //public string ApiResponseUuid4 { get; set; }
        }

        public class Result
        {
            //public PublisherData PublisherData { get; set; }
            public Epub Epub { get; set; }
            //public long TimePerPage { get; set; }
            public ResultBook Book { get; set; }
            //public UserBook UserBook { get; set; }
            //public object Quiz { get; set; }
        }

        public class ResultBook
        {
            //public long ModelId { get; set; }
            //public long Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Illustrator { get; set; }
            public long NumPages { get; set; }
            public string BookDescription { get; set; }
            //public long CoverColorR { get; set; }
            //public long CoverColorG { get; set; }
            //public long CoverColorB { get; set; }
            //public long Version { get; set; }
            //public long PreviewPercent { get; set; }
            //public string Publisher { get; set; }
            //public long PublisherId { get; set; }
            //public long Age { get; set; }
            //public long PageNumOffset { get; set; }
            //public long Active { get; set; }
            //public long Audio { get; set; }
            //public string Lexile { get; set; }
            //public string AvgTime { get; set; }
            //public long AvgTimeInt { get; set; }
            //public string Ar { get; set; }
            //public string Fandp { get; set; }
            //public long Dra { get; set; }
            //public string Gr { get; set; }
            //public long Rating { get; set; }
            //public long Type { get; set; }
            //public long Subject { get; set; }
            //public string SubjectDesc { get; set; }
            //public string SubjectColor { get; set; }
            //public string Copyright { get; set; }
            //public double AspectRatio { get; set; }
            //public string ContentHash { get; set; }
            //public long PayPerView { get; set; }
            //public long HighlightingEnabled { get; set; }
            //public long HighlightingStatus { get; set; }
            //public long DateModified { get; set; }
            //public string RecommendationUuid4 { get; set; }
            //public long ContentType { get; set; }
            //public long Language { get; set; }
            //public long PanelStatus { get; set; }
            //public long ReadingAgeMin { get; set; }
            //public long ReadingAgeMax { get; set; }
            //public long FreemiumBookUnlockStatus { get; set; }
            //public List<string> IsConverted { get; set; }
        }

        public class Epub
        {
            public List<Spine> Spine { get; set; }
            //public Extra Extra { get; set; }
        }

        //public partial class Extra
        //{
        //    public string Copyright { get; set; }
        //    public string Color { get; set; }
        //}

        public class Spine
        {
            public string Page { get; set; }
        }

        //public partial class UserBook
        //{
        //    public string ModelId { get; set; }
        //    public long UserId { get; set; }
        //    public long BookId { get; set; }
        //    public long CurrentPageIndex { get; set; }
        //    public long FarthestPageIndex { get; set; }
        //    public long Favorited { get; set; }
        //    public long FinishTime { get; set; }
        //    public long Paid { get; set; }
        //    public long Progress { get; set; }
        //    public long Rated { get; set; }
        //    public long Rating { get; set; }
        //    public long ReadTime { get; set; }
        //    public long Recommended { get; set; }
        //    public long LastModified { get; set; }
        //    public string Bookmarks { get; set; }
        //    public long TimesCompleted { get; set; }
        //    public long CurrentReadTime { get; set; }
        //    public long DateModified { get; set; }
        //    public UserBookBook Book { get; set; }
        //}

        //public partial class UserBookBook
        //{
        //    public long ModelId { get; set; }
        //    public string Title { get; set; }
        //    public long Type { get; set; }
        //    public long Active { get; set; }
        //    public long Audio { get; set; }
        //    public string RecommendationUuid4 { get; set; }
        //    public long ContentType { get; set; }
        //}
    }
}
