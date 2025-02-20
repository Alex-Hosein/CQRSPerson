using AutoMapper;
using CQRSPerson.API.Map;
using CQRSPerson.Domain.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace CQRSPerson.TestData
{
    [TestFixture]
    public abstract class UnitTestBase
    {
        public Exception Exception { get; set; }
        public IMapper Mapper { get; set; }
        public UnitTestBase()
        {
            Exception = new Exception("This is only a test");
            SetupMapper();
        }
        public static Mock<IApplicationLogger<T>> SetupLoggerMock<T>()
        {
            var loggerMock = new Mock<IApplicationLogger<T>>();
            loggerMock.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();
            loggerMock.Setup(x => x.LogError(It.IsAny<string>())).Verifiable();
            loggerMock.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();
            return loggerMock;
        }

        private void SetupMapper()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            Mapper = mappingConfig.CreateMapper();
        }

    }
}