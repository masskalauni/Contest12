namespace Contest.Web.Constants
{
    public class AppFeature
    {
        // Participants
        public const string RegisterTeam = "Register team for the event";
        public const string ModifyTeamDetails = "Modify team details for his/her team";
        public const string DeleteTeam = "Delete team he/she registered";
        public const string SeeAllTeamDetails = "See all registered teams for that year";
        public const string SeeAllTeamSummary = "See all teams synopsis";
        public const string VoteOnPeoplesChoiceAward = "Vote on people's choice award";
        public const string PeoplesChoiceAwardResults = "See final results for people's choice award voting polls";
        
        
        // Admins
        public const string ModifyTeamsDetails = "Modify any team details";
        public const string DeleteTeams = "Delete any team registered";
        public const string JudgeTeam = "Evaluate teams contested for the event, once pitching/demo is done";
        public const string SeeAllTeamEvaluationDetails = "See evaluation details for all teams.";
        public const string SeeFinalResults = "See final results for the event on different criteria e.g. Flat Average Score, Best Tech. Implementation etc.";
        public const string CreateSurvey = "Create new survey for any event";
        public const string SeeSurveyResponses = "See collected survey responses/feedbacks";


        // Super Admins
        public const string ConfigureSettings = "Configure feature settings e.g. Reveal final results for admins, allow team registration etc.";
        public const string AssignRoles = "Grant/revoke roles to/from registered users";
        public const string AddRole = "Add new role to auth";
    }
}
