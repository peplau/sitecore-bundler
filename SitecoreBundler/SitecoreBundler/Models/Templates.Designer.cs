
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#pragma warning disable 1591

namespace SitecoreBundler.Models.Templates
{
  #region Designer generated code

  using System;
  using Sitecore;
  using Sitecore.Diagnostics;
  using Sitecore.Data.Items;
  using Sitecore.Data.Fields;
  using Sitecore.Data;
  
      
  /// <summary>Represents the "_Base Bundle Group" template.</summary>
  public partial class __BaseBundleGroup : CustomItem
  {
    public static readonly ID TemplateID = ID.Parse("{8F640C0E-BA86-41A5-9E95-8EAB419072A7}");

    public __BaseBundleGroup(Item item) : base(item) {
    }

    public static class FieldIds {
      
      public static readonly ID Bundles = ID.Parse("{108171BF-254E-4987-89F4-5926ACD9F14E}");

      public static readonly ID BundledFilename = ID.Parse("{C82404D1-FD37-4BEE-A2B3-B51237F39F95}");

      public static readonly ID Bundle = ID.Parse("{C7630412-27CA-4A28-A8AF-485CB2B543E6}");

      public static readonly ID Minify = ID.Parse("{F81E5C28-3DF7-4766-901D-54A9C9BFA1C5}");

      public static readonly ID AgressiveCache = ID.Parse("{AEAB6ABC-ECAA-4168-9935-07F77393C0DE}");

    }
    
    /// <summary>Gets or sets the "Bundles" field.</summary>
    public string Bundles 
    {
      get 
      {
        return this.InnerItem[FieldIds.Bundles];
      }
      set
      {
        this.InnerItem[FieldIds.Bundles] = value;
      }
    }
  
    /// <summary>Gets or sets the "Bundled Filename" field.</summary>
    public string BundledFilename 
    {
      get 
      {
        return this.InnerItem[FieldIds.BundledFilename];
      }
      set
      {
        this.InnerItem[FieldIds.BundledFilename] = value;
      }
    }
  
    /// <summary>Gets or sets the "Bundle" field.</summary>
    public bool Bundle 
    {
      get 
      {
        return MainUtil.GetBool(this.InnerItem[FieldIds.Bundle], false);
      }
      set 
      {
        this.InnerItem[FieldIds.Bundle] = value ? "1" : string.Empty;
      }
    }
  
    /// <summary>Gets or sets the "Minify" field.</summary>
    public bool Minify 
    {
      get 
      {
        return MainUtil.GetBool(this.InnerItem[FieldIds.Minify], false);
      }
      set 
      {
        this.InnerItem[FieldIds.Minify] = value ? "1" : string.Empty;
      }
    }
  
    /// <summary>Gets or sets the "Agressive Cache" field.</summary>
    public bool AgressiveCache 
    {
      get 
      {
        return MainUtil.GetBool(this.InnerItem[FieldIds.AgressiveCache], false);
      }
      set 
      {
        this.InnerItem[FieldIds.AgressiveCache] = value ? "1" : string.Empty;
      }
    }
  
    public static __BaseBundleGroup Create(Item item) 
    {
      return new __BaseBundleGroup(item);
    }

    public static implicit operator Item (__BaseBundleGroup item)
    {
      if (item == null)
      {
        return null;
      }

      return item.InnerItem;
    }

    public static explicit operator __BaseBundleGroup(Item item)
    {
      if (item == null)
      {
        return null;
      }

      if (item.TemplateID != TemplateID)
      {
        return null;
      }

      return Create(item);
    }
  }
      
  /// <summary>Represents the "Bundler" template.</summary>
  public partial class Bundler : CustomItem
  {
    public static readonly ID TemplateID = ID.Parse("{1E8FC351-FD7F-4BC8-98BF-BC114D1C7912}");

    public Bundler(Item item) : base(item) {
    }

    public static class FieldIds {
      
      public static readonly ID Layout = ID.Parse("{4D311A3D-F02B-4AD3-B876-EC7BD3A508D6}");

      public static readonly ID BundleLocalPath = ID.Parse("{65931423-93F8-440B-A292-6E2B38D3538B}");

      public static readonly ID Bundle = ID.Parse("{89C046C2-4DF4-4AF4-B3AC-69E477279EA3}");

      public static readonly ID Minify = ID.Parse("{B70270E3-EA39-468F-9DB1-A14D2BD097EA}");

      public static readonly ID AgressiveCache = ID.Parse("{E7F1FF80-206D-46DC-8D92-97A92DD958B6}");

    }
    
    /// <summary>Gets the "Layout" field.</summary>
    public ReferenceField Layout 
    {
      get 
      {
        return this.InnerItem.Fields[FieldIds.Layout];
      }
    }
  
