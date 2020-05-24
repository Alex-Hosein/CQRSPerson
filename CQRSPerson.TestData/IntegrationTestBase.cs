using AutoMapper;
using CQRSPerson.API.Map;
using CQRSPerson.API.Person.Command;
using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.Domain.Options;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.Infrastructure;
using CQRSPerson.Infrastructure.Logging;
using CQRSPerson.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CQRSPerson.TestData
{
    [TestFixture]
    public class IntegrationTestBase
    {
        public IOptions<ConnectionStrings> ConnectionStrings { get; set; }
        public IMapper Mapper;
        public DBContext DBContext;
        public IPersonQueryRepository PersonQueryRepository { get; set; }
        public IPersonCommandRepository PersonCommandRepository { get; set; }
        public ApplicationLogger<GetPersonsHandler> GetPersonsHandlerLogger { get; set; }
        public ApplicationLogger<CreatePersonHandler> CreatePersonHandlerLogger { get; set; }
        public IValidator<CreatePersonCommand> CreatePersonValidator { get; set; }
        public GetPersonsHandler GetPersonsHandler { get; set; }
        public CreatePersonHandler CreatePersonHandler { get; set; }

        public IntegrationTestBase()
        {
            SetupEverything();
        }

        public void SetupEverything()
        {
            SetupOptions();
            SetupDBContext();
            SetupLoggers();
            SetupMapper();
            SetupRepositories();
            SetupValidators();
            SetupHandlers();
        }

        public void SetupOptions()
        {
            var connectionStrings = new ConnectionStrings()
            {
                CQRSPersonDatabase = Constants.ConnectionStrings.CQRSPersonDatabaseConnectionString
            };

            ConnectionStrings = Options.Create(connectionStrings);
        }

        private void SetupDBContext()
        {
            DBContext = new DBContext(ConnectionStrings);
        }
        private void SetupLoggers()
        {
            GetPersonsHandlerLogger = new ApplicationLogger<GetPersonsHandler>(new Logger<GetPersonsHandler>(new LoggerFactory()));
            CreatePersonHandlerLogger = new ApplicationLogger<CreatePersonHandler>(new Logger<CreatePersonHandler>(new LoggerFactory()));
        }
        private void SetupMapper()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            Mapper = mappingConfig.CreateMapper();
        }

        public void SetupRepositories()
        {
            PersonQueryRepository = new PersonQueryRepository(DBContext);
            PersonCommandRepository = new PersonCommandRepository(DBContext);
        }
        public void SetupHandlers()
        {
            GetPersonsHandler = new GetPersonsHandler(PersonQueryRepository, Mapper, GetPersonsHandlerLogger);
            CreatePersonHandler = new CreatePersonHandler(CreatePersonValidator, CreatePersonHandlerLogger, PersonCommandRepository, Mapper);
        }
        public void SetupValidators()
        {
            CreatePersonValidator = new CreatePersonValidator();
        }
    }
}
