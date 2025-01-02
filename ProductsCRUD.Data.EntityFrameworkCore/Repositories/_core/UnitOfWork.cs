using ProductsCRUD.Data.EntityFrameworkCore.Context;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories.Products;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories.Promotions;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.AdminUsers;
using ProductsCRUD.Domain.Products;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Data.EntityFrameworkCore.Repositories._core
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;


        private IRepository<AdminUser> _adminUserRepository;
        public IRepository<AdminUser> AdminUserRepository
        {
            get
            {
                return _adminUserRepository ??= new Repository<AdminUser>(_context);
            }
        }

        private IRepository<ProductPromotion> _productPromotionRepository;
        public IRepository<ProductPromotion> ProductPromotionRepository
        {
            get
            {
                return _productPromotionRepository ??= new Repository<ProductPromotion>(_context);
            }
        }

        private IRepository<PromotionType> _promotionTypeRepository;
        public IRepository<PromotionType> PromotionTypeRepository
        {
            get
            {
                return _promotionTypeRepository ??= new Repository<PromotionType>(_context);
            }
        }




        private IPromotionRepository _promotionRepository;
        public IPromotionRepository PromotionRepository
        {
            get
            {
                return _promotionRepository ??= new PromotionRepository(_context);
            }
        }

        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }








        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
