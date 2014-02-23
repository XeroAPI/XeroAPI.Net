namespace XeroApi.Model
{
    public class TaxComponent : ModelBase
    {

        public string Name { get; set; }

        public decimal Rate { get; set; }

        public bool IsCompound { get; set; }

        public override string ToString()
        {
            return string.Format("TaxComponent:{0}", Name);
        }
    }

    public class TaxComponents : ModelList<TaxComponent>
    {

    }
}
