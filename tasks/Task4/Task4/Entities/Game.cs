using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Task4.Entities
{
    #region Enum Genre
    public enum Genre
    {
        [Display(Name = "Unknown Genre", GroupName = "Unknown")]
        Unknown,

        [Display(Name = "Construction and management simulation", GroupName = "Simulation")]
        CityBuilder,

        [Display(Name = "Point & Click Adventure", GroupName = "Adventure")]
        PointAndClick,

        [Display(Name = "Graphic Adventure", GroupName = "Adventure")]
        GraphicAdventure,

        [Display(Name = "Text Adventure", GroupName = "Adventure")]
        TextAdventure,

        [Display(Name = "Shooter", GroupName = "Action")]
        Shooter,

        [Display(Name = "Stealth", GroupName = "Action")]
        Stealth,

        [Display(Name = "Massively multiplayer online role-playing games", GroupName = "Role-Playing")]
        Mmorpg,

        [Display(Name = "4x Strategy", GroupName = "Strategy")]
        Strategy4X
    }
    #endregion

    public class Game
    {
        public long AppId { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public Genre Genre { get; set; }

        public IEnumerable<NewsItem> NewsItems { get; set; } = Enumerable.Empty<NewsItem>();
    }
}
