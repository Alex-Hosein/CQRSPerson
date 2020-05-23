using CQRSPerson.Domain.Entities;
using CQRSPerson.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

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
