using System.Collections.ObjectModel;
using PPPAMI.Models;

namespace PPPAMI.Models
{
    public class Team
    {
        public ObservableCollection<Character> TeamMembers { get; set; }
            = new ObservableCollection<Character>();

        public Team()
        {
            TeamMembers = new ObservableCollection<Character>();
        }
    }
}
