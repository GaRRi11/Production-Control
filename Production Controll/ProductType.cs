namespace Production_Controll
{
    internal class ProductType
    {
        public long id { get; set; }
        public string name { get; set; }
        public double liters { get; set; }
        public string version { get; set; }

        public ProductType(
            long id,
            string name,
            string version,
            double liters)
        {
            this.id = id;
            this.name = name;
            this.liters = liters;
            this.version = version;
        }
    }
}
