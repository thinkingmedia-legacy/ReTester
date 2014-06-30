using ReTesterPlugin.Features.Filtering;
using ReTesterPlugin.Features.Naming;
using ReTesterPlugin.Features.Templates;

namespace ReTesterPlugin.Features
{
    public class FeatureType : iFeatureType
    {
        private readonly iFilter _filter;
        private readonly iTypeNaming _naming;
        private readonly iTemplateProvider _template;

        public FeatureType(iFilter pFilter, iTypeNaming pNaming, iTemplateProvider pTemplate)
        {
            _filter = pFilter;
            _naming = pNaming;
            _template = pTemplate;
        }

        public iFilter Filter
        {
            get { return _filter; }
        }

        public iTypeNaming Naming
        {
            get { return _naming; }
        }

        public iTemplateProvider Template
        {
            get { return _template; }
        }
    }
}