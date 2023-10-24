namespace reto_Guardians.DTO
{
    public class CombateDTO
    {
        public int? IdCombate {  get; set; }
        public DateTime? Fecha { get; set; }
        public string? Lugar { get; set; }
        public string? Resultado { get; set; }
        public int? IdHeroe { get; set; }
        public string? Heroe { get; set; }
        public int? IdVillano { get; set; }
        public string? Villano { get; set; }
    }
}
