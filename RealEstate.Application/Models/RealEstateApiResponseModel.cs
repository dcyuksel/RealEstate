namespace RealEstate.Application.Models
{
    public class RealEstateApiResponseModel<T>
    {
        public Paging Paging { get; set; }
        public int TotaalAantalObjecten { get; set; }
        public IEnumerable<T> Objects { get; set; }
    }

    public class Paging
    {
        public int AantalPaginas { get; set; }
        public int HuidigePagina { get; set; }
    }
}
