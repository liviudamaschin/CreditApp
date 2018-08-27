using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace CreditAppBMG.CustomAttributes
{
    public class CustomMetadataProvider : IDisplayMetadataProvider
    {
        public CustomMetadataProvider() { }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.PropertyAttributes != null)
            {

                foreach (object propAttr in context.PropertyAttributes)
                {
                    HTMLMaskAttribute addMetaAttr = propAttr as HTMLMaskAttribute;
                    if (addMetaAttr != null)
                    {
                        context.DisplayMetadata.AdditionalValues.Add
                                      (addMetaAttr.Name, addMetaAttr.Value);
                    }
                }
            }
        }
    }
}