    /// <summary>Gets or sets the "Bundle Local Path" field.</summary>
    public string BundleLocalPath 
    {
      get 
      {
        return this.InnerItem[FieldIds.BundleLocalPath];
      }
      set
      {
        this.InnerItem[FieldIds.BundleLocalPath] = value;
      }
    }
  
    /// <summary>Gets the "Bundle" field.</summary>
    public LookupField Bundle 
    {
      get 
      {
        return this.InnerItem.Fields[FieldIds.Bundle];
      }
    }
  
    /// <summary>Gets the "Minify" field.</summary>
    public LookupField Minify 
    {
      get 
      {
        return this.InnerItem.Fields[FieldIds.Minify];
      }
    }
  
    /// <summary>Gets the "Agressive Cache" field.</summary>
    public LookupField AgressiveCache 
    {
      get 
      {
        return this.InnerItem.Fields[FieldIds.AgressiveCache];
      }
    }
  
    public static Bundler Create(Item item) 
    {
      return new Bundler(item);
    }

    public static implicit operator Item (Bundler item)
    {
      if (item == null)
      {
        return null;
      }

      return item.InnerItem;
    }

    public static explicit operator Bundler(Item item)
    {
      if (item == null)
      {
        return null;
      }

      if (item.TemplateID != TemplateID)
      {
        return null;
      }

      return Create(item);
    }
  }
      
  /// <summary>Represents the "Javascript Bundler" template.</summary>
  public partial class JavascriptBundler : CustomItem
  {
    public static readonly ID TemplateID = ID.Parse("{941080E4-0E64-4BD4-B216-B7A85A659BE6}");

    public JavascriptBundler(Item item) : base(item) {
    }

    public static class FieldIds {
      
    }
    
    public static JavascriptBundler Create(Item item) 
    {
      return new JavascriptBundler(item);
    }

    public static implicit operator Item (JavascriptBundler item)
    {
      if (item == null)
      {
        return null;
      }

      return item.InnerItem;
    }

    public static explicit operator JavascriptBundler(Item item)
    {
      if (item == null)
      {
        return null;
      }

      if (item.TemplateID != TemplateID)
      {
        return null;
      }

      return Create(item);
    }
  }
      
  /// <summary>Represents the "Css Bundler" template.</summary>
  public partial class CssBundler : CustomItem
  {
    public static readonly ID TemplateID = ID.Parse("{31C4438D-D6C6-4CF5-819A-3755CC29D2C1}");

    public CssBundler(Item item) : base(item) {
    }

    public static class FieldIds {
      
    }
    
    public static CssBundler Create(Item item) 
    {
      return new CssBundler(item);
    }

    public static implicit operator Item (CssBundler item)
    {
      if (item == null)
      {
        return null;
      }

      return item.InnerItem;
    }

    public static explicit operator CssBundler(Item item)
    {
      if (item == null)
      {
        return null;
      }

      if (item.TemplateID != TemplateID)
      {
        return null;
      }

      return Create(item);
    }
  }
      
  /// <summary>Represents the "Dictionary entry" template.</summary>
  public partial class DictionaryEntry : CustomItem
  {
    public static readonly ID TemplateID = ID.Parse("{6D1CD897-1936-4A3A-A511-289A94C2A7B1}");

    public DictionaryEntry(Item item) : base(item) {
    }

    public static class FieldIds {
      
      public static readonly ID Key = ID.Parse("{580C75A8-C01A-4580-83CB-987776CEB3AF}");

      public static readonly ID Phrase = ID.Parse("{2BA3454A-9A9C-4CDF-A9F8-107FD484EB6E}");

    }
    
    /// <summary>Gets or sets the "Key" field.</summary>
    public string Key 
    {
      get 
      {
        return this.InnerItem[FieldIds.Key];
      }
      set
      {
        this.InnerItem[FieldIds.Key] = value;
      }
    }
  
    /// <summary>Gets or sets the "Phrase" field.</summary>
    public string Phrase 
    {
      get 
      {
        return this.InnerItem[FieldIds.Phrase];
      }
      set
      {
        this.InnerItem[FieldIds.Phrase] = value;
      }
    }
  
    public static DictionaryEntry Create(Item item) 
    {
      return new DictionaryEntry(item);
    }

    public static implicit operator Item (DictionaryEntry item)
    {
      if (item == null)
      {
        return null;
      }

      return item.InnerItem;
    }

    public static explicit operator DictionaryEntry(Item item)
    {
      if (item == null)
      {
        return null;
      }

      if (item.TemplateID != TemplateID)
      {
        return null;
      }

      return Create(item);
    }
  }
  
  #endregion
}

#pragma warning restore 1591
