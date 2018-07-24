using Ex2.BL.Entities;
using Ex2.BL.Entities.Builders;
using Ex2.BL.Exceptions;
using Ex2.Infrastructure.Abstractions;
using Ex2.Infrastructure.Interfaces;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using static NUnit.StaticExpect.Expectations;

namespace Ex2.Tests
{
    [TestFixture]
    public class PipeLineBuilderTests
    {
        private const string UseHandleErrorMessage = "Method UseHandle works incorrect";
        private const string ClearErrorMessage = "Method Clear works incorrect";
        private const string BuildErrorMessage = "Method Build works incorrect";

        private IPipeLineHandler<object> _pipeLineHandler;
        private PipeLineBuilder<object> _builder;

        [SetUp]
        public void SetUp()
        {
            _pipeLineHandler = Substitute.For<PipeLineHandler<object>>();
            _builder = new PipeLineBuilder<object>();
        }

        [Test]
        public void UseHandler_Should_ThrowArgumentNullException_When_SetNullReferenceHandler()
        {
            Expect(() => _builder.UseHandler(null), Throws.ArgumentNullException, UseHandleErrorMessage);
        }

        [Test]
        public void UseHandler_Should_ThrowNothing_When_SetHandler()
        {

            Expect(() => _builder.UseHandler(_pipeLineHandler), Throws.Nothing, UseHandleErrorMessage, UseHandleErrorMessage);
        }

        [Test]
        public void UseHandler_Should_ThrowArgumentException_When_HandlerAlreadyUsed()
        {
            _builder.UseHandler(_pipeLineHandler);

            Expect(() => _builder.UseHandler(_pipeLineHandler), Throws.ArgumentException, UseHandleErrorMessage);
        }

        [Test]
        public void Clear_Should_ThrowNothing()
        {
            Expect(() => _builder.Clear(), Throws.Nothing, ClearErrorMessage);
        }

        [Test]
        public void Build_Should_ThrowEmptyCollectionException_When_HandlersWereNotUsed()
        {
            Expect(()=> _builder.Build(), Throws.TypeOf<EmptyCollectionException>(), BuildErrorMessage);
        }

        [Test]
        public void Build_Should_ThrowNothing_When_HandlersWereUsedBeforeBuild()
        {
            _builder.UseHandler(_pipeLineHandler);

            Expect(() => _builder.Build(), Throws.Nothing, BuildErrorMessage);
        }

        [Test]
        public void Build_Should_ThrowEmptyCollectionException_When_HandlersWereClearedBeforeBuild()
        {
            _builder.UseHandler(_pipeLineHandler);
            _builder.Clear();

            Expect(() => _builder.Build(), Throws.TypeOf<EmptyCollectionException>(), BuildErrorMessage);
        }
    }
}