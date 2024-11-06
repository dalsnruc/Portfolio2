using DataLayer;
using WebApi.Models.TitleModels;

namespace WebApi.Models.UserRatingModels
{
    public class UserRatingModel
    {
        public int Rating { get; set; }
        public UserTitleModel Title { get; set; }

    }
}
