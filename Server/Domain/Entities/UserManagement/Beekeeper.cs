using API.Domain.Entities.PlotManagement;
using System.Collections.ObjectModel;

namespace API.Domain.Entities.UserManagement
{
    public interface Beekeeper
    {
        public Collection<CropType> honeyType { get; set; }
    }
}
