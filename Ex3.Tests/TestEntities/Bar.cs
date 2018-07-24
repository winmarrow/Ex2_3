using Ex3.Tests.TestInterfaces;

namespace Ex3.Tests.TestEntities
{
    public class Bar : IBar
    {
        public Bar(Foo foo)
        {
            Foo = foo;
        }

        public Foo Foo { get; set; }
    }
}