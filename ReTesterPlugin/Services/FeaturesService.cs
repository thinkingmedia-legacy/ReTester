using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Features;
using ReTesterPlugin.Features.Filtering;
using ReTesterPlugin.Features.Naming;
using ReTesterPlugin.Features.Templates;

namespace ReTesterPlugin.Services
{
    public static class FeaturesService
    {
        public static readonly iFeatureType MockObjects = new FeatureType(new PublicOnly(), new MockObjectNaming(), new MockObjectTemplate<IClassDeclaration>());
        public static readonly iFeatureType MockInterfaces = new FeatureType(new PublicOnly(), new MockInterfaceNaming(), new MockObjectTemplate<IInterfaceDeclaration>());
        public static readonly iFeatureType UnitTests = new FeatureType(new PublicOnly(), new UnitTestNaming(), new UnitTestTemplate());
    }
}
