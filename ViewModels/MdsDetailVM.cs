using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class MdsDetailVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom {  get; set; }
      

    }
}
