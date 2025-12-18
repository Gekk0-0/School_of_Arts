using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace School_of_arts.ModelBinding
{
    public sealed class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var t = context.Metadata.ModelType;
            if (t == typeof(decimal) || t == typeof(decimal?))
                return new BinderTypeModelBinder(typeof(DecimalModelBinder));
            return null;
        }
    }

}
