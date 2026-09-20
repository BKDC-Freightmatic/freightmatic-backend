namespace Freightmatic.Domain.Files
{
    public class File
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public byte[] Content { get; set; }
        public File(string title, string url, byte[] content)
        {
            Title = title;
            Url = url;
            Content = content;
        }
    }
}
