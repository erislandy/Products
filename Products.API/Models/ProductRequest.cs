

namespace Products.API.Models
{
    using Products.Domain;
    public class ProductRequest : Product
    {
        public byte[] ImageArray { get; set; }
    }
}