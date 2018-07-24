using System;
using Ex3.DI;
using Ex3.Tests.TestEntities;
using Ex3.Tests.TestInterfaces;
using NUnit.Framework;
using static NUnit.StaticExpect.Expectations;

namespace Ex3.Tests
{
    [TestFixture]
    public class DiContainerTests
    {
        [Test]
        public void DiContainerInstance_Should_BeeInitialized()
        {
            Expect(DiContainer.Instance, Not.Null);
        }

        [Test]
        public void RegisterByInstance_Should_DoNotThrowException()
        {
            Expect(()=> DiContainer.Instance.Register(new Foo()), Throws.Nothing);
        }

        [Test]
        public void RegisterByType_Should_DoNotThrowException()
        {
            Expect(() => DiContainer.Instance.Register<Foo>(), Throws.Nothing);
        }

        [Test]
        public void RegisterByInterface_Should_DoNotThrowException()
        {
            Expect(() => DiContainer.Instance.Register<IBar, Bar>(), Throws.Nothing);
        }

        [Test]
        public void GetInstanceByType_Should_ReturnRegistredInstance()
        {
            var foo = new Foo();
            DiContainer.Instance.Register(foo);
            Expect(DiContainer.Instance.GetInstance<Foo>(), SameAs(foo));
        }

        [Test]
        public void GetInstanceByType_Should_CreateNewInstance()
        {
            DiContainer.Instance.Register<Foo>();
            Expect(DiContainer.Instance.GetInstance<Foo>(), TypeOf<Foo>());
        }


        [Test]
        public void GetInstance_Should_ThrowArgumentException_When_MissingTypeRegistrations()
        {
            DiContainer.Instance.Register<IBar, Bar>();
            Expect(()=> DiContainer.Instance.GetInstance<IBar>(), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetInstanceByInterface_Should_CreateNewInstance()
        {
            DiContainer.Instance.Register<Foo>();
            DiContainer.Instance.Register<IBar, Bar>();
            Expect(DiContainer.Instance.GetInstance<IBar>(), TypeOf<Bar>());
        }
    }
}
