using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ItemIdAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ItemNumberAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ItemUpdatedDateAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnlyAttribute : Attribute
    {
        
    }

    
    public interface IAttachmentParent
    {
    }
  
}
