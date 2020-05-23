using CQRSPerson.Domain.Entities;
using CQRSPerson.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Repositories
{
    public class PersonQueryRepository : IPersonQueryRepository
    {
        private DBContext _dbContext;
        public PersonQueryRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await _dbContext.Person.ToListAsync();
        }
    }
}
