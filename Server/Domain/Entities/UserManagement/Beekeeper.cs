using API.Domain.Entities.PlotManagement;
using System.Collections.ObjectModel;

namespace API.Domain.Entities.UserManagement
{
    public class Beekeeper  : ApplicationUser
    {
        public Collection<CropType> honeyType { get; set; }
    }
}
