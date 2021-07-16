// media: {
//     requestAddNewMediaObj,
//     season: [
//         {
//             requestAddNewSeasonObj: [
//                 {
//                     episode: requestAddNewEpisodeObj
//                 }
//             ]
//         }
//     ]
// },


let MultiAddObj = {
    UserEmail: "",
    Title: "",
    FilmType: "",
    Director: "",
    Cast: "",
    PublishYear: "",
    Duration: "",
    BannerURL: "",
    Language: "",
    Description: "",
    CategoryIDArray: [],
    seasons: [
        {
            title: "",
            ThumbnailURL: "",
            Number: "",
            UserEmail: "",
            episodes: [
                {
                    seasonID: "",
                    title: "",
                    ThumbnailURL: "",
                    episodeURL: "",
                    Number: "",
                    UserEmail: ""
                }
            ]
        }
    ]
}

