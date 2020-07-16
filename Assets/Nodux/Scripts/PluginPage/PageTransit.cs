namespace Nodux.PluginPage
{
    public class PageTransit
    {
        public string FromPage;
        public string ToPage;

        public PageTransit(string from, string to)
        {
            this.FromPage = from;
            this.ToPage = to;
        }
    }
}