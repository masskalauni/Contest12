using System.Collections.Generic;
using Contest.Web.Constants;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contest.Web.Areas.Docs.Pages.About
{
    public class RoleModel : PageModel
    {
        public void OnGet()
        {
            RoleList = new List<RoleModel>
            {
                new RoleModel 
                { 
                    RoleTitle = Role.Participant, 
                    Description = "Participant role is assigned by default to just registerd user.",
                    AllowedFeatures = new [] 
                    { 
                        AppFeature.RegisterTeam, 
                        AppFeature.ModifyTeamDetails,
                        AppFeature.DeleteTeam,
                        AppFeature.SeeAllTeamDetails,
                        AppFeature.SeeAllTeamSummary,
                        AppFeature.VoteOnPeoplesChoiceAward,
                        AppFeature.PeoplesChoiceAwardResults
                    }
                },

                new RoleModel
                {
                    RoleTitle = Role.Admin,
                    Description = "Admins are normally judges, committee members who have more privilege on app features",
                    AllowedFeatures = new []
                    {
                        AppFeature.ModifyTeamsDetails,
                        AppFeature.DeleteTeams,
                        AppFeature.JudgeTeam,
                        AppFeature.SeeAllTeamEvaluationDetails,
                        AppFeature.SeeFinalResults,
                        AppFeature.CreateSurvey,
                        AppFeature.SeeSurveyResponses
                    }
                },

                new RoleModel
                {
                    RoleTitle = Role.SuperAdmin,
                    Description = "Super Admins have app configuration privileges and have access to all the features.",
                    AllowedFeatures = new []
                    {
                        AppFeature.ConfigureSettings,
                        AppFeature.AssignRoles,
                        AppFeature.AddRole
                    }
                },
            };
        }

        public string RoleTitle { get; set; }
        public string Description { get; set; }
        public string[] AllowedFeatures { get; set; }

        public List<RoleModel> RoleList { get; set; }
    }
}
