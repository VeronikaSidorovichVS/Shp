namespace Shops.Models
{
    public class fromData
    {
        public class Image
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public byte[] Data { get; set; } = null!;
        }
    }
}