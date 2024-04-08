using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;


namespace FurnitureStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public override void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Category = obj.Category;
                objFromDb.ProductCode = obj.ProductCode;
                objFromDb.Supplier = obj.Supplier;
                objFromDb.Manufacturer = obj.Manufacturer;
                objFromDb.Stock = obj.Stock;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price = 0;
                objFromDb.IsAvailableForPurchase = obj.IsAvailableForPurchase;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
