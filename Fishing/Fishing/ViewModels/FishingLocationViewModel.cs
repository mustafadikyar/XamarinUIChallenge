using Fishing.Models;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Fishing.ViewModels
{
    public class FishingLocationViewModel : BaseViewModel
    {
        private int peopleToShow = 3;
        private FishingLocationModel _location;
        public FishingLocationViewModel(FishingLocationModel location)
        {
            _location = location;
        }

        public FishingLocationModel Location
        {
            get => _location;
            set => _location = value;
        }

        public string PeopleAtLocation
        {
            get
            {
                var peopleCount = _location.People.Count;
                var first = _location.People.FirstOrDefault();
                if (first == null)
                    return "It's just you";

                var names = _location.People.Select(x => x.Name).OrderBy(o => o).Take(peopleToShow).ToList();
                string nameList = string.Join(", ", names);

                if (peopleCount > peopleToShow)
                    return $"{nameList} and {peopleCount - peopleToShow} others";
                else
                    return nameList;
            }
        }

        public List<object> PeopleIcons
        {
            get
            {
                var peopleCount = _location.People.Count;

                List<object> returnList = new List<object>();
                returnList.AddRange(_location.People.Take(peopleToShow));

                // do we need a more badge
                if (peopleCount > peopleToShow)
                {
                    returnList.Add(peopleCount - peopleToShow);
                }
                return returnList;
            }
        }        
    }
}