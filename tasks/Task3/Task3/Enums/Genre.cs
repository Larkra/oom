using System.ComponentModel.DataAnnotations;

namespace Task3.Enums
{
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
        Strategy4X,

        [Display(Name = "Card game", GroupName = "Card game")]
        CardGame,

        [Display(Name = "Board game", GroupName = "Board Game")]
        BoardGame
    }
}
