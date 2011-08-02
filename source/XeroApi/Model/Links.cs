using System;

namespace XeroApi.Model
{
    public class Link : ModelBase
    {
        public Guid? LinkID { get;  set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }

    public class Links : ModelList<Link>
    {
    }
}
