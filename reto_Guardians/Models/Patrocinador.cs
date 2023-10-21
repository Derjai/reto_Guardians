using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

[Table("Patrocinador")]
public partial class Patrocinador
{
    [Key]
    [Column("id_patrocinador")]
    public int IdPatrocinador { get; set; }

    [Column("id_heroe")]
    public int IdHeroe { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("origen")]
    [StringLength(50)]
    [Unicode(false)]
    public string Origen { get; set; } = null!;

    [Column("monto")]
    public double Monto { get; set; }

    [JsonIgnore]
    [ForeignKey("IdHeroe")]
    [InverseProperty("Patrocinadors")]
    public virtual Heroe IdHeroeNavigation { get; set; } = null!;
}
