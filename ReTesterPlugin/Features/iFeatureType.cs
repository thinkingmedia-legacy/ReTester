using ReTesterPlugin.Features.Filtering;
using ReTesterPlugin.Features.Naming;
using ReTesterPlugin.Features.Templates;

namespace ReTesterPlugin.Features
{
    public interface iFeatureType
    {
        iFilter Filter { get; }
        iTypeNaming Naming { get; }
        iTemplateProvider Template { get; }
    }
}