namespace Production_Controll
{
    internal class ProductType
    {
        public long id { get; set; }
        public string name { get; set; }
        public long litersId { get; set; }

        public ProductType(
            long id,
            string name,
            long litersId)
        {
            this.id = id;
            this.name = name;
            this.litersId = litersId;
        }
    }
}
