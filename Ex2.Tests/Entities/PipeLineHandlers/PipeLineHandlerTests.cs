using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.StaticExpect;

namespace Ex2.Tests.Entities.PipeLineHandlers
{
    public class PipeLineHandlerTests
    {
        [TestFixture]
        public class PipeLineHandlerTest
        {
            private const string SetNextErrorMessage = "Method SetNext works incorrect";

            private IPipeLineHandler<object> _pipeLineHandler;

            [SetUp]
            public void SetUp()
            {
                _pipeLineHandler = Substitute.For<PipeLineHandler<object>>();
            }

            [Test]
            public void SetNext_Should_ThrowArgumentNullException_When_SetNullReferenceHandler()
            {
                Expectations.Expect(() => _pipeLineHandler.SetNext(null), Throws.ArgumentNullException, SetNextErrorMessage);
            }

            [Test]
            public void SetNext_Should_ThrowArgumentException_When_SetSameHandler()
            {
                Expectations.Expect(() => _pipeLineHandler.SetNext(_pipeLineHandler), Throws.ArgumentException, SetNextErrorMessage);
            }

            [Test]
            public void SetNext_Should_DontThrowException_When_SetOtherHandler()
            {
                var next = Substitute.For<PipeLineHandler<object>>();

                Expectations.Expect(() => _pipeLineHandler.SetNext(next), Throws.Nothing, SetNextErrorMessage);
            }
        }
    }
}